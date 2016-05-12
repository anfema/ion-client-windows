using Anfema.Ion.DataModel;
using Anfema.Ion.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Anfema.Ion.Caching
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
        public static async Task<T> retrieve<T>( string requestUrl, IonConfig  config ) where T : CacheIndex
        {
            T index = MemoryCacheIndex.get<T>(requestUrl, config.collectionIdentifier);

            if( index != null )
            {
                IonLogging.log( "Index lookup " + requestUrl + " from memory", IonLogMessageTypes.SUCCESS );
                return index;
            }

            // check isolated storage
            try
            {
                index = await StorageUtils.getIndexAsync<T>(requestUrl, config ).ConfigureAwait(false);

                if( index != null )
                {
                    IonLogging.log( "Index lookup " + requestUrl + " from isolated storage", IonLogMessageTypes.SUCCESS );
                }
            }
            catch(Exception e)
            {
                IonLogging.log( "Index lookup " + requestUrl + " is not in isolated storage. Message: " + e.Message, IonLogMessageTypes.INFORMATION );
            }

            // Save to memory cache, if index is not null
            if( index != null )
            {
                MemoryCacheIndex.put(requestUrl, config.collectionIdentifier, index);
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
        public static async Task save<T>( string requestURL, T cacheIndex, IonConfig config ) where T : CacheIndex
        {
            try
            {
                // save to memory cache
                MemoryCacheIndex.put<T>(requestURL, config.collectionIdentifier, cacheIndex);

                // save to isolated storage
                await StorageUtils.saveIndexAsync(requestURL, cacheIndex, config ).ConfigureAwait(false);
            }
            catch( Exception e)
            {
                IonLogging.log( "Cache Index " + requestURL + " could not be saved. Message: " + e.Message, IonLogMessageTypes.ERROR );
            }
        }


        /// <summary>
        /// Clears the memory and file caches
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <param name="locale"></param>
        public static async Task clear( string collectionIdentifier )
        {
            try
            {
                // Clear memory cache
                MemoryCacheIndex.clear( collectionIdentifier );

                // Clear isolated storage cache
                await StorageUtils.deleteFolderInIsolatedStorageAsync( collectionIdentifier ).ConfigureAwait( false );
            }

            catch( Exception e )
            {
                IonLogging.log( "Error cleaning caches: " + e.Message, IonLogMessageTypes.ERROR );
            }
        }
    }
}
