using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
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

            try
            {
                // Try fetching the collection from the server
                _collectionCache = await _dataClient.getCollectionAsync(_config.collectionIdentifier);
                return _collectionCache;
            }

            catch( Exception e)
            {
                Debug.WriteLine("Error retreiving collection data: " + e.Message);
                return new AmpCollection(); // TODO remove the empty class here
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

            try
            {
                // Retrieve the page from the server
                page = await _dataClient.getPageAsync(pageIdentifier);

                // Add page to cache
                _pagesCache.Add(page);

                return page;
            }
            catch( Exception e)
            {
                Debug.WriteLine("Error getting page " + pageIdentifier + " from server! " + e.Message);
                throw;
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
                try
                {
                    // Get collection from server
                    _collectionCache = await getCollectionAsync();
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Error getting collection from server! " + e.Message);
                    throw;
                }
            }

            List<string> allPageIdentifier = new List<string>();

            for(int i=0; i<_collectionCache.pages.Count; i++)
            {
                allPageIdentifier.Add(_collectionCache.pages[i].identifier);
            }

            return allPageIdentifier;
        }
    }
}
