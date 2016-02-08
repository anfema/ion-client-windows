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
        private static StorageFolder _localFolder = ApplicationData.Current.LocalFolder;


        public static async Task<bool> saveCollectionToIsolatedStorage( AmpCollection collection )
        {
            // Create folder or use existing folder
            StorageFolder folder = await _localFolder.CreateFolderAsync(collection.identifier, CreationCollisionOption.OpenIfExists);

            // Create file or use existing file
            StorageFile file = await folder.CreateFileAsync(collection.identifier + ".json", CreationCollisionOption.OpenIfExists);

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
            StorageFile file = await localeFolder.CreateFileAsync(page.identifier + ".json", CreationCollisionOption.OpenIfExists);

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
    }
}
