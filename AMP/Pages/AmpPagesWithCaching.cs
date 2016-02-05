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


        /// <summary>
        /// Constructor with configuration file for initialization
        /// </summary>
        /// <param name="config"></param>
        public AmpPagesWithCaching( AmpConfig config )
        {
            this._config = config;
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
            if( _collectionCache != null )
            {
                // Collection in cache
                return _collectionCache;
            }

            if (NetworkUtils.isOnline())
            {
                // Try fetching the collection from the server
                _collectionCache = await getCollectionFromServerAsync(_config.collectionIdentifier);
                return _collectionCache;
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
            AmpPage page = _pagesCache.Find( x => x.identifier.Equals( pageIdentifier) );

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

                // Add page to cache, if it is not null
                if (page != null)
                {
                    _pagesCache.Add(page);
                }

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
                AmpCollection collection = await _dataClient.getCollectionAsync( collectionIdentifier);
                return collection;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error retreiving collection data: " + e.Message);
                return null;
            }
        }
    }
}
