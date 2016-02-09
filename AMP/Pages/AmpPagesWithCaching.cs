using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Anfema.Amp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Anfema.Amp.Pages
{
    public class AmpPagesWithCaching : IAmpPages
    {
        // Config associated with this collection of pages
        private AmpConfig _config;

        // Data saved in memory, will be replaced by real caching in the future TODO: implement caching
        private List<AmpPage> _pagesCache;
        private AmpCollection _collectionCache;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;


        private static int COLLECTION_NOT_MODIFIED = 304;


        /// <summary>
        /// Constructor with configuration file for initialization
        /// </summary>
        /// <param name="config"></param>
        public AmpPagesWithCaching( AmpConfig config )
        {
            _config = config;
            _dataClient = new DataClient(config);

            // Init pages cache
            _pagesCache = new List<AmpPage>();
        }


        /// <summary>
        /// Used to get the collection of the class
        /// </summary>
        /// <returns>The collection of this pages</returns>
        public async Task<AmpCollection> getCollectionAsync()
        {
            string collectionURL = PagesURLs.getCollectionURL(_config);
            //CollectionCacheIndex cacheIndex = CollectionCacheIndex.

            // Try to get the collection from cache
            AmpCollection collection = await getCollectionFromCache(_config.collectionIdentifier);

            // TODO: check outdated collections
            if (collection != null)
            {
                return collection;
            }

            if (NetworkUtils.isOnline())
            {
                // Try fetching the collection from the server
                collection = await getCollectionFromServerAsync(_config.collectionIdentifier);

                return collection;
            }
            else
            {
                Debug.WriteLine("Error getting collection " + _config.collectionIdentifier + " from server or cache.");
                return null;
            }
        }


        /// <summary>
        /// Used to get a page with a desired identifier
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns>Already parsed AmpPage</returns>
        public async Task<AmpPage> getPageAsync(string pageIdentifier)
        {
            // Try to get page from cache
            AmpPage page = await getPageFromCache(pageIdentifier);

            if( page != null )
            {
                // Page in cache
                return page;
            }

            // Page is not in cache and device is online
            if (NetworkUtils.isOnline())
            {
                // Retrieve the page from the server
                page = await getPageFromServerAsync( pageIdentifier );

                return page;
            }
            else
            {
                // Device is not online and page not in cache
                Debug.WriteLine("Error getting page " + pageIdentifier + " from cache or server");
                return null;
            }       
        }


        /// <summary>
        /// Collects all the identifier of the pages included in this collection
        /// </summary>
        /// <returns>List if page identifier</returns>
        public async Task<List<string>> getAllPagesIdentifierAsync()
        { 
            if(_collectionCache == null)
            {
                // Get collection from server
                _collectionCache = await getCollectionAsync();
            }

            List<string> allPageIdentifier = new List<string>();

            for(int i=0; i<_collectionCache.pages.Count; i++)
            {
                allPageIdentifier.Add(_collectionCache.pages[i].identifier);
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
                AmpPage page = await _dataClient.getPageAsync(pageIdentifier);

                // Add page to cache, if it is not null
                if (page != null)
                {
                    // Memory cache
                    _pagesCache.Add(page);

                    // Local storage cache
                    await StorageUtils.savePageToIsolatedStorage(page);
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
        private async Task<AmpCollection> getCollectionFromServerAsync( string collectionIdentifier )
        {
            try
            {
                // Retrive collecion from server
                AmpCollection collection = await _dataClient.getCollectionAsync( collectionIdentifier);

                // Add collection to cache
                _collectionCache = collection;

                // Save collection to isolated storage
                await StorageUtils.saveCollectionToIsolatedStorage(collection);

                return collection;
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
        private async Task<AmpCollection> getCollectionFromCache( string collectionIdentifier )
        {
            if( _collectionCache != null )
            {
                // Memory cache
                return _collectionCache;
            }
            else
            {
                // Local cache
                AmpCollection collection = await StorageUtils.loadCollectionFromIsolatedStorage(_config.collectionIdentifier);

                // Add collection to memory cache
                if( collection != null )
                {
                    _collectionCache = collection;
                }


                return collection;
            }
        }


        /// <summary>
        /// Get a desired page from the cache
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <returns></returns>
        private async Task<AmpPage> getPageFromCache( string pageIdentifier )
        {
            // Try to get page from memory cache
            AmpPage page = _pagesCache.Find(x => x.identifier.Equals(pageIdentifier));

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
                _pagesCache.Add(page);
            }

            return page;
        }
    }
}
