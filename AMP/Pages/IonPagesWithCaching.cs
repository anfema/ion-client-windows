using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Anfema.Ion.Exceptions;
using Anfema.Ion.Parsing;
using Anfema.Ion.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Anfema.Ion.Pages
{
    public class IonPagesWithCaching : IIonPages, IIonConfigUpdateable
    {
        // Config associated with this collection of pages
        private IonConfig _config;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;

        // Different caching methods
        private MemoryCache _memoryCache;


        /// <summary>
        /// Constructor with configuration file for initialization
        /// </summary>
        /// <param name="config"></param>
        public IonPagesWithCaching( IonConfig config )
        {
            _config = config;

            // Init the data client
            _dataClient = new DataClient( config );

            // Init memory cache
            _memoryCache = new MemoryCache( 16000000 );
        }


        /// <summary>
        /// Used to get the collection of the class
        /// </summary>
        /// <returns>The collection of this pages</returns>
        public async Task<IonCollection> getCollectionAsync()
        {
            string collectionURL = PagesURLs.getCollectionURL( _config );
            CollectionCacheIndex cacheIndex = await CollectionCacheIndex.retrieve( collectionURL, _config ).ConfigureAwait( false );

            // Check if there is a not outdated cacheIndex avialible
            bool currentCacheEntry = cacheIndex != null && !cacheIndex.isOutdated( _config );
            bool networkConnected = NetworkUtils.isOnline();


            if( currentCacheEntry )
            {
                // retrieve current version from cache
                return await getCollectionFromCacheAsync( cacheIndex, false ).ConfigureAwait( false );
            }
            else
            {
                if( networkConnected )
                {
                    // download collection or check for modifications
                    return await getCollectionFromServerAsync( cacheIndex, false ).ConfigureAwait( false );
                }
                else
                {
                    if( cacheIndex != null )
                    {
                        // no network: use potential old version from cache
                        return await getCollectionFromCacheAsync( cacheIndex, false ).ConfigureAwait( false );
                    }
                    else
                    {
                        // Collection can neither be downloaded nor be found in cache
                        throw new CollectionNotAvailableException();
                    }
                }
            }
        }


        /// <summary>
        /// Used to get a page with a desired identifier
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns>Already parsed IonPage</returns>
        public async Task<IonPage> getPageAsync( string pageIdentifier )
        {
            string pageURL = PagesURLs.getPageURL( _config, pageIdentifier );
            PageCacheIndex pageCacheIndex = await PageCacheIndex.retrieve( pageURL, _config ).ConfigureAwait( false );
            bool isNetworkConnected = NetworkUtils.isOnline();

            if( pageCacheIndex == null )
            {
                if( isNetworkConnected )
                {
                    return await getPageFromServerAsync( pageIdentifier ).ConfigureAwait( false );
                }
                else
                {
                    throw new PageNotAvailableException();
                }

            }

            // Get collection
            IonCollection collection = await getCollectionAsync().ConfigureAwait( false );

            // Get last changed of the page from collection
            DateTime pageLastChanged = collection.getPageLastChanged( pageIdentifier );

            // Estimate, if the page is outdated or not
            bool isOutdated = pageCacheIndex.isOutdated( pageLastChanged );

            if( !isOutdated )
            {
                return await getPageFromCacheAsync( pageIdentifier ).ConfigureAwait( false );
            }
            else
            {
                if( isNetworkConnected )
                {
                    // Download page from server
                    return await getPageFromServerAsync( pageIdentifier ).ConfigureAwait( false );
                }
                else
                {
                    // get old version from cache
                    return await getPageFromCacheAsync( pageIdentifier ).ConfigureAwait( false );
                }
            }
        }


        /// <summary>
        /// Collects all the identifier of the pages included in this collection
        /// </summary>
        /// <returns>List if page identifier</returns>
        public async Task<List<string>> getAllPagesIdentifierAsync()
        {
            if( _memoryCache.collection == null )
            {
                // Get collection from server
                _memoryCache.collection = await getCollectionAsync().ConfigureAwait( false );
            }

            List<string> allPageIdentifier = new List<string>();

            for( int i = 0; i < _memoryCache.collection.pages.Count; i++ )
            {
                allPageIdentifier.Add( _memoryCache.collection.pages[i].identifier );
            }

            return allPageIdentifier;
        }


        /// <summary>
        /// Used to get a list of pages matching a given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of IonPage</returns>
        public async Task<List<IonPage>> getPagesAsync( Predicate<IonPagePreview> filter )
        {
            List<IonPagePreview> filteredPagePreviews = await getPagePreviewsAsync( filter ).ConfigureAwait( false );

            // Fetch all pages that fitted the filter
            List<IonPage> pageList = new List<IonPage>();

            for( int i = 0; i < filteredPagePreviews.Count; i++ )
            {
                pageList.Add( await getPageAsync( filteredPagePreviews[i].identifier ).ConfigureAwait( false ) );
            }

            return pageList;
        }


        /// <summary>
        /// Used to get a list of pagePreviews that match the given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of pagePreviews</returns>
        public async Task<List<IonPagePreview>> getPagePreviewsAsync( Predicate<IonPagePreview> filter )
        {
            // Filter all pagePreviews in collection with the given filter
            IonCollection collection = await getCollectionAsync().ConfigureAwait( false );
            List<IonPagePreview> filteredPagePreviews = collection.pages.FindAll( filter );

            return filteredPagePreviews;
        }


        /// <summary>
        /// Updates the IonConfig file
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( IonConfig config )
        {
            _dataClient = new DataClient( config ); // TODO: check this for intense use of GC
            _config = config;
        }


        /// <summary>
        /// Gets a page directly from the server
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns></returns>
        private async Task<IonPage> getPageFromServerAsync( string pageIdentifier )
        {
            try
            {
                // Retrieve the page from the server
                HttpResponseMessage response = await _dataClient.getPageAsync( pageIdentifier ).ConfigureAwait( false );
                IonPage page = await DataParser.parsePageAsync( response ).ConfigureAwait( false );

                // Add page to cache, if it is not null
                if( page != null )
                {
                    await savePageToCachesAsync( page, _config );
                }

                return page;
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error getting page " + pageIdentifier + " from server! " + e.Message );
                return null;
            }
        }


        /// <summary>
        /// Used to save a page to the memory and isolated storage cache
        /// </summary>
        /// <param name="page"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task savePageToCachesAsync( IonPage page, IonConfig config )
        {
            // Memory cache
            _memoryCache.savePage( page, _config );

            // Local storage cache
            await StorageUtils.savePageToIsolatedStorageAsync( page, config ).ConfigureAwait( false );

            // Save page cache index
            await PageCacheIndex.save( page, _config ).ConfigureAwait( false );
        }


        /// <summary>
        /// Gets a collection from the server
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        private async Task<IonCollection> getCollectionFromServerAsync( CollectionCacheIndex cacheIndex, bool cacheAsBackup )
        {
            //DateTime lastModified = cacheIndex != null ? cacheIndex.lastModified : DateTime.MinValue;

            try
            {
                // Retrive collecion from server and parse it
                HttpResponseMessage response = await _dataClient.getCollectionAsync( _config.collectionIdentifier, cacheIndex != null ? cacheIndex.lastModified : DateTime.MinValue ).ConfigureAwait( false );

                // Only parse the answer if it is not newer than the cached version
                if( !( response.StatusCode == System.Net.HttpStatusCode.NotModified ) )
                {
                    // Parse collection
                    IonCollection collection = await DataParser.parseCollectionAsync( response ).ConfigureAwait( false );

                    // Add collection to memory cache
                    _memoryCache.collection = collection;

                    // Save collection to isolated storage
                    await StorageUtils.saveCollectionToIsolatedStorageAsync( collection, _config ).ConfigureAwait( false );

                    // save cacheIndex
                    await saveCollectionCacheIndexAsync( collection.last_changed ).ConfigureAwait( false );

                    return collection;
                }
                else
                {
                    // Collection in the server is the same as stored already in isolated storage cache
                    if( _memoryCache.collection == null )
                    {
                        // Only load collection from isolated storage cache, if the memory cache has no collection cached
                        try
                        {
                            // Get collection from isolated storage
                            IonCollection collection = await StorageUtils.loadCollectionFromIsolatedStorageAsync( _config ).ConfigureAwait( false );

                            // Add collection to memory cache
                            if( collection != null )
                            {
                                _memoryCache.collection = collection;
                            }

                            // change the last-mofied date in the cacheIndex to now
                            await saveCollectionCacheIndexAsync( collection.last_changed ).ConfigureAwait( false );

                            return collection;
                        }
                        catch( Exception e )
                        {
                            Debug.WriteLine( "Error getting collection from isolated storage. Message: " + e.Message );
                            return null;
                        }
                    }
                    else
                    {
                        // change the last-mofied date in the cacheIndex to now
                        await saveCollectionCacheIndexAsync( _memoryCache.collection.last_changed ).ConfigureAwait( false );
                        return _memoryCache.collection;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error retreiving collection data: " + e.Message );
                return null;
            }
        }


        /// <summary>
        /// Gets a collection from the cache 
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        private async Task<IonCollection> getCollectionFromCacheAsync( CollectionCacheIndex cacheIndex, bool serverCallAsBackup )
        {
            string collectionURL = PagesURLs.getCollectionURL( _config );

            // retrieve from memory cache
            IonCollection collection = _memoryCache.collection;

            if( collection != null )
            {
                Debug.WriteLine( "Memory cache lookup" );
                return collection;
            }

            // try to load collection from isolated storage
            try
            {
                collection = await StorageUtils.loadCollectionFromIsolatedStorageAsync( _config ).ConfigureAwait( false );

                // Add collection to memory cache
                if( collection != null )
                {
                    _memoryCache.collection = collection;
                }
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error getting collection from isolated storage. Message: " + e.Message );
            }

            return collection;
        }


        /// <summary>
        /// Get a desired page from the cache
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns></returns>
        private async Task<IonPage> getPageFromCacheAsync( string pageIdentifier )
        {
            // Try to get page from memory cache
            IonPage page = _memoryCache.getPage( pageIdentifier, _config );

            if( page != null )
            {
                // Page in memory cache
                return page;
            }

            // Try to load page from local storage
            page = await StorageUtils.loadPageFromIsolatedStorageAsync( _config, pageIdentifier ).ConfigureAwait( false );

            // Add page to memory cache
            if( page != null )
            {
                _memoryCache.savePage( page, _config );
            }

            return page;
        }


        /// <summary>
        /// Saves the collection cache index
        /// </summary>
        /// <param name="lastModified"></param>
        private async Task saveCollectionCacheIndexAsync( DateTime lastModified )
        {
            await CollectionCacheIndex.save( _config, lastModified ).ConfigureAwait( false );
        }
    }
}
