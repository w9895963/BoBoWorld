using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreClass
{
    /// <summary> 列表监视器</summary>
    public partial class ListMonitor<T>
    {
        public void SelectManyToList<TValue>(Func<T, IEnumerable<TValue>> selector, ListMonitor<TValue> list)
        {
            _onAdd += x => list.AddRange(selector(x));
            _onRemove += x => list.RemoveRange(selector(x));
        }

        public void SelectToDict<TKey, TValue>(Func<T, TKey> keySelector, Func<T, TKey, TValue> valueSelector, CoreDict<TKey, TValue> dict)
        {
            _onAdd += x =>
            {
                TKey key = keySelector(x);
                dict.Add(key, valueSelector(x, key));
            };
            _onRemove += x => dict.Remove(keySelector(x));
        }
        public void SelectToDict<V>(Func<T, V> VSelector, CoreDict<T, V> dict)
        {
            _onAdd += x =>
            {
                dict.Add(x, VSelector(x));
            };
            _onRemove += x => dict.Remove(x);
        }



        public void Update()
        {
            IEnumerable<T> saved = _list_saved;
            IEnumerable<T> currArray = _list;

            IEnumerable<T> toAdd = currArray.Except(saved).ToArray();
            IEnumerable<T> toRemove = saved.Except(currArray).ToArray();

            // _list_saved = currArray.ToList();
            _list_saved.AddRange(toAdd);
            _list_saved.RemoveAll(toRemove);
            toAdd.ForEach(x => _onAdd?.Invoke(x));
            toRemove.ForEach(x => _onRemove?.Invoke(x));
        }

        public void Add(T member)
        {
            _list.Add(member);
            _list_saved.Add(member);
            _onAdd?.Invoke(member);
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
            _onRemove?.Invoke(member);
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
                _onAdd = value.add;
                _onRemove = value.remove;
            }
        }

        public ListMonitor(Func<Config, Config> configGetter = null)
        {
            _list = new();
            if (configGetter != null)
            {
                var value = configGetter(new());
                _onAdd = value.add;
                _onRemove = value.remove;
            }
        }

        private List<T> _list;
        private List<T> _list_saved = new();
        private Action<T> _onAdd;
        private Action<T> _onRemove;

        public class Config
        {
            public Action<T> add;
            public Action<T> remove;
        }



    }

    ///*新方法
    public partial class ListMonitor<T> : IActionList<T>
    {
        public IActionList<TSelect> Select<TSelect>(Func<T, TSelect> selector)
        {
            throw new NotImplementedException();
        }

        public IActionList<TSelect> SelectMany<TSelect>(Func<T, IEnumerable<TSelect>> selector)
        {
            throw new NotImplementedException();
        }

        public event Action<T> OnAdd
        {
            add => _onAdd += value;
            remove => _onAdd -= value;
        }
        public event Action<T> OnRemove
        {
            add => _onRemove += value;
            remove => _onRemove -= value;
        }
    }





    /// <summary> 虚拟列表</summary>
    public class ListVirtual<TSource, T> : IActionList<T>
    {
        public IActionList<TSelect> Select<TSelect>(Func<T, TSelect> selector)
        {
            ListVirtual<T, TSelect> newList = new ListVirtual<T, TSelect>()
            {
            };

            onAdd += newListItem =>
            {
                newList.onAdd.Invoke(newList.dict.GetOrCreate(newListItem, () => selector(newListItem)));
            };
            onRemove += newListItem =>
            {
                newList.onRemove.Invoke(newList.dict.GetOrCreate(newListItem, () => selector(newListItem)));
            };


            return newList;
        }

        public IActionList<TSelect> SelectMany<TSelect>(Func<T, IEnumerable<TSelect>> selector)
        {
            throw new NotImplementedException();
        }

        public event Action<T> OnAdd
        {
            add => onAdd += value;
            remove => onAdd -= value;
        }
        public event Action<T> OnRemove
        {
            add => onRemove += value;
            remove => onRemove -= value;
        }




        private Action<T> onAdd;
        private Action<T> onRemove;
        private Dictionary<TSource, T> dict = new Dictionary<TSource, T>();


    }



    public interface IActionList<T>
    {
        public IActionList<TSelect> Select<TSelect>(Func<T, TSelect> selector);
        public IActionList<TSelect> SelectMany<TSelect>(Func<T, IEnumerable<TSelect>> selector);

        public event Action<T> OnAdd;
        public event Action<T> OnRemove;



    }
    public class ActionList<T> : IActionList<T>
    {
        public IActionList<TSelect> Select<TSelect>(Func<T, TSelect> selector)
        {
            throw new NotImplementedException();
        }
        public IActionList<TSelect> SelectMany<TSelect>(Func<T, IEnumerable<TSelect>> selector)
        {
            throw new NotImplementedException();
        }



        public event Action<T> OnAdd
        {
            add => onAddAction += value;
            remove => onAddAction -= value;
        }
        public event Action<T> OnRemove
        {
            add => onRemoveAction += value;
            remove => onRemoveAction -= value;
        }







        private Action<T> onAddAction;
        private Action<T> onRemoveAction;


    }

}