using Anfema.Ion.DataModel;
using Anfema.Ion.Pages;
using Anfema.Ion.Utils;
using System;
using System.Threading.Tasks;

namespace Anfema.Ion.Caching
{
    public class CollectionCacheIndex : CacheIndex
    {
        public DateTime lastUpdated { get; set; }  // Used to store the last time, the collection was checked for modification
        public DateTime lastModified { get; set; } // Used to store the last modification time of the collection of the server


        public CollectionCacheIndex( string filename, DateTime lastUpdated, DateTime lastModified ) : base( filename )
        {
            this.lastUpdated = lastUpdated;
            this.lastModified = lastModified;
        }


        /// <summary>
        /// Checks if this object is older than the given DateTime
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool isOutdated( IonConfig config )
        {
            return lastUpdated < ( DateTime.Now.ToUniversalTime().AddMinutes( -config.minutesUntilCollectionRefresh ) );
        }


        /// <summary>
        /// Retrieves a collectionCacheIndex from cache
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>collectionCacheIndex or null, if the index isn't found</returns>
        public static async Task<CollectionCacheIndex> retrieve( string requestURL, IonConfig config )
        {
            return await CacheIndexStore.retrieve<CollectionCacheIndex>( requestURL, config ).ConfigureAwait( false );
        }


        /// <summary>
        /// Retrieves a collection cache index either from memory or file cache
        /// </summary>
        /// <param name="config"></param>
        /// <returns>Cache index or null, if no cache index was found</returns>
        public static async Task<CollectionCacheIndex> retrieve( IonConfig config )
        {
            String requestUrl = PagesURLs.getCollectionURL( config );
            return await CacheIndexStore.retrieve<CollectionCacheIndex>( requestUrl, config ).ConfigureAwait( false );
        }


        /// <summary>
        /// Saves a collectionCacheIndex to cache
        /// </summary>
        /// <param name="config"></param>
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public static async Task<bool> save( IonConfig config, DateTime lastModified )
        {
            string collectionURL = PagesURLs.getCollectionURL( config );
            CollectionCacheIndex cacheIndex = new CollectionCacheIndex( collectionURL, DateTimeUtils.now().ToUniversalTime(), lastModified );
            await CacheIndexStore.save( collectionURL, cacheIndex, config ).ConfigureAwait( false );

            return true;
        }
    }
}
