using System;
using System.Collections.Generic;
using System.Linq;
using EventData.Core;
using UnityEngine;
using Object = System.Object;

namespace EventData
{
    namespace Core
    {


        //类：事件数据
        public class EventData
        {
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();


            public GameObject GameObject { get => gameObject; }
            public System.Type Type { get => type; }


            public bool IsGlobal { get => gameObject == null; }
            public string Key { get => stringKey; }





            public EventData(string stringKey, GameObject gameObject = null, System.Type type = null)
            {
                this.stringKey = stringKey;
                this.gameObject = gameObject;
                this.type = type;
            }


            //方法：获得数据
            public System.Object GetData()
            {
                return DataGetter();
            }
            //方法：修改数据
            public void SetIfNotEqual(Object data)
            {
                //如果不相同则修改
                if (!System.Object.Equals(GetData(), data))
                {
                    ModifyData_((Object) => { return data; });
                }
            }




            public void ForceUpdateData()
            {
                //分离执行
                conditionActionList.ForEach(conditionAction =>
               {
                   SeparatedExecutionQueue.AddCondition(conditionAction);
               });

            }





            protected virtual Func<object> DataGetter { get => null; }
            protected virtual void ModifyData_(Func<Object, Object> modifyFunc) { }


            //索引
            private string stringKey;
            //所有本地字典
            private GameObject gameObject;
            private System.Type type;
        }
        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;


            public EventData(string key, GameObject gameObject = null) : base(key, gameObject, typeof(T))
            {

            }





            /// <summary>设置数据, 如果不相同则唤起数据事件</summary>
            public void SetIfNotEqual(T data)
            {
                //如果不相同则修改
                if (!System.Object.Equals(this.data, data))
                {
                    ModifyData((T) => { return data; });
                }

            }
            /// <summary>设置数据, 唤起数据事件</summary>
            public void Set(T data)
            {
                ModifyData((T) => { return data; });
            }
            /// <summary>修改数据, 唤起数据事件</summary>
            public void Modify(Func<T, T> modifyFunc)
            {
                ModifyData(modifyFunc);
            }

            ///<summary> 方法：获取数据 </summary>
            public new T GetData()
            {
                return data;
            }



            protected override Func<System.Object> DataGetter => () => { return data; };
            protected override void ModifyData_(Func<Object, Object> modifyFunc)
            {
                //更新数据
                SeparatedExecutionQueue.AddDataAction(() =>
                {
                    this.data = (T)modifyFunc(this.data);
                    //强制更新
                    this.ForceUpdateData();
                });
            }




            private void ModifyData(Func<T, T> modifyFunc)
            {
                //更新数据
                SeparatedExecutionQueue.AddDataAction(() =>
                {
                    this.data = modifyFunc(this.data);
                    //强制更新
                    this.ForceUpdateData();
                });

            }







        }
    }
}
