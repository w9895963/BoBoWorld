using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace Configure.Inspector
{





    public abstract class CustomData : EventData.DataName.IDataNameInstance
    {
        public static List<EventData.DataName.IDataNameInstance> AllNames => EventData.DataNameD.AllNameInstance.ToList();



        public Action<string> onDataNameChangeAction = null;
        public Object creator = null;


        /// <summary>数据名, 设置时会触发事件改名事件</summary>
        public string DataName { get => dataName; set { dataName = value; onDataNameChangeAction?.Invoke(value); } }
        public Func<bool> enableGetter = () => true;
        public bool Enabled => enableGetter();
        public Type DataType => type;

        public void RenameAllSameNameTo(string newName)
        {
            var allNames = AllNames;
            var currentName = dataName;
            allNames.Where((data) => data.DataName == currentName).ForEach((data) =>
            {
                data.DataName = newName;
            });
        }



        public CustomData(Type type, string dataName = null, Func<bool> enableGetter = null)
        {

            this.type = type;
            if (enableGetter != null)
            {
                this.enableGetter = enableGetter;
            }



            var defaultName = this.dataName;

            if (dataName == null)
            {
                IEnumerable<string> enumerable = AllNames
                // .Log("AllNames")
                .Where(n => n.DataName.StartsWith(defaultName))
                // .Log("Where")
                .Select(n => n.DataName);
                int i = 0;
                while (enumerable.Contains(defaultName + i))
                {
                    i++;
                }
                this.dataName = defaultName + i;
            }
            else
            {
                this.dataName = dataName;
            }


            //清理掉重复的名字
            AllNames.RemoveAll((data) =>
            {
                bool re = false;
                if(data is CustomData customData)
                {
                    if (customData.creator == creator)
                    {
                        re = true;
                    }
                }
                return re;
            });

            AllNames.Add(this);
        }


        private string dataName = "未命名数据";
        private Type type;


      

    }



    public class CustomData<T> : CustomData, EventData.DataName.IDataNameInstance
    {
        public CustomData(string dataName = null, Func<bool> enableGetter = null)
        : base(typeof(T), dataName, enableGetter)
        { }




    }



}