using Anfema.Amp.DataModel;
using Anfema.Amp.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Anfema.Amp.Caching
{
    public class CacheIndexStore
    {
        /// <summary>
        /// Used to retrieve a class that inherits from CacheIndex
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>First it tries to get a index from memoryCache, then from fileCache and after that returns null, if no index is found</returns>
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

            try
            {
                index = await StorageUtils.getIndex<T>(requestUrl, collectionIdentifier).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Index lookup " + requestUrl + " is not in isolated storage");
            }

            // Save to memory cache, if index is not null
            if( index != null )
            {
                MemoryCacheIndex.put(requestUrl, collectionIdentifier, index);
            }

            return index;
        }


        /// <summary>
        /// Saves a index to memory and isolated storage cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestURL"></param>
        /// <param name="cacheIndex"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<bool> save<T>( string requestURL, T cacheIndex, AmpConfig config ) where T : CacheIndex
        {
            try
            {
                // save to memory cache
                MemoryCacheIndex.put<T>(requestURL, config.collectionIdentifier, cacheIndex);

                // save to isolated storage
                await StorageUtils.saveIndex(requestURL, cacheIndex, config.collectionIdentifier).ConfigureAwait(false);
            }
            catch( Exception e)
            {
                Debug.WriteLine("Cache Index " + requestURL + " could not be saved");
            }

            return true;
        }


        /// <summary>
        /// Clears the memory and file cache for all indices
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static async Task<bool> clear( string collectionIdentifier, string locale )
        {
            // Clear memory cache
            MemoryCacheIndex.clear(collectionIdentifier);

            // Clear isolated storage cache
            if (locale == null)
            {
                await StorageUtils.deleteFolderInIsolatedStorage(collectionIdentifier).ConfigureAwait(false);
            }
            else
            {
                await StorageUtils.deleteFolderInIsolatedStorage(collectionIdentifier + "/" + locale).ConfigureAwait(false);
            }

            return true;
        }
    }
}
