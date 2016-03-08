using Anfema.Amp.DataModel;
using Anfema.Amp.FullTextSearch;
using Anfema.Amp.MediaFiles;
using Anfema.Amp.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Anfema.Amp
{
    public class Amp
    {
        // Dictionary for all the possible instances bound to a specific config
        private static Dictionary<AmpConfig, Amp> instances = new Dictionary<AmpConfig, Amp>();
        
        /// Organizes all the data handling for pages and collections
        private IAmpPages _ampPages;
        private IAmpFiles _ampFiles;
        private IAmpFts _ampFts;


        /// <summary>
        /// Is used to get a instance of Amp corresponding to the given configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Constructor with a config file for initialization
        /// </summary>
        /// <param name="config"></param>
        public Amp( AmpConfig config )
        {
            _ampPages = new AmpPagesWithCaching(config);
            _ampFiles = new AmpFilesWithCaching( config );
            _ampFts = new AmpFtsImpl( _ampPages, _ampFiles, config );
        }


        /// <summary>
        /// Returns a whole AmpPage with the desired name and calls the given callback method after retrieving the page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns>AmpPage with the desired name</returns>
        public async Task<AmpPage> getPageAsync( string name, Action callback )
        {
            AmpPage page = await _ampPages.getPageAsync(name);

            if( callback != null )
            {
                callback();
            }

            return page;
        }


        /// <summary>
        /// Returns a list of all page identifiers
        /// </summary>
        /// <returns>List of identifier</returns>
        public async Task<List<string>> getAllPageIdentifierAsync()
        {
            return await _ampPages.getAllPagesIdentifierAsync();
        }

        public IAmpFiles AmpFiles()
        {
            return _ampFiles;
        }

        public Task<String> DownloadSearchDatabase()
        {
            return _ampFts.DownloadSearchDatabase();
        }

        public Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout )
        {
            return _ampFts.FullTextSearch( searchTerm, locale, pageLayout );
        }
    }
}
