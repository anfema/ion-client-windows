using Anfema.Amp.DataModel;
using Anfema.Amp.Pages;
using Anfema.Amp.Utils;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class CollectionCacheIndex : CacheIndex
    {
        public DateTime lastUpdated { get; set; }
        public DateTime lastModified { get; set; }


        public CollectionCacheIndex( string filename, DateTime lastUpdated, DateTime lastModified ) : base(filename)
        {
            this.lastUpdated = lastUpdated;
            this.lastModified = lastModified;
        }


        /// <summary>
        /// Checks if this object is older than the given DateTime
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool isOutdated( AmpConfig config )
        {
            return lastUpdated < (DateTime.Now.AddMinutes(-config.minutesUntilCollectionRefresh));
        }


        [JsonIgnore]
        public DateTime lastModifiedDate
        {
            get
            {
                if(lastModified == null)
                {
                    Debug.WriteLine("Last modified: string is null");
                    return DateTime.MinValue;
                }

                try
                {
                    return lastModified;
                }
                catch( Exception e)
                {
                    Debug.WriteLine("Last modified: Parse error for " + lastModified);
                    return DateTime.MinValue;
                }
            }
        }


        /// <summary>
        /// Retrieves a collectionCacheIndex from cache
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns>collectionCacheIndex or null, if the index isn't found</returns>
        public static async Task<CollectionCacheIndex> retrieve( string requestURL, string collectionIdentifier )
        {
            return await CacheIndexStore.retrieve<CollectionCacheIndex>(requestURL, collectionIdentifier);
        }


        /// <summary>
        /// Saves a collectionCacheIndex to cache
        /// </summary>
        /// <param name="config"></param>
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public static async Task<bool> save( AmpConfig config, DateTime lastModified )
        {
            string collectionURL = PagesURLs.getCollectionURL(config);
            CollectionCacheIndex cacheIndex = new CollectionCacheIndex(collectionURL, DateTimeUtils.now(), lastModified);
            await CacheIndexStore.save<CollectionCacheIndex>(collectionURL, cacheIndex, config);

            return true;
        }
    }
}
