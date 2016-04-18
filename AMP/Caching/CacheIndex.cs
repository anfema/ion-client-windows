using System;


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
            filename = FilePaths.getFileName(requestUrl.AbsoluteUri);
        }
    }
}
