using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anfema.Amp.Caching
{
    class LRUCacheItem<K, V>
    {
        public K key { get; set; }
        public V value { get; set; }
        public long sizeInByte { get; set; }


        public LRUCacheItem(K k, V v)
        {
            key = k;
            value = v;

            // Approximate the size of the object in Bytes
            string valSerialized = JsonConvert.SerializeObject(v);
            sizeInByte = valSerialized.Length * sizeof(Char);
        }
    }
}
