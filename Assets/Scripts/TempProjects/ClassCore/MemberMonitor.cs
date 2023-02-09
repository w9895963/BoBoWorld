using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCore
{
    /// <summary> 列表监视器</summary>
    public class ListMonitor<T>
    {
        public void SelectManyToList<TValue>(Func<T, IEnumerable<TValue>> selector, ListMonitor<TValue> list)
        {
            _add += x => list.AddRange(selector(x));
            _remove += x => list.RemoveRange(selector(x));
        }
        public void SelectManyToList<TValue, TResult>(Func<T, IEnumerable<TValue>> selector, Func<T, TValue, TResult> resultSelector, ListMonitor<TResult> list)
        {
            _add += x =>
            {
                list.AddRange(selector(x).Select(y => resultSelector(x, y)));
            };
            _remove += x =>
            {
                list.RemoveRange(selector(x).Select(y => resultSelector(x, y)));
            };
        }
        public void SelectToDict<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TKey, TValue> valueSelector, CoreDict<TKey, TValue> dict)
        {
            _add += x =>
            {
                TKey key = keySelector(x);
                dict.Add(key, valueSelector(x, key));
            };
            _remove += x => dict.Remove(keySelector(x));
        }


        public void Update()
        {
            IEnumerable<T> saved = _list_saved;
            IEnumerable<T> currArray = _list;

            IEnumerable<T> toAdd = currArray.Except(saved).ToArray();
            IEnumerable<T> toRemove = saved.Except(currArray).ToArray();

            _list_saved = currArray.ToList();
            toAdd.ForEach(x => _add?.Invoke(x));
            toRemove.ForEach(x => _remove?.Invoke(x));
        }

        public void Add(T member)
        {
            _list.Add(member);
            _list_saved.Add(member);
            _add?.Invoke(member);
        }
        public void AddRange(IEnumerable<T> member)
        {
            member.ForEach(Add);
        }
        public void Remove(T member)
        {
            bool v = _list.Remove(member);
            if (v == false) return;
            _list_saved.Remove(member);
            _remove?.Invoke(member);
        }
        public void RemoveRange(IEnumerable<T> member)
        {
            member.ForEach(Remove);
        }
        public void ForEach(Action<T> action)
        {
            _list.ForEach(action);
        }











        public ListMonitor(List<T> list, Func<Config, Config> configGetter = null)
        {
            _list = list;
            if (configGetter != null)
            {
                var value = configGetter(new());
                _add = value.add;
                _remove = value.remove;
            }
        }

        public ListMonitor(Func<Config, Config> configGetter = null)
        {
            _list = new();
            if (configGetter != null)
            {
                var value = configGetter(new());
                _add = value.add;
                _remove = value.remove;
            }
        }

        private List<T> _list;
        private List<T> _list_saved = new();
        private Action<T> _add;
        private Action<T> _remove;
        public class Config
        {
            public Action<T> add;
            public Action<T> remove;
        }



    }


}