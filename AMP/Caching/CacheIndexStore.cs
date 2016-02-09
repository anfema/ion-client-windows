using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class CacheIndexStore
    {        
        public static async Task<T> retrieve<T>( string requestUrl, string collectionIdentifier ) where T : CacheIndex
        {

            T index = MemoryCacheIndex.get<T>(requestUrl, collectionIdentifier);

            if( index != null )
            {
                Debug.WriteLine("Index lookup " + requestUrl + " from memory");
                return index;
            }

            // check isolated storage
            Debug.WriteLine("Index lookup " + requestUrl + " from isolated storage");

            string json = await StorageUtils.loadJsonFromIsolatedStorage(requestUrl);
            index = JsonConvert.DeserializeObject<T>(json);

            // Save to memory cache, if index is not null
            if( index != null )
            {
                MemoryCacheIndex.put(requestUrl, collectionIdentifier, index);
            }


            return index;
        }


        public static async Task<bool> save<T>( string requestURL, T cacheIndex, AmpConfig config ) where T : CacheIndex
        {
            try
            {
                // save to memory cache
                MemoryCacheIndex.put<T>(requestURL, config.collectionIdentifier, cacheIndex);

                // save to isolated storage
                await StorageUtils.saveJsonToIsolatedStorage(requestURL, JsonConvert.SerializeObject(cacheIndex));
            }
            catch( Exception e)
            {
                Debug.WriteLine("Cache Index " + requestURL + " could not be saved");
            }

            return true;
        }


        public static async Task<bool> clear( string collectionIdentifier, string locale )
        {
            // Clear memory cache
            MemoryCacheIndex.clear(collectionIdentifier);

            // Clear isolated storage cache
            await StorageUtils.deleteFolderInIsolatedStorage(collectionIdentifier + "/" + locale);

            return true;
        }
    }
}
