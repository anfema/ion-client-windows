using System.IO;


namespace Anfema.Amp.Exceptions
{
    public class CollectionNotAvailableException : IOException
    {
        public CollectionNotAvailableException() : base("Collection not in cache and no internet connection available") { }
    }
}
