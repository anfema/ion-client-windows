using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public abstract class CacheIndex
    {
        private string _filename;

        public CacheIndex( string filename)
        {
            this._filename = filename;
        }
        

        // Use MD5 of request uri as filename
        public CacheIndex( Uri requestUrl )
        {
            _filename = FilePaths.getFileName(requestUrl.AbsoluteUri);
        }


        public string getFilename
        {
            get
            {
                return _filename;
            }

            set
            {
                _filename = value;
            }
        }
    }
}
