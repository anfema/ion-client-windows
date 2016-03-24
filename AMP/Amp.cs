using Anfema.Amp.DataModel;
using Anfema.Amp.Exceptions;
using Anfema.Amp.FullTextSearch;
using Anfema.Amp.MediaFiles;
using Anfema.Amp.Pages;
using Anfema.Amp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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
        /// Returns a list of AmpPages that match the given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of AmpPagees</returns>
        public async Task<List<AmpPage>> getPagesAsync( Predicate<PagePreview> filter, Action callback = null )
        {
            List<AmpPage> pagesList = await _ampPages.getPagesAsync(filter);

            if( callback != null )
            {
                callback();
            }

            return pagesList;
        }


        /// <summary>
        /// Returns a list of all page identifiers
        /// </summary>
        /// <returns>List of identifier</returns>
        public async Task<List<string>> getAllPageIdentifierAsync()
        {
            return await _ampPages.getAllPagesIdentifierAsync();
        }


        /// <summary>
        /// Used to get a list of PagePreviews matching the given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of PagePreview elements</returns>
        public async Task<List<PagePreview>> getPagePreviewsAsync( Predicate<PagePreview> filter, Action callback = null )
        {
            List<PagePreview> pagePreviewList = await _ampPages.getPagePreviewsAsync(filter);

            if( callback != null )
            {
                callback();
            }

            return pagePreviewList;
        }


        /// <summary>
        /// Used to search for a specific PagePreview with the exact identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="callback"></param>
        /// <returns>PagePreview</returns>
        public async Task<PagePreview> getPagePreviewAsync( string identifier, Action callback = null )
        {
            List<PagePreview> searchResult = await _ampPages.getPagePreviewsAsync(PageFilter.identifierEquals(identifier));

            // If no PagePreview was found throw a not found exception
            if( searchResult.Count == 0 )
            {
                throw new PagePreviewNotFoundException(identifier);
            }

            // Get first result
            PagePreview pagePreview = searchResult[0];

            if( callback != null )
            {
                callback();
            }

            return pagePreview;
        }


        public async Task<String> DownloadSearchDatabase()
        {
            return await _ampFts.DownloadSearchDatabase();
        }


        public async Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout = null )
        {
            return await _ampFts.FullTextSearch( searchTerm, locale, pageLayout);
        }


        public async Task<StorageFile> Request( String url, String checksum, Boolean ignoreCaching = false )
        {
            return await _ampFiles.Request( url, checksum, ignoreCaching );
        }

        public async Task LoadContentFiles( AmpPageObservableCollection content )
        {
            foreach ( AmpImageContent ampImageContent in content.imageContent )
            {
                await ampImageContent.loadImage( this );
            }

            foreach ( AmpFileContent ampFileContent in content.fileContent )
            {
                await ampFileContent.loadFile( this );
            }
        }
    }
}
