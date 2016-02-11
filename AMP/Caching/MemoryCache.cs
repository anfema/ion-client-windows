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
        private static int _pageMemoryCacheSize = 100;
        
        public AmpCollection collection { get; set; }

        private Dictionary<string, AmpPage> _pageMemoryCache;

        public MemoryCache( int pageMemoryCacheSize)
        {
            _pageMemoryCacheSize = pageMemoryCacheSize;
            collection = null;
            _pageMemoryCache = new Dictionary<string, AmpPage>(); 
        }


        public AmpPage getPage( string pageURL )
        {
            AmpPage page = null;
            _pageMemoryCache.TryGetValue(pageURL, out page);
            return page;
        }


        public AmpPage getPage( string pageIdentifier , AmpConfig config )
        {
            string pageUrl = PagesURLs.getPageURL(config, pageIdentifier);

            return getPage(pageUrl);
        }


        public void savePage( AmpPage page, AmpConfig config )
        {
            string pageUrl = PagesURLs.getPageURL(config, page.identifier);
            _pageMemoryCache.Add(pageUrl, page);
        }


        public void clearPageMemomryCache()
        {
            _pageMemoryCache.Clear();
        }

    }
}
