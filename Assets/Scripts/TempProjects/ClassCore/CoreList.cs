using System;
using System.Collections.Generic;

namespace ClassCore
{
    ///<summary> 这个类用来添加和删除成员 </summary>
    public class CoreList<T>
    {
        public CoreList(Action<T> add, Action<T> remove)
        {
            _remove = remove;
            _add = add;
        }

        public CoreList(Func<Config, Config> configGetter)
        {
            var value = configGetter(new());
            _add = value.add;
            _remove = value.remove;
        }
        public class Config
        {
            public Action<T> add;
            public Action<T> remove;
        }
        private Action<T> _add;
        private Action<T> _remove;
        private List<T> _members = new();



        public void Add(T member)
        {
            _members.Add(member);
            _add?.Invoke(member);
        }
        public void Add(IEnumerable<T> member)
        {
            member.ForEach(Add);
        }
        public void RemoveAll(T member)
        {
            int v = _members.RemoveAll(x => x.Equals(member));
            if (v == 0) return;
            _remove?.Invoke(member);
        }
        public void RemoveAll(IEnumerable<T> member)
        {
            member.ForEach(RemoveAll);
        }








    }






}