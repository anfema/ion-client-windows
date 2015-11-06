using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp
{
    public class Amp
    {
        private static Amp instance = null;

        // Caches for the data
        private List<AmpPage> _pagesCache = new List<AmpPage>();
        private List<AmpCollection> _collectionCache = new List<AmpCollection>();

        // Parser for data
        private DataParser _parser = new DataParser();
        private DataClient _client = new DataClient();

        private Task initTask;

        public Amp()
        {
        }

        public static Amp Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Amp();
                }

                return instance;
            }
        }

        public Dictionary<string, string> ApiCalls
        {
            get { return _client.ApiCalls; }
        }

        /// <summary>
        /// Starts the login process.
        /// </summary>
        /// <param name="username">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the login was successful.</returns>
        public async Task<bool> LoginAsync(string username, string password)
        {
            return await _client.LoginAsync(username, password);
        }


        // Inits the data loading process from the server
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
                _collectionCache = collectionsRoot.collection;

                // Load all pages. TODO: only testing purpose
                _pagesCache = getAllPagesOfCollection(_collectionCache[0].identifier).Result;
            });
        }


        // Gets a page with the desired name and all included translations
        public AmpPage getPageAllTranslations(string name, string translation, Action callback)
        {
            this.EnsureDataInitCompleted();

            AmpPage page = _pagesCache.Find(x => x.identifier.Equals(name));

            if (callback != null)
            {
                callback();
            }

            return page;
        }


        // Returns already as PageContent parsed data
        public AmpPageContent getPageContent(string name, string translation, Action callback)
        {
            this.EnsureDataInitCompleted();

            AmpPage page = _pagesCache.Find(x => x.identifier.Equals(name));
            AmpPageTranslation pageTranslation = page.translations.Find(x => x.locale.Equals(translation));
            AmpPageContent content = pageTranslation.content[0].children[0];

            if (callback != null)
            {
                callback();
            }

            return content;
        }


        // Returns a list of pagenames from the cached pages
        public void getPagesNames( string collectionIdentifier, out List<string> pageNames, Action callback)
        {
            this.EnsureDataInitCompleted();

            pageNames = new List<string>();

            AmpCollection desiredCollection = _collectionCache.Find(x => x.identifier.Equals(collectionIdentifier));

            if( desiredCollection != null )
            {
                for (int i = 0; i < desiredCollection.pages.Count; i++)
                {
                    pageNames.Add( desiredCollection.pages[i].identifier);
                }
            } 

            if (callback != null)
            {
                callback();
            }
        }


        // Creates a list of all pages and their included translationss
        public List<PageAllTranslationsModel> GetPageTranslations()
        {
            this.EnsureDataInitCompleted();

            var pageTranslations = new List<PageAllTranslationsModel>();

            for (int i = 0; i < _pagesCache.Count; i++)
            {
                PageAllTranslationsModel ptm = new PageAllTranslationsModel();
                ptm.name = _pagesCache[i].identifier;

                // Get all possible translations
                List<string> translations = new List<string>();
                for (int j = 0; j < _pagesCache[i].translations.Count; j++)
                {
                    translations.Add(_pagesCache[i].translations[j].locale);
                }

                ptm.translations = translations;

                pageTranslations.Add(ptm);
            }

            return pageTranslations;
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
            AmpCollection currentCollection = _collectionCache.Find(x => x.identifier.Equals(collectionName));

            if( currentCollection == null)
            {
                return new List<AmpPage>();
            }

            List<AmpPageRaw> pagesCacheRaw = new List<AmpPageRaw>();


            for ( int i=0; i<currentCollection.pages.Count; i++ )
            {
                string pageRawContent = await _client.getPageOfCollection(currentCollection.pages[i].identifier, currentCollection.identifier);
                AmpPageRootRaw ampPageRootRaw = JsonConvert.DeserializeObject<AmpPageRootRaw>(pageRawContent);

                AmpPageRaw ampPageRaw = ampPageRootRaw.page[0];

                pagesCacheRaw.Add(ampPageRaw);
            }

            // Parse the raw data to the working data types
            return _parser.parsePages(pagesCacheRaw);
        }
    }
}
