using Anfema.Amp.Caching;
using Anfema.Amp.DataModel;
using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Anfema.Amp.Utils
{
    public class StorageUtils
    {
        private static string CACHE_FOLDER_IDENTIFIER = "cache_indices";
        private static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;
        private static string CACHE_INDICES_FILENAME = "cacheIndices.json";
        
        private static Windows.Storage.ApplicationDataContainer _settings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static void SetValue<T>( String key, T value )
        {
            // Save value to app storage
            _settings.Values[key] = value;
        }

        public static T GetValue<T>( String key )
        {
            // Retrieve value from app storage
            if ( _settings.Values.Keys.Contains( key ) )
            {
                return ( T ) _settings.Values[key];
            }

            return default( T );
        }

        /// <summary>
        /// Deletes a whole folder in the isolated storage of the device
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static async Task<bool> deleteFolderInIsolatedStorage( string folderName )
        {
            StorageFolder folder = await _localFolder.GetFolderAsync(folderName);
            await folder.DeleteAsync();

            return true;
        }


        /// <summary>
        /// Saves a collection to the isolated storage of the device
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static async Task<bool> saveCollectionToIsolatedStorage( AmpCollection collection )
        {
            // Create folder or use existing folder
            StorageFolder folder = await _localFolder.CreateFolderAsync(collection.identifier, CreationCollisionOption.OpenIfExists);

            // Create file or use existing file
            StorageFile file = await folder.CreateFileAsync(collection.identifier + ".json", CreationCollisionOption.ReplaceExisting);

            // Serialize collection
            string collectionSerialized = JsonConvert.SerializeObject(collection);

            // Write serialzed collection to file
            await FileIO.WriteTextAsync(file, collectionSerialized);

            return true;
        }


        /// <summary>
        /// Loads a collection from the isolated storage folder of the device
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The collection or null, if the collection isn't found</returns>
        public static async Task<AmpCollection> loadCollectionFromIsolatedStorage( string collectionIdentifier )
        {
            try
            {
                StorageFolder folder = await _localFolder.GetFolderAsync(collectionIdentifier);
                StorageFile file = await folder.GetFileAsync(collectionIdentifier + ".json");

                string content = await FileIO.ReadTextAsync(file);

                AmpCollection collection = JsonConvert.DeserializeObject<AmpCollection>(content);

                return collection;
            }

            catch( Exception e )
            {
                Debug.WriteLine("Error loading collection " + collectionIdentifier + " from isolated storeage");
                return null;
            }
        }


        /// <summary>
        /// Saves a page to the isolated storage folder of the device
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task<bool> savePageToIsolatedStorage(AmpPage page)
        {
            // Create folder for collection or use existing folder
            StorageFolder collectionFolder = await _localFolder.CreateFolderAsync(page.collection, CreationCollisionOption.OpenIfExists);

            // Create folder for locale or use existing folder
            StorageFolder localeFolder = await collectionFolder.CreateFolderAsync(page.locale, CreationCollisionOption.OpenIfExists);

            // Create file or use existing file
            StorageFile file = await localeFolder.CreateFileAsync(page.identifier + ".json", CreationCollisionOption.ReplaceExisting);

            // Serialize collection
            string collectionSerialized = JsonConvert.SerializeObject(page);

            // Write serialzed collection to file
            await FileIO.WriteTextAsync(file, collectionSerialized);

            return true;
        }


        /// <summary>
        /// Loads a page from the isolated storage of the device
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="pageIdentifier"></param>
        /// <returns>The desired page or null, if the page wasn't found in the isolated folder</returns>
        public static async Task<AmpPage> loadPageFromIsolatedStorage( string collectionIdentifier, string locale, string pageIdentifier )
        {
            try
            {
                StorageFolder collectionFolder = await _localFolder.GetFolderAsync(collectionIdentifier);
                StorageFolder localeFolder = await collectionFolder.GetFolderAsync(locale);
                StorageFile file = await localeFolder.GetFileAsync(pageIdentifier + ".json");

                string content = await FileIO.ReadTextAsync(file);

                AmpPage page = JsonConvert.DeserializeObject<AmpPage>(content);

                return page;
            }

            catch (Exception e)
            {
                Debug.WriteLine("Error loading page " + collectionIdentifier + " from isolated storeage");
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
        public static async Task<bool> saveIndex<T>( string requestURL, T cacheIndex, string collectionIdentifier ) where T : CacheIndex
        {
            try
            {
                // Create folder or use existing folder
                StorageFolder collectionFolder = await _localFolder.CreateFolderAsync(collectionIdentifier, CreationCollisionOption.OpenIfExists);
                StorageFolder cacheFolder = await collectionFolder.CreateFolderAsync(CACHE_FOLDER_IDENTIFIER, CreationCollisionOption.OpenIfExists);

                // Create file or use existing file
                string filename = FilePaths.getFileName(requestURL) + ".json";
                StorageFile file = await cacheFolder.CreateFileAsync( filename, CreationCollisionOption.ReplaceExisting);

                // Serialize cache índex
                string cacheIndexSerialized = JsonConvert.SerializeObject(cacheIndex);

                // Write serialzed collection to file
                await FileIO.WriteTextAsync(file, cacheIndexSerialized);
            }

            catch( Exception e )
            {
                Debug.WriteLine("Error saving cacheIndex to isolated storage");
            }

            return true;
        }


        /// <summary>
        /// Gets a index from isolated storage of the device. Returns null, if the index isn't found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>The index file or null, if the index file isn't found</returns>
        public static async Task<T> getIndex<T>(string requestURL, string collectionIdentifier) where T : CacheIndex
        {
            try
            {
                // Create folder or use existing folder
                StorageFolder collectionFolder = await _localFolder.CreateFolderAsync(collectionIdentifier, CreationCollisionOption.OpenIfExists);
                StorageFolder cacheFolder = await collectionFolder.CreateFolderAsync(CACHE_FOLDER_IDENTIFIER, CreationCollisionOption.OpenIfExists);

                // Create file or use existing file
                StorageFile file = await cacheFolder.GetFileAsync(FilePaths.getFileName(requestURL) + ".json");

                // Read content of the file
                string content = await FileIO.ReadTextAsync(file);

                // Deserialize cache index
                T cacheIndex = JsonConvert.DeserializeObject<T>(content);

                return cacheIndex;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error saving cacheIndex to isolated storage");
                return null;
            }
        }
    }
}
