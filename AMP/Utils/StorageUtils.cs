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

        public static async Task<string> loadJsonFromIsolatedStorage( string requestURL )
        {
            StorageFile file = await _localFolder.GetFileAsync(requestURL);
            string content = await FileIO.ReadTextAsync(file);

            return content;
        }


        public static async Task<bool> saveJsonToIsolatedStorage( string requestURL, string jsonFile )
        {
            // TODO: Check if the path exists
            StorageFile file = await _localFolder.CreateFileAsync(requestURL + ".json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, jsonFile);

            return true;
        }


        public static async Task<bool> deleteFolderInIsolatedStorage( string folderName )
        {
            StorageFolder folder = await _localFolder.GetFolderAsync(folderName);
            await folder.DeleteAsync();

            return true;
        }



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

        /*
        /// <summary>
        /// Trys to load the indices dictionary for the given collection from isolated storage
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, string>> loadIndices( string collectionIdentifier )
        {
            try
            {
                StorageFolder folder = await _localFolder.GetFolderAsync(collectionIdentifier);
                StorageFile file = await folder.GetFileAsync(CACHE_INDICES_FILENAME);

                string content = await FileIO.ReadTextAsync(file);

                Dictionary<string, string> indices = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                return indices;
            }

            catch( Exception e )
            {
                Debug.WriteLine("Error loading indices from isolated storage");
                return null;
            }
        }
        

        /// <summary>
        /// Saves the indices dictionary to the folder of the collection
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static async Task<bool> saveIndices( string collectionIdentifier, Dictionary<string, string> indices )
        {
            // Create folder or use existing folder
            StorageFolder folder = await _localFolder.CreateFolderAsync(collectionIdentifier, CreationCollisionOption.OpenIfExists);

            // Create file or use existing file
            StorageFile file = await folder.CreateFileAsync(CACHE_INDICES_FILENAME, CreationCollisionOption.ReplaceExisting);

            // Serialize indices dictionary
            string collectionSerialized = JsonConvert.SerializeObject(indices);

            // Write serialzed indices dictionray
            await FileIO.WriteTextAsync(file, collectionSerialized);
            
            return true;
        }*/



        public static async Task<bool> saveIndex<T>( string requestURL, T cacheIndex, string collectionIdentifier ) where T : CacheIndex
        {
            try
            {
                // Create folder or use existing folder
                StorageFolder collectionFolder = await _localFolder.CreateFolderAsync(collectionIdentifier, CreationCollisionOption.OpenIfExists);
                StorageFolder cacheFolder = await collectionFolder.CreateFolderAsync(CACHE_FOLDER_IDENTIFIER, CreationCollisionOption.OpenIfExists);

                // Create file or use existing file
                StorageFile file = await cacheFolder.CreateFileAsync( FilePaths.getFileName(requestURL) + ".json", CreationCollisionOption.ReplaceExisting);

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
