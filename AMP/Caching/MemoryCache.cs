using Anfema.Amp.DataModel;
using Anfema.Amp.Pages;
using Anfema.Amp.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class MemoryCache
    {
        // Defines the maximum elements in cache. TODO: make this affect the cache size!
        private static int _pageMemoryCacheSize;
        
        // Holds the cached collection
        public AmpCollection collection { get; set; }

        // Holds all cached pages
        private Dictionary<string, AmpPage> _pageMemoryCache;


        /// <summary>
        /// Inits the memory cache with a given size
        /// </summary>
        /// <param name="pageMemoryCacheSize"></param>
        public MemoryCache( int pageMemoryCacheSize)
        {
            _pageMemoryCacheSize = pageMemoryCacheSize;
            collection = null;
            _pageMemoryCache = new Dictionary<string, AmpPage>(); 
        }

        /// <summary>
        /// Returns a page from the memory cache with the desired URL. If the page is not in cache, then null will be returned
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns></returns>
        private AmpPage getPage( string pageURL )
        {
            AmpPage page = null;
            _pageMemoryCache.TryGetValue(pageURL, out page);
            return page;
        }


        /// <summary>
        /// Called to recieve a desired page from memory cache
        /// </summary>
        /// <param name="pageIdentifier"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public AmpPage getPage( string pageIdentifier , AmpConfig config )
        {
            string pageUrl = PagesURLs.getPageURL(config, pageIdentifier);

            return getPage(pageUrl);
        }


        /// <summary>
        /// Saves a page to the memory cache
        /// </summary>
        /// <param name="page"></param>
        /// <param name="config"></param>
        public void savePage( AmpPage page, AmpConfig config )
        {
            string pageUrl = PagesURLs.getPageURL(config, page.identifier);
            _pageMemoryCache.Add(pageUrl, page);
        }



        /// <summary>
        /// Clears the whole memory cache
        /// </summary>
        public void clearPageMemomryCache()
        {
            _pageMemoryCache.Clear();
        }
    }
}
