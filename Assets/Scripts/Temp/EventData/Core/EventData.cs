using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventDataS
{
    namespace Core
    {


        //类：事件数据
        public class EventData
        {
            //索引
            private string stringKey;
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();
            //所有本地字典
            private GameObject gameObject;
            public bool IsGlobal { get => gameObject == null; }
            public string Key { get => stringKey; }
            public GameObject GameObject { get => gameObject; }
            public virtual Func<object> DataGetter { get => null; }

            public EventData(string stringKey, GameObject gameObject = null)
            {
                this.stringKey = stringKey;
                this.gameObject = gameObject;
            }







            //方法：获得短名
            public string GetShortName()
            {
                //以点分割取最末尾
                return stringKey.Split('.').Last();
            }



            //方法：获得数据
            public  System.Object GetData()
            {
                if (DataGetter == null)
                {
                    return null;
                }
                return DataGetter();
            }



            public void ForceUpdateData()
            {
                // Debug.Log(conditionActionList.Count);//调试
                //执行conditionActionList
                conditionActionList.ForEach(conditionAction =>
                {
                    conditionAction.CheckAndRun();
                });
            }

        
        }
        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;
            public override Func<System.Object> DataGetter => () => { return data; };


            public EventData(string key, GameObject gameObject = null) : base(key, gameObject)
            {
            }

   



            //方法：设置数据
            public void SetData(T data)
            {
                // Debug.Log("SetData" + data.ToString());//调试
                //如果输入参数与data相同则不执行
                if (data == null && this.data == null)
                {
                    return;
                }
                if (data.Equals(this.data))
                {
                    return;
                }
                this.data = data;

                //更新数据
                ForceUpdateData();
                // Debug.Log("SetData" + data.ToString());//调试

            }

            //方法：获取数据
            public new T GetData()
            {
                return data;
            }

        
        



        }
    }
}
