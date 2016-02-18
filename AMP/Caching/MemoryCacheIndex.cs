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

        
        // TODO: keep it synchronized!
        public static T get<T>( string requestUrl, string collectionIdentifier ) where T : CacheIndex
        {
            Dictionary<string, CacheIndex> memoryCacheIndex = null;
            _memoryCacheIndices.TryGetValue(collectionIdentifier, out memoryCacheIndex);

            if( memoryCacheIndex == null )
            {
                return null;
            }

            CacheIndex cacheIndex;
            memoryCacheIndex.TryGetValue(requestUrl, out cacheIndex);

            return (T) cacheIndex;
        }


        // TODO: ensure that this is all synchronized
        public static void put<T>( string requestURL, string collectionIdentifier, T index ) where T : CacheIndex
        {

            Dictionary<string, CacheIndex> memoryCacheIndex;
            _memoryCacheIndices.TryGetValue(collectionIdentifier, out memoryCacheIndex);

            if( memoryCacheIndex == null )
            {
                memoryCacheIndex = new Dictionary<string, CacheIndex>(); // no size defined as in Android pendant
                _memoryCacheIndices.Add(collectionIdentifier, memoryCacheIndex);
            }

            memoryCacheIndex.Add(requestURL, index);
        }


        public static void clear( string collectionIdentifier )
        {
            Dictionary<string, CacheIndex> memoryCacheIndex = _memoryCacheIndices[collectionIdentifier];

            if( memoryCacheIndex != null )
            {
                memoryCacheIndex.Clear();
            }
        }


        public static void clearAll()
        {
            _memoryCacheIndices.Clear();
        }
    }
}
