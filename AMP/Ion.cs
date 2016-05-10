using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using Anfema.Ion.Exceptions;
using Anfema.Ion.FullTextSearch;
using Anfema.Ion.MediaFiles;
using Anfema.Ion.Pages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Anfema.Ion.Archive;


namespace Anfema.Ion
{
    public class Ion : IIonConfigUpdateable
    {
        // Dictionary for all the possible instances bound to a specific config
        private static Dictionary<IonConfig, Ion> instances = new Dictionary<IonConfig, Ion>();

        /// Organizes all the data handling for pages and collections
        private IIonPages _ionPages;
        private IIonFiles _ionFiles;
        private IIonFts _ionFts;
        private IIonArchive _ionArchive;

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
            _ionPages = new IonPagesWithCaching( config );
            _ionFiles = new IonFilesWithCaching( config );
            _ionFts = new IonFtsImpl( _ionPages, _ionFiles, config );
            _ionArchive = new IonArchiveOperations( config );
        }


        /// <summary>
        /// Returns a whole IonPage with the desired name and calls the given callback method after retrieving the page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns>IonPage with the desired name</returns>
        public async Task<IonPage> getPageAsync( string name, Action callback = null )
        {
            IonPage page = await _ionPages.getPageAsync( name ).ConfigureAwait( false );

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
        public async Task<List<IonPage>> getPagesAsync( Predicate<IonPagePreview> filter = null, Action callback = null )
        {
            // In case there is no filter predicate given then set "all" as default
            if( filter == null )
            {
                filter = PageFilter.all;
            }

            List<IonPage> pagesList = await _ionPages.getPagesAsync( filter ).ConfigureAwait( false );

            if( callback != null )
            {
                callback();
            }

            return pagesList;
        }


        /// <summary>
        /// Used to get a list of PagePreviews matching the given filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of IonPagePreview elements</returns>
        public async Task<List<IonPagePreview>> getPagePreviewsAsync( Predicate<IonPagePreview> filter = null, Action callback = null )
        {
            // If the filter is set to null, then set it manualy to "all"
            if( filter == null )
            {
                filter = PageFilter.all;
            }

            List<IonPagePreview> pagePreviewList = await _ionPages.getPagePreviewsAsync( filter ).ConfigureAwait( false );

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
            List<IonPagePreview> searchResult = await _ionPages.getPagePreviewsAsync( PageFilter.identifierEquals( identifier ) ).ConfigureAwait( false );

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


        public async Task<string> DownloadSearchDatabaseAsync()
        {
            return await _ionFts.DownloadSearchDatabaseAsync().ConfigureAwait( false );
        }


        public async Task<List<SearchResult>> FullTextSearchAsync( string searchTerm, string locale, string pageLayout = null )
        {
            return await _ionFts.FullTextSearchAsync( searchTerm, locale, pageLayout ).ConfigureAwait( false );
        }


        /// <summary>
        /// Requests a element from the server/cache with a specific url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checksum"></param>
        /// <param name="content"></param>
        /// <param name="ignoreCaching"></param>
        /// <returns>Storage file including the desired element</returns>
        public async Task<StorageFile> requestAsync( String url, String checksum, IonContent content, Boolean ignoreCaching = false )
        {
            return await _ionFiles.requestAsync( url, checksum, content, ignoreCaching ).ConfigureAwait( false );
        }


        public async Task loadContentFilesAsync( IonPageObservableCollection content )
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
        public async Task loadArchiveAsync( string url, Action callback = null )
        {
            await _ionArchive.loadArchiveAsync( _ionFiles, _ionPages, url, callback );
        }


        /// <summary>
        /// Called to update the config file of all relevant elements
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( IonConfig config )
        {
            _ionPages.updateConfig( config );
            _ionFiles.updateConfig( config );
            _ionFts.updateConfig( config );
            _ionArchive.updateConfig( config );
        }
    }
}
