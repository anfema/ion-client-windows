using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Anfema.Amp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Anfema.Amp.Pages
{
    public class AmpPagesWithCaching : IAmpPages
    {
        // Config associated with this collection of pages
        private AmpConfig _config;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;

        private static int COLLECTION_NOT_MODIFIED = 304;

        // Different caching methods
        private MemoryCache _memoryCache;


        /// <summary>
        /// Constructor with configuration file for initialization
        /// </summary>
        /// <param name="config"></param>
        public AmpPagesWithCaching( AmpConfig config )
        {
            _config = config;

            // Init the data client
            _dataClient = new DataClient(config);

            // Init memory cache
            _memoryCache = new MemoryCache(100);
        }


        /// <summary>
        /// Used to get the collection of the class
        /// </summary>
        /// <returns>The collection of this pages</returns>
        public async Task<AmpCollection> getCollectionAsync()
        {
            string collectionURL = PagesURLs.getCollectionURL(_config);
            CollectionCacheIndex cacheIndex = await CollectionCacheIndex.retrieve(collectionURL, _config.collectionIdentifier);

            bool currentCacheEntry = cacheIndex != null && !cacheIndex.isOutdated(_config);
            bool networkConnected = NetworkUtils.isOnline();

            
            if( currentCacheEntry)
            {
                // retrieve current version from cache
                return await getCollectionFromCache(cacheIndex, false);
            }
            else
            {
                if( networkConnected )
                {
                    // download collection
                    return await getCollectionFromServerAsync(cacheIndex, false);
                }
                else
                {
                    if( cacheIndex != null )
                    {
                        // no network: use potential old version from cache
                        return await getCollectionFromCache(cacheIndex, false);
                    }
                    else
                    {
                        // Collection can neither be downloaded nor be found in cache
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// Used to get a page with a desired identifier
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns>Already parsed AmpPage</returns>
        public async Task<AmpPage> getPageAsync(string pageIdentifier)
        {
            string pageURL = PagesURLs.getPageURL(_config, pageIdentifier);
            PageCacheIndex pageCacheIndex = await PageCacheIndex.retrieve(pageURL, _config.collectionIdentifier);
            bool isNetworkConnected = NetworkUtils.isOnline();

            if( pageCacheIndex == null )
            {
                if( isNetworkConnected )
                {
                    return await getPageFromServerAsync(pageIdentifier);
                }
                else
                {
                    return null;
                }

            }

            // Get collection
            AmpCollection collection = await getCollectionAsync();

            // Get last changed of the page from collection
            DateTime pageLastChanged = collection.getPageLastChanged( pageIdentifier );

            // Estimate, if the page is outdated or not
            bool isOutdated = pageCacheIndex.isOutdated( pageLastChanged );

            if( !isOutdated )
            {
                return await getPageFromCache(pageIdentifier);
            }
            else
            {
                if( isNetworkConnected )
                {
                    // Download page from server
                    return await getPageFromServerAsync(pageIdentifier);
                }
                else
                {
                    // get old version from cache
                    return await getPageFromCache(pageIdentifier);
                }
            }
        }


        /// <summary>
        /// Collects all the identifier of the pages included in this collection
        /// </summary>
        /// <returns>List if page identifier</returns>
        public async Task<List<string>> getAllPagesIdentifierAsync()
        {
            if( _memoryCache.collection == null)
            {
                // Get collection from server
                _memoryCache.collection = await getCollectionAsync();
            }

            List<string> allPageIdentifier = new List<string>();

            for(int i=0; i<_memoryCache.collection.pages.Count; i++)
            {
                allPageIdentifier.Add(_memoryCache.collection.pages[i].identifier);
            }

            return allPageIdentifier;
        }


        /// <summary>
        /// Gets a page directly from the server
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns></returns>
        private async Task<AmpPage> getPageFromServerAsync(string pageIdentifier)
        {
            try
            {
                // Retrieve the page from the server
                HttpResponseMessage response = await _dataClient.getPageAsync(pageIdentifier);
                AmpPage page = await DataParser.parsePage(response);

                // Add page to cache, if it is not null
                if (page != null)
                {
                    // Memory cache
                    _memoryCache.savePage(page, _config);

                    // Local storage cache
                    await StorageUtils.savePageToIsolatedStorage(page);

                    // Save page cache index
                    await PageCacheIndex.save(page, _config);
                }

                return page;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error getting page " + pageIdentifier + " from server! " + e.Message);
                return null;
            }
        }


        /// <summary>
        /// Gets a collection from the server
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        private async Task<AmpCollection> getCollectionFromServerAsync( CollectionCacheIndex cacheIndex, bool cacheAsBackup )
        {
            DateTime lastModified = cacheIndex != null ? cacheIndex.lastModified : DateTime.MinValue;
            
            try
            {
                // Retrive collecion from server and parse it
                HttpResponseMessage response = await _dataClient.getCollectionAsync( _config.collectionIdentifier, cacheIndex != null ? cacheIndex.lastModifiedDate : DateTime.MinValue);

                // Only parse the answer if it is not newer than the cached version
                if (! (response.StatusCode == System.Net.HttpStatusCode.NotModified ) )
                {
                    // Parse collection
                    AmpCollection collection = await DataParser.parseCollection(response);

                    // Add collection to memory cache
                    _memoryCache.collection = collection;

                    // Save collection to isolated storage
                    await StorageUtils.saveCollectionToIsolatedStorage(collection);

                    // save cacheIndex
                    await saveCollectionCacheIndex(collection.last_changed);

                    return collection;
                }
                else
                {
                    // Collection in the server is the same as stored already in isolated storage cache
                    try
                    {
                        // Get collection from isolated storage
                        AmpCollection collection = await StorageUtils.loadCollectionFromIsolatedStorage(_config.collectionIdentifier);

                        // Add collection to memory cache
                        if (collection != null)
                        {
                            _memoryCache.collection = collection;
                        }

                        return collection;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error getting collection from isolated storage.");
                        return null;
                    }
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine("Error retreiving collection data: " + e.Message);
                return null;
            }
        }


        /// <summary>
        /// Gets a collection from the cache 
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        private async Task<AmpCollection> getCollectionFromCache( CollectionCacheIndex cacheIndex, bool serverCallAsBackup )
        {
            string collectionURL = PagesURLs.getCollectionURL(_config);

            // retrieve from memory cache
            AmpCollection collection = _memoryCache.collection;

            if( collection != null )
            {
                Debug.WriteLine("Memory cache lookup");
                return collection;
            }

            // try to load collection from isolated storage
            try
            {
                collection = await StorageUtils.loadCollectionFromIsolatedStorage(_config.collectionIdentifier);

                // Add collection to memory cache
                if (collection != null)
                {
                    _memoryCache.collection = collection;
                }
            }
            catch( Exception e)
            {
                Debug.WriteLine("Error getting collection from isolated storage.");
            }

            return collection;
        }


        /// <summary>
        /// Get a desired page from the cache
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns></returns>
        private async Task<AmpPage> getPageFromCache( string pageIdentifier )
        {
            // Try to get page from memory cache
            AmpPage page = _memoryCache.getPage( pageIdentifier, _config );

            if (page != null)
            {
                // Page in memory cache
                return page;
            }

            // Try to load page from local storage
            page = await StorageUtils.loadPageFromIsolatedStorage(_config.collectionIdentifier, _config.locale, pageIdentifier);

            // Add page to memory cache
            if( page != null )
            {
                _memoryCache.savePage(page, _config);
            }

            return page;
        }


        /// <summary>
        /// Saves the collection cache index
        /// </summary>
        /// <param name="lastModified"></param>
        private async Task<bool> saveCollectionCacheIndex( DateTime lastModified )
        {
            await CollectionCacheIndex.save(_config, lastModified);

            return true;
        }
    }
}
