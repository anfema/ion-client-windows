using System.IO;


namespace Anfema.Ion.Exceptions
{
    public class CollectionNotAvailableException : IOException
    {
        public CollectionNotAvailableException() : base("Collection not in cache and no internet connection available") { }
    }
}
