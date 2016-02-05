using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    public class MemoryCacheIndex
    {
        /// <summary>
        /// Outer key: collection identifier
        /// inner key: request URL
        /// </summary>
        private static Dictionary<string, Dictionary<string, CacheIndex>> _memoryCacheIndices = new Dictionary<string, Dictionary<string, CacheIndex>>();

        /*
        // TODO: keep it synchronized!
        public static T? get<T>( string requestUrl, string collectionIdentifier, Type T)
        {
            Dictionary<string, CacheIndex> memoryCacheIndex = _memoryCacheIndices[collectionIdentifier];

            if( memoryCacheIndex == null )
            {
                return null;
            }

            return ( T ) (memoryCacheIndex[requestUrl]);
        }*/
    }
}
