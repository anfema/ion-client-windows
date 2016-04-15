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
        private static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
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
        public static async Task deleteFolderInIsolatedStorage( string folderName )
        {
            using( await fileLocks.ObtainLock( folderName ).LockAsync().ConfigureAwait( false ) )
            {
                StorageFolder folder = await _localFolder.GetFolderAsync( folderName );
                await folder.DeleteAsync();
            }
            fileLocks.ReleaseLock( folderName );
        }


        /// <summary>
        /// Saves a collection to the isolated storage of the device
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static async Task saveCollectionToIsolatedStorage( IonCollection collection )
        {
            String fileName = collection.identifier + ".json";
            String filePath = collection.identifier + IonConstants.Slash + fileName;

            StorageFolder folder = null;
            using( await fileLocks.ObtainLock( collection.identifier ).LockAsync().ConfigureAwait( false ) )
            {
                // Create folder or use existing folder
                folder = await _localFolder.CreateFolderAsync( collection.identifier, CreationCollisionOption.OpenIfExists );
            }
            fileLocks.ReleaseLock( collection.identifier );

            using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
            {
                // Create file or use existing file
                StorageFile file = await folder.CreateFileAsync( fileName, CreationCollisionOption.ReplaceExisting );

                // Serialize collection
                string collectionSerialized = JsonConvert.SerializeObject( collection );

                // Write serialzed collection to file
                await FileIO.WriteTextAsync( file, collectionSerialized );
            }
            fileLocks.ReleaseLock( filePath );
        }


        /// <summary>
        /// Loads a collection from the isolated storage folder of the device
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The collection or null, if the collection isn't found</returns>
        public static async Task<IonCollection> loadCollectionFromIsolatedStorage( string collectionIdentifier )
        {
            try
            {
                String fileName = collectionIdentifier + IonConstants.JsonFileExtension;
                String filePath = collectionIdentifier + IonConstants.Slash + fileName;

                StorageFolder folder = null;
                using( await fileLocks.ObtainLock( collectionIdentifier ).LockAsync().ConfigureAwait( false ) )
                {
                    folder = await _localFolder.GetFolderAsync( collectionIdentifier );
                }
                fileLocks.ReleaseLock( collectionIdentifier );

                IonCollection collection = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    StorageFile file = await folder.GetFileAsync( fileName );

                    string content = await FileIO.ReadTextAsync( file );

                    collection = JsonConvert.DeserializeObject<IonCollection>( content );
                }
                fileLocks.ReleaseLock( filePath );

                return collection;
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error loading collection " + collectionIdentifier + " from isolated storeage" );
                return null;
            }
        }


        /// <summary>
        /// Saves a page to the isolated storage folder of the device
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task savePageToIsolatedStorage( IonPage page )
        {
            try
            {
                // Check if the page to be saved was parsed correctly.
                // TODO: Remove this little fix, caused by an ION Bug
                if( page.identifier == null )
                {
                    return;
                }


                String localeFolderPath = page.collection + IonConstants.Slash + page.locale;
                String fileName = page.identifier + IonConstants.JsonFileExtension;
                String filePath = localeFolderPath + IonConstants.Slash + fileName;

                StorageFolder collectionFolder = null;
                using( await fileLocks.ObtainLock( page.collection ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create folder for collection or use existing folder
                    collectionFolder = await _localFolder.CreateFolderAsync( page.collection, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( page.collection );

                StorageFolder localeFolder = null;
                using( await fileLocks.ObtainLock( localeFolderPath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create folder for locale or use existing folder
                    localeFolder = await collectionFolder.CreateFolderAsync( page.locale, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( page.locale );

                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create file or use existing file
                    StorageFile file = await localeFolder.CreateFileAsync( fileName, CreationCollisionOption.ReplaceExisting );

                    // Serialize collection
                    string collectionSerialized = JsonConvert.SerializeObject( page );

                    // Write serialzed collection to file
                    await FileIO.WriteTextAsync( file, collectionSerialized );
                }
                fileLocks.ReleaseLock( filePath );
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error: " + e.Message );
            }
        }


        /// <summary>
        /// Loads a page from the isolated storage of the device
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="pageIdentifier"></param>
        /// <returns>The desired page or null, if the page wasn't found in the isolated folder</returns>
        public static async Task<IonPage> loadPageFromIsolatedStorage( string collectionIdentifier, string locale, string pageIdentifier )
        {
            try
            {
                String localeFolderPath = collectionIdentifier + IonConstants.Slash + locale;
                String fileName = pageIdentifier + IonConstants.JsonFileExtension;
                String filePath = localeFolderPath + IonConstants.Slash + fileName;

                StorageFolder collectionFolder = null;
                using( await fileLocks.ObtainLock( collectionIdentifier ).LockAsync().ConfigureAwait( false ) )
                {
                    collectionFolder = await _localFolder.GetFolderAsync( collectionIdentifier );
                }
                fileLocks.ReleaseLock( collectionIdentifier );

                StorageFolder localeFolder = null;
                using( await fileLocks.ObtainLock( localeFolderPath ).LockAsync().ConfigureAwait( false ) )
                {
                    localeFolder = await collectionFolder.GetFolderAsync( locale );
                }
                fileLocks.ReleaseLock( localeFolderPath );

                IonPage page = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    StorageFile file = await localeFolder.GetFileAsync( fileName );

                    string content = await FileIO.ReadTextAsync( file );

                    page = JsonConvert.DeserializeObject<IonPage>( content );
                }
                fileLocks.ReleaseLock( filePath );

                return page;
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error loading page " + collectionIdentifier + " from isolated storeage" );
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
        public static async Task saveIndex<T>( string requestURL, T cacheIndex, string collectionIdentifier ) where T : CacheIndex
        {
            try
            {
                String cacheFolderPath = collectionIdentifier + IonConstants.Slash + IonConstants.CacheIndicesFolderIdentifier;
                String fileName = FilePaths.getFileName( requestURL ) + IonConstants.JsonFileExtension;
                String filePath = cacheFolderPath + IonConstants.Slash + fileName;

                StorageFolder collectionFolder = null;
                using( await fileLocks.ObtainLock( collectionIdentifier ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create folder or use existing folder
                    collectionFolder = await _localFolder.CreateFolderAsync( collectionIdentifier, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( collectionIdentifier );

                StorageFolder cacheFolder = null;
                using( await fileLocks.ObtainLock( cacheFolderPath ).LockAsync().ConfigureAwait( false ) )
                {
                    cacheFolder = await collectionFolder.CreateFolderAsync( IonConstants.CacheIndicesFolderIdentifier, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( cacheFolderPath );

                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    StorageFile file = await cacheFolder.CreateFileAsync( fileName, CreationCollisionOption.ReplaceExisting );

                    // Serialize cache índex
                    string cacheIndexSerialized = JsonConvert.SerializeObject( cacheIndex );

                    // Write serialzed collection to file
                    await FileIO.WriteTextAsync( file, cacheIndexSerialized );
                }
                fileLocks.ReleaseLock( filePath );
            }

            catch( Exception e )
            {
                Debug.WriteLine( "Error saving cacheIndex to isolated storage" );
            }
        }


        /// <summary>
        /// Gets a index from isolated storage of the device. Returns null, if the index isn't found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The index file or null, if the index file isn't found</returns>
        public static async Task<T> getIndex<T>( string requestURL, string collectionIdentifier ) where T : CacheIndex
        {
            try
            {
                String cacheFolderPath = collectionIdentifier + IonConstants.Slash + IonConstants.CacheIndicesFolderIdentifier;
                String fileName = FilePaths.getFileName( requestURL ) + IonConstants.JsonFileExtension;
                String filePath = cacheFolderPath + IonConstants.Slash + fileName;

                StorageFolder collectionFolder = null;
                using( await fileLocks.ObtainLock( collectionIdentifier ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create folder or use existing folder
                    collectionFolder = await _localFolder.CreateFolderAsync( collectionIdentifier, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( collectionIdentifier );

                StorageFolder cacheFolder = null;
                using( await fileLocks.ObtainLock( cacheFolderPath ).LockAsync().ConfigureAwait( false ) )
                {
                    cacheFolder = await collectionFolder.CreateFolderAsync( IonConstants.CacheIndicesFolderIdentifier, CreationCollisionOption.OpenIfExists );
                }
                fileLocks.ReleaseLock( cacheFolderPath );

                T cacheIndex = null;
                using( await fileLocks.ObtainLock( filePath ).LockAsync().ConfigureAwait( false ) )
                {
                    // Create file or use existing file
                    StorageFile file = await cacheFolder.CreateFileAsync( fileName, CreationCollisionOption.OpenIfExists );

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
                Debug.WriteLine( "Error loading cacheIndex from isolated storage" );
                return null;
            }
        }
    }
}
