using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreClass
{
    public class CoreDict<K, V>
    {  /// <summary> 所有值</summary>
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
            V value = _dict[key];
            bool v = _dict.Remove(key);
            if (!v) return;
            _remove?.Invoke(key, value);
        }
        public bool ContainsKey(K key) => _dict.ContainsKey(key);





        public CoreDict(Action<K, V> add = null, Action<K, V> remove = null)
        {
            _add = add;
            _remove = remove;
        }
        public CoreDict(Func<Config, Config> configGetter)
        {
            var value = configGetter(new());
            _add = value.OnAdd;
            _remove = value.OnRemove;
        }
        public class Config
        {
            public Action<K, V> OnAdd;
            public Action<K, V> OnRemove;
        }
        private Dictionary<K, V> _dict = new();
        private Action<K, V> _add;
        private Action<K, V> _remove;



    }
}