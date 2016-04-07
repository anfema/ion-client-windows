using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Ion.Caching
{
    public abstract class CacheIndex
    {
        public string filename { get; set; }
        
        public CacheIndex(){}

        public CacheIndex( string filename)
        {
            this.filename = filename;
        }
        
        // Use MD5 of request uri as filename
        public CacheIndex( Uri requestUrl )
        {
            filename = FilePaths.GetFileName(requestUrl.AbsoluteUri);
        }
    }
}
