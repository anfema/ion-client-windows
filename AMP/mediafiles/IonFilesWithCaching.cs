using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.MediaFiles
{
    public class IonFilesWithCaching : IIonFiles
    {
        // Config associated with this collection of pages
        private IonConfig _config;

        // Data client that will be used to get the data from the server
        private DataClient _dataClient;

        private OperationLocks downloadLocks = new OperationLocks();

        public IonFilesWithCaching( IonConfig config )
        {
            _config = config;

            // Init the data client
            _dataClient = new DataClient( config );
        }


        /// <summary>
        /// Request a element from a given url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checksum"></param>
        /// <param name="ignoreCaching"></param>
        /// <param name="inTargetFile"></param>
        /// <returns></returns>
        public async Task<StorageFile> request( String url, String checksum, IonContent content, Boolean ignoreCaching )
        {
            String targetPath = "other";
            string contentType = content.type;

            switch( contentType )
            {
                case IonConstants.ImageContentIdentifier:
                    {
                        targetPath = FilePaths.getMediaFilePath( url, _config );
                        break;
                    }

                case IonConstants.FileContentIdentifier:
                    {
                        targetPath = FilePaths.getMediaFilePath( url, _config );
                        break;
                    }

                case IonConstants.MediaContentIdentifier:
                    {
                        targetPath = FilePaths.getMediaFilePath( url, _config );
                        break;
                    }
            }

            return await request( url, targetPath, checksum, ignoreCaching );
        }


        /// <summary>
        /// Requests a archive file from a given url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checksum"></param>
        /// <param name="ignoreCaching"></param>
        /// <returns>Archive file</returns>
        public async Task<StorageFile> requestArchiveFile( string url )
        {
            string targetFile = FilePaths.getArchiveFilePath( url, _config );
            return await request( url, targetFile, "", false );
        }


        /// <summary>
        /// Requests a file from a specific url and saves it to the given filePath
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="checksum"></param>
        /// <param name="ignoreCaching"></param>
        /// <returns></returns>
        private async Task<StorageFile> request( String url, string filePath, String checksum, Boolean ignoreCaching )
        {
            StorageFile returnFile = null;
            using( await downloadLocks.ObtainLock( url ).LockAsync().ConfigureAwait( false ) )
            {
                // fetch file from local storage or download it?
                bool fileExists = await FileUtils.Exists( filePath ).ConfigureAwait( false );
                bool upToDate = await IsFileUpToDate( url, checksum ).ConfigureAwait( false );

                if( fileExists && upToDate )
                {
                    // retrieve current version from cache
                    returnFile = await FileUtils.ReadFromFile( filePath ).ConfigureAwait( false );
                }

                else if( NetworkUtils.isOnline() )
                {
                    // download media file
                    MemoryStream saveStream = await _dataClient.PerformRequest( new Uri( url ) ).ConfigureAwait( false );

                    // save data to file
                    returnFile = await FileUtils.WriteToFile( saveStream, filePath ).ConfigureAwait( false );
                    await FileCacheIndex.save( url, saveStream, _config, checksum ).ConfigureAwait( false );
                }
                else if( await FileUtils.Exists( filePath ).ConfigureAwait( false ) )
                {
                    // TODO notify app that data might be outdated
                    // no network: use old version from cache (even if no cache index entry exists)
                    returnFile = await FileUtils.ReadFromFile( filePath );
                }
            }

            downloadLocks.ReleaseLock( url );

            if( returnFile == null )
            {
                throw new Exception( "Media file " + url + " is not in cache and no internet connection is available." );
            }

            return returnFile;
        }


        /// <summary>
        /// Updates the IonConfig file
        /// </summary>
        /// <param name="config"></param>
        public void updateConfig( IonConfig config )
        {
            _config = config;
        }


        private async Task<bool> IsFileUpToDate( String url, String checksum )
        {
            FileCacheIndex fileCacheIndex = await FileCacheIndex.retrieve( url, _config ).ConfigureAwait( false );
            if( fileCacheIndex == null )
            {
                return false;
            }

            if( checksum != null )
            {
                // check with file's checksum
                return !fileCacheIndex.IsOutdated( checksum );
            }
            else
            {
                // check with collection's last_modified (previewPage.last_changed would be slightly more precise)
                CollectionCacheIndex collectionCacheIndex = await CollectionCacheIndex.retrieve( _config ).ConfigureAwait( false );
                DateTime collectionLastModified = collectionCacheIndex == null ? default( DateTime ) : collectionCacheIndex.lastModified;
                return collectionLastModified != null && fileCacheIndex.lastUpdated != null && !( collectionLastModified.CompareTo( fileCacheIndex.lastUpdated ) > 0 );
            }
        }
    }
}
