using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LCW.Framework.Common.Ioc
{
    public class Multimap<K, V> : IEnumerable<KeyValuePair<K, ICollection<V>>>
    {
        private readonly Dictionary<K, ICollection<V>> _items = new Dictionary<K, ICollection<V>>();
        public ICollection<V> this[K key]
        {
            get
            {
                Ensure.ArgumentNotNull(key, "key");
                if (!_items.ContainsKey(key))
                    _items[key] = new List<V>();
                return _items[key];
            }
        }
        public ICollection<K> Keys
        {
            get
            {
                return _items.Keys;
            }
        }
        public ICollection<ICollection<V>> Values
        {
            get
            {
                return _items.Values;
            }
        }
        public void Add(K key, V value)
        {
            Ensure.ArgumentNotNull(key, "key");
            Ensure.ArgumentNotNull(value, "value");
            this[key].Add(value);
        }
        public bool Remove(K key, V value)
        {
            Ensure.ArgumentNotNull(key, "key");
            Ensure.ArgumentNotNull(value, "value");
            if (!_items.ContainsKey(key))
                return false;
            return _items[key].Remove(value);
        }
        public bool RemoveAll(K key)
        {
            Ensure.ArgumentNotNull(key, "key");
            return _items.Remove(key);
        }
        public void Clear()
        {
            _items.Clear();
        }
        public bool ContainsKey(K key)
        {
            Ensure.ArgumentNotNull(key, "key");
            return _items.ContainsKey(key);
        }
        public bool ContainsValue(K key, V value)
        {
            Ensure.ArgumentNotNull(key, "key");
            Ensure.ArgumentNotNull(value, "value");
            return _items.ContainsKey(key) && _items[key].Contains(value);
        }
        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator<KeyValuePair<K, ICollection<V>>> IEnumerable<KeyValuePair<K, ICollection<V>>>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
