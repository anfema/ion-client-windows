using Anfema.Ion.Utils;
using System.Collections.Generic;
using System.Diagnostics;

namespace Anfema.Ion.Caching
{
    public class LRUCache<K, V>
    {
        // Private members for capacity, storing and sorting of the cache items
        private long _capacity, _currentlyUsedCapacity;
        private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
        private LinkedList<LRUCacheItem<K, V>> lruList = new LinkedList<LRUCacheItem<K, V>>();


        public LRUCache(long capacity)
        {
            _capacity = capacity;
            _currentlyUsedCapacity = 0;
        }


        /// <summary>
        /// Gets a item from the cache
        /// </summary>
        /// <param name="key">Key for the desired item</param>
        /// <returns>Item from the cache</returns>
        public V get(K key)
        {
            LinkedListNode<LRUCacheItem<K, V>> node;
            if( cacheMap.TryGetValue( key, out node ) )
            {
                V value = node.Value.value;
                lruList.Remove(node);
                lruList.AddLast(node);
                return value;
            }

            return default(V);
        }


        /// <summary>
        /// Adds a new key value pair to the cache
        /// </summary>
        /// <param name="key">Key used for later getting of the item</param>
        /// <param name="val">Value of the item</param>
        public void add(K key, V val)
        {
            // Check cache for possible duplicate
            removePossibleDouplicate( key );

            // Generate new LRUCacheItem 
            LRUCacheItem<K, V> cacheItem = new LRUCacheItem<K, V>(key, val);

            // Increase used capacity
            _currentlyUsedCapacity += cacheItem.sizeInByte;

            // Check if the maximum capacity is reached
            if( _currentlyUsedCapacity > _capacity )
            {
                removeLastRecentlyUsedItem();
            }

            // Add the new node to the cache
            LinkedListNode<LRUCacheItem<K, V>> node = new LinkedListNode<LRUCacheItem<K, V>>(cacheItem);
            lruList.AddLast(node);
            cacheMap.Add(key, node);
        }


        /// <summary>
        /// Clears the whole LRU Cache
        /// </summary>
        public void clear()
        {
            // Set currently used capacity to 0
            _currentlyUsedCapacity = 0;

            // Remove all items from cache
            cacheMap.Clear();

            // Clear sorting list
            lruList.Clear();
        }


        /// <summary>
        /// Removes the least used item in the LRU Cache
        /// </summary>
        private void removeLastRecentlyUsedItem()
        {
            // Remove from LRUPriority
            LinkedListNode<LRUCacheItem<K, V>> node = lruList.First;
            lruList.RemoveFirst();

            // Decrease used memory
            _currentlyUsedCapacity -= node.Value.sizeInByte;

            // Remove from cache
            cacheMap.Remove(node.Value.key);
        }


        /// <summary>
        /// Checks the cache for a possible duplicate and removes it
        /// </summary>
        /// <param name="key"></param>
        private void removePossibleDouplicate( K key )
        {
            // Check if there is a existing node with the specified key
            LinkedListNode<LRUCacheItem<K, V>> node;
            if( cacheMap.TryGetValue( key, out node ) )
            {
                // Decrease capacity
                _capacity -= node.Value.sizeInByte;

                // Remove node from the lruList
                lruList.Remove( node );

                // Remove key from the cacheMap
                cacheMap.Remove( key );

                IonLogging.log( "Removed duplicate node from LRU-Cache with the key: " + key.ToString(), IonLogMessageTypes.INFORMATION );
            }
        }
    }
}