using Anfema.Ion.DataModel;
using Anfema.Ion.Exceptions;
using Anfema.Ion.FullTextSearch;
using Anfema.Ion.MediaFiles;
using Anfema.Ion.Pages;
using Anfema.Ion.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion
{
    public class Ion : IIonConfigUpdateable
    {
        // Dictionary for all the possible instances bound to a specific config
        private static Dictionary<IonConfig, Ion> instances = new Dictionary<IonConfig, Ion>();

        /// Organizes all the data handling for pages and collections
        private IIonPages _ampPages;
        private IIonFiles _ampFiles;
        private IIonFts _ampFts;

        /// <summary>
        /// Is used to get a instance of Ion corresponding to the given configuration
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Ion getInstance( IonConfig config )
        {
            Ion storedClient;

            if( instances.TryGetValue( config, out storedClient ) )
            {
                // Eventually update the config file saved in the stored client with the given one that might contain new or updated parameters
                storedClient.updateConfig( config );
                return storedClient;
            }

            Ion amp = new Ion( config );
            instances.Add( config, amp );
            return amp;
        }


        /// <summary>
        /// Constructor with a config file for initialization
        /// </summary>
        /// <param name="config"></param>
        private Ion( IonConfig config )
        {
            _ampPages = new IonPagesWithCaching( config );
            _ampFiles = new IonFilesWithCaching( config );
            _ampFts = new IonFtsImpl( _ampPages, _ampFiles, config );
        }


        /// <summary>
        /// Returns a whole IonPage with the desired name and calls the given callback method after retrieving the page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns>IonPage with the desired name</returns>
        public async Task<IonPage> getPageAsync( string name, Action callback )
        {
            IonPage page = await _ampPages.getPageAsync( name ).ConfigureAwait( false );

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
        public async Task<List<IonPage>> getPagesAsync( Predicate<IonPagePreview> filter, Action callback = null )
        {
            List<IonPage> pagesList = await _ampPages.getPagesAsync( filter ).ConfigureAwait( false );

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
            return await _ampPages.getAllPagesIdentifierAsync().ConfigureAwait( false );
        }


        /// <summary>
        /// Used to get a list of PagePreviews matching the given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of IonPagePreview elements</returns>
        public async Task<List<IonPagePreview>> getPagePreviewsAsync( Predicate<IonPagePreview> filter, Action callback = null )
        {
            List<IonPagePreview> pagePreviewList = await _ampPages.getPagePreviewsAsync( filter ).ConfigureAwait( false );

            if( callback != null )
            {
                callback();
            }

            return pagePreviewList;
        }


        /// <summary>
        /// Used to search for a specific IonPagePreview with the exact identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="callback"></param>
        /// <returns>IonPagePreview</returns>
        public async Task<IonPagePreview> getPagePreviewAsync( string identifier, Action callback = null )
        {
            List<IonPagePreview> searchResult = await _ampPages.getPagePreviewsAsync( PageFilter.identifierEquals( identifier ) ).ConfigureAwait( false );

            // If no IonPagePreview was found throw a not found exception
            if( searchResult.Count == 0 )
            {
                throw new PagePreviewNotFoundException( identifier );
            }

            // Get first result
            IonPagePreview pagePreview = searchResult[0];

            if( callback != null )
            {
                callback();
            }

            return pagePreview;
        }


        public async Task<String> DownloadSearchDatabase()
        {
            return await _ampFts.DownloadSearchDatabase().ConfigureAwait( false );
        }


        public async Task<List<SearchResult>> FullTextSearch( String searchTerm, String locale, String pageLayout = null )
        {
            return await _ampFts.FullTextSearch( searchTerm, locale, pageLayout ).ConfigureAwait( false );
        }


        /// <summary>
        /// Requests a element from the server/cache with a specific url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checksum"></param>
        /// <param name="content"></param>
        /// <param name="ignoreCaching"></param>
        /// <returns>Storage file including the desired element</returns>
        public async Task<StorageFile> Request( String url, String checksum, IonContent content, Boolean ignoreCaching = false )
        {
            return await _ampFiles.request( url, checksum, content, ignoreCaching ).ConfigureAwait( false );
        }

        public async Task LoadContentFiles( IonPageObservableCollection content )
        {
            foreach( IonImageContent ampImageContent in content.imageContent )
            {
                await ampImageContent.loadImage( this ).ConfigureAwait( false );
            }

            foreach( IonFileContent ampFileContent in content.fileContent )
            {
                await ampFileContent.loadFile( this ).ConfigureAwait( false );
            }
        }


        /// <summary>
        /// Loads a archive file from a given url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Storage file including the archive file</returns>
        public async Task<StorageFile> loadArchive( string url )
        {
            return await _ampFiles.requestArchiveFile( url );
        }


        /// <summary>
        /// Called to update the config file of all relevant elements
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( IonConfig config )
        {
            _ampPages.updateConfig( config );
            _ampFiles.updateConfig( config );
            _ampFts.updateConfig( config );
        }
    }
}
