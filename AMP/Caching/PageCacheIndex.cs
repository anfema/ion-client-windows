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

        
        public bool isOutdated( DateTime serverDate )
        {
            return lastChanged < serverDate;
        }


        public static async Task<PageCacheIndex> retrieve( string requestURL, string collectionIdentifier )
        {
            return await CacheIndexStore.retrieve<PageCacheIndex>(requestURL, collectionIdentifier);
        }


        public static async Task<bool> save( AmpPage page, AmpConfig config )
        {
            string url = PagesURLs.getPageURL(config, page.identifier);
            PageCacheIndex cacheIndex = new PageCacheIndex(url, page.last_changed);
            await CacheIndexStore.save<PageCacheIndex>(url, cacheIndex, config);

            return true;
        }
    }
}
