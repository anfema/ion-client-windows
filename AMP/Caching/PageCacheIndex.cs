using Anfema.Amp.DataModel;
using Anfema.Amp.Pages;
using System;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class PageCacheIndex : CacheIndex
    {
        public DateTime lastChanged { get; set; }


        public PageCacheIndex( string filename, DateTime lastChanged ) : base( filename )
        {
            this.lastChanged = lastChanged;
        }

        
        /// <summary>
        /// Checks if the lastChanged DateTime of this object is older than the given parameter
        /// </summary>
        /// <param name="serverDate"></param>
        /// <returns></returns>
        public bool isOutdated( DateTime serverDate )
        {
            return lastChanged < serverDate;
        }


        /// <summary>
        /// Used to get a pageCacheIndex from cache
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        public static async Task<PageCacheIndex> retrieve( string requestURL, string collectionIdentifier )
        {
            return await CacheIndexStore.retrieve<PageCacheIndex>(requestURL, collectionIdentifier);
        }


        /// <summary>
        /// Saves a pageCacheIndex to cache
        /// </summary>
        /// <param name="page"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<bool> save( AmpPage page, AmpConfig config )
        {
            string url = PagesURLs.getPageURL(config, page.identifier);
            PageCacheIndex cacheIndex = new PageCacheIndex(url, page.last_changed);
            await CacheIndexStore.save<PageCacheIndex>(url, cacheIndex, config);

            return true;
        }
    }
}
