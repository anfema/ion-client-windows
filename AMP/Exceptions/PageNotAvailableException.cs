using System.IO;


namespace Anfema.Ion.Exceptions
{
    public class PageNotAvailableException : IOException
    {
        public PageNotAvailableException() : base ("Page is not in cache and no internet connection available") { }
    }
}
