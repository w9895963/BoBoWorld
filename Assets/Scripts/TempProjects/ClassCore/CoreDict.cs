using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCore
{
    public class CoreDict<K, V>
    {
        public CoreDict(Action<K, V> add = null, Action<K, V> remove = null)
        {
            _add = add;
            _remove = remove;
        }
        public CoreDict(Func<Config, Config> configGetter)
        {
            var value = configGetter(new());
            _add = value.add;
            _remove = value.remove;
        }
        public class Config
        {
            public Action<K, V> add;
            public Action<K, V> remove;
        }
        private Dictionary<K, V> _dict = new();
        private Action<K, V> _add;
        private Action<K, V> _remove;


        /// <summary> 所有值</summary>
        public IEnumerable<V> Values => _dict.Values;
        /// <summary> 所有键</summary>
        public IEnumerable<K> Keys => _dict.Keys;


        /// <summary> 添加</summary>
        public void Add(K key, V value)
        {
            _dict.Add(key, value);
            _add?.Invoke(key, value);
        }

        /// <summary> 删除</summary>
        public void Remove(K key)
        {
            V v1 = _dict[key];
            bool v = _dict.Remove(key);
            if (!v) return;
            _remove?.Invoke(key, v1);
        }
    }
}