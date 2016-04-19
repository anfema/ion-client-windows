using Anfema.Ion.Caching;
using Anfema.Ion.DataModel;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Ion.Utils
{
    public class StorageUtils
    {
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private static OperationLocks fileLocks = new OperationLocks();
        private static ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        /// <summary>
        /// Save primitive value to app storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue<T>( String key, T value )
        {
            _settings.Values[key] = value;
        }

        /// <summary>
        /// Retrieve primitive value from app storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>( String key )
        {
            if( _settings.Values.Keys.Contains( key ) )
            {
                return (T)_settings.Values[key];
            }

            return default( T );
        }

        /// <summary>
        /// Deletes a whole folder in the isolated storage of the device
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static async Task deleteFolderInIsolatedStorageAsync( string folderName )
        {
            using( await fileLocks.ObtainLock( folderName ).LockAsync().ConfigureAwait( false ) )
            {
                StorageFolder folder = await _localFolder.GetFolderAsync( folderName );
                await folder.DeleteAsync();
            }

            fileLocks.ReleaseLock( folderName );
        }


        /// <summary>
        /// Saves a collection to the isolated storage
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static async Task saveCollectionToIsolatedStorageAsync( IonCollection collection, IonConfig config )
        {
            // Generate filePath for collection file
            string filePath = FilePaths.getCollectionFolderPath( config ) + collection.identifier + IonConstants.JsonFileExtension;

            using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
            {
                // Create file or use existing file
                StorageFile file = await _localFolder.CreateFileAsync( filePath, CreationCollisionOption.ReplaceExisting );

                // Serialize collection
                string collectionSerialized = JsonConvert.SerializeObject( collection );

                // Write serialzed collection to file
                await FileIO.WriteTextAsync( file, collectionSerialized );
            }

            fileLocks.ReleaseLock( filePath );
        }


        /// <summary>
        /// Loads a collection from the isolated storage folder
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The collection or null, if the collection isn't found</returns>
        public static async Task<IonCollection> loadCollectionFromIsolatedStorageAsync( IonConfig config )
        {
            try
            {
                // Generate filePath for collection
                string filePath = FilePaths.getCollectionFolderPath( config ) + config.collectionIdentifier + IonConstants.JsonFileExtension;

                IonCollection collection = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Open file
                    StorageFile file = await _localFolder.GetFileAsync( filePath );

                    // Extract content
                    string content = await FileIO.ReadTextAsync( file );

                    // Deserialize collection
                    collection = JsonConvert.DeserializeObject<IonCollection>( content );
                }

                fileLocks.ReleaseLock( filePath );

                return collection;
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error loading collection " + config.collectionIdentifier + " from isolated storeage. Message: " + e.Message );
                return null;
            }
        }


        /// <summary>
        /// Saves a page to the isolated storage folder
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task savePageToIsolatedStorageAsync( IonPage page, IonConfig config )
        {
            try
            {
                // Generate filePath for page
                string filePath = FilePaths.getPagesFolderPath( config ) + page.identifier + IonConstants.JsonFileExtension;

                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create file or use existing file
                    StorageFile file = await _localFolder.CreateFileAsync( filePath, CreationCollisionOption.ReplaceExisting );

                    // Serialize collection
                    string pageSerialized = JsonConvert.SerializeObject( page );

                    // Write serialzed collection to file
                    await FileIO.WriteTextAsync( file, pageSerialized );
                }

                fileLocks.ReleaseLock( filePath );
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error saving page to isolated storage: " + e.Message );
            }
        }


        /// <summary>
        /// Loads a page from the isolated storage of the device
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="pageIdentifier"></param>
        /// <returns>The desired page or null, if the page wasn't found in the isolated folder</returns>
        public static async Task<IonPage> loadPageFromIsolatedStorageAsync( IonConfig config, string pageIdentifier )
        {
            try
            {
                // Generate filePath for page
                string filePath = FilePaths.getPagesFolderPath( config ) + pageIdentifier + IonConstants.JsonFileExtension;

                IonPage page = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Get file from file Path
                    StorageFile file = await _localFolder.GetFileAsync( filePath );

                    // Read content string
                    string content = await FileIO.ReadTextAsync( file );

                    // Deserialize page
                    page = JsonConvert.DeserializeObject<IonPage>( content );
                }

                fileLocks.ReleaseLock( filePath );

                return page;
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error loading page " + pageIdentifier + " of collection " + config.collectionIdentifier + " from isolated storeage. Message: " + e.Message );
                return null;
            }
        }


        /// <summary>
        /// Saves a index file to the isolated storage on the device
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestURL"></param>
        /// <param name="cacheIndex"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        public static async Task saveIndexAsync<T>( string requestURL, T cacheIndex, IonConfig config ) where T : CacheIndex
        {
            try
            {
                // Generate filePath for index
                string filePath = FilePaths.getCacheIndicesFolderPath( config ) + FilePaths.getFileName( requestURL ) + IonConstants.JsonFileExtension;

                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Open an existing file or create a new one
                    StorageFile file = await _localFolder.CreateFileAsync( filePath, CreationCollisionOption.ReplaceExisting );

                    // Serialize cache índex
                    string cacheIndexSerialized = JsonConvert.SerializeObject( cacheIndex );

                    // Write serialzed collection to file
                    await FileIO.WriteTextAsync( file, cacheIndexSerialized );
                }

                fileLocks.ReleaseLock( filePath );
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error saving cacheIndex to isolated storage. Message: " + e.Message );
            }
        }


        /// <summary>
        /// Gets a index from isolated storage of the device. Returns null, if the index isn't found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The index file or null, if the index file isn't found</returns>
        public static async Task<T> getIndex<T>( string requestURL, IonConfig config ) where T : CacheIndex
        {
            try
            {
                // Generate filePath for index
                string filePath = FilePaths.getCacheIndicesFolderPath( config ) + FilePaths.getFileName( requestURL ) + IonConstants.JsonFileExtension;

                T cacheIndex = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create file or use existing file
                    StorageFile file = await _localFolder.CreateFileAsync( filePath, CreationCollisionOption.OpenIfExists );

                    // Read content of the file
                    string content = await FileIO.ReadTextAsync( file );

                    // Deserialize cache index
                    cacheIndex = JsonConvert.DeserializeObject<T>( content );
                }

                fileLocks.ReleaseLock( filePath );

                return cacheIndex;

            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error loading cacheIndex " + requestURL + " from isolated storage: " + e.Message );
                return null;
            }
        }
    }
}
