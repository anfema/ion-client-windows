using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Anfema.Amp
{
    public class Amp
    {
        // Dictionary for all the possible instances bound to a specific config
        private static Dictionary<AmpConfig, Amp> instances = new Dictionary<AmpConfig, Amp>();

        public static Amp getInstance( AmpConfig config )
        {
            Amp storedClient;

            if( instances.TryGetValue(config, out storedClient ) )
            {
                return storedClient;
            }


            Amp amp = new Amp( config );
            instances.Add(config, amp);
            return amp;
        }

        // Caches for the data
        private List<AmpPage> _pagesCache = new List<AmpPage>();
        private AmpCollection _collectionCache = new AmpCollection();

        // Parser for data
        private DataParser _parser = new DataParser();
        private DataClient _client = new DataClient();

        private Task initTask;


        public Amp( AmpConfig config )
        {
            _client.configureDataClient(config);
        }


        // Inits the data loading process from the server  TODO: only for testing purpose -> move to desired data class
        public async Task LoadDataAsync()
        {
            await new TaskFactory().StartNew(() =>
            {
                this.BeginLoadData();
                Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "LoadDataAsync() begin.");
                this.initTask.Wait();
                Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "LoadDataAsync() end.");
            });
        }


        private void BeginLoadData()
        {
            this.initTask = new TaskFactory().StartNew(() =>
            {
                // Init the collections
                string collectionsString = _client.getData("collections").Result;
                CollectionRoot collectionsRoot = JsonConvert.DeserializeObject<CollectionRoot>(collectionsString);
                _collectionCache = collectionsRoot.collection[0]; // TODO change this for the multiton

                // Load all pages. TODO: only testing purpose
                _pagesCache = getAllPagesOfCollection(_collectionCache.identifier).Result;
            });
        }


        // Returns all the cached collections
        public AmpCollection getCollection()
        {
            return _collectionCache;
        }
        

        // Returns the whole data of a desired page already parsed as observable collections
        public AmpPageObservableCollection getPageContent(string name, Action callback)
        {
            this.EnsureDataInitCompleted();

            AmpPage page = _pagesCache.Find(x => x.identifier.Equals(name) );
            AmpPageObservableCollection content = page.contents[0].children[0];

            if (callback != null)
            {
                callback();
            }

            return content;
        }


        // Returns a list of all page identifiers
        public List<string> getPageNames()
        {
            List<string> pageNames = new List<string>();

            for( int i=0; i< _pagesCache.Count; i++)
            {
                pageNames.Add(_pagesCache[i].identifier);
            }

            return pageNames;
        }


        private void EnsureDataInitCompleted()
        {
            if (this.initTask == null)
            {
                throw new Exception("You must call the BeginLoadData method before you call any other method of the Amp class.");
            }
            else if (this.initTask.Status != TaskStatus.RanToCompletion)
            {
                Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "Waiting for InitData-Task completion.");

                this.initTask.Wait();

                Debug.WriteLine("{0}.{1}: {2}", DateTime.Now.Second, DateTime.Now.Millisecond, "Waiting for InitData-Task completed.");
                if (this.initTask.Status != TaskStatus.RanToCompletion)
                {
                    throw this.initTask.Exception;
                }
            }
        }


        // Gets all pages for a given collection name
        private async Task<List<AmpPage>> getAllPagesOfCollection( string collectionName )
        {            
            // If no collection with the desired name was found return a blank AmpPage to avoid nullpointer exceptions
            if( _collectionCache == null)
            {
                return new List<AmpPage>();
            }

            List<PageRaw> pagesCacheRaw = new List<PageRaw>();


            for ( int i=0; i<_collectionCache.pages.Count; i++ )
            {
                string pageRawContent = await _client.getPageOfCollection(_collectionCache.pages[i].identifier, _collectionCache.identifier);
                PageRootRaw ampPageRootRaw = JsonConvert.DeserializeObject<PageRootRaw>(pageRawContent);

                PageRaw ampPageRaw = ampPageRootRaw.page[0];

                pagesCacheRaw.Add(ampPageRaw);
            }

            // Parse the raw data to the working data types
            return _parser.parsePages(pagesCacheRaw);
        }
    }
}
