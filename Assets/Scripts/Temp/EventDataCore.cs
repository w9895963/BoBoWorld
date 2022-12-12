using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventDataS
{
    namespace EventDataCore
    {
                // 全局事件数据存储字典
        public static class GlobalData
        {
            public static Dictionary<string, EventData> holderDictStr = new Dictionary<string, EventData>();
        }

        // 基于物体的事件数据存储字典, Unity 组件
        public class EventDataStoreMono : MonoBehaviour
        {
            public Dictionary<string, EventData> dateHolderDictStr = new Dictionary<string, EventData>();
        }








        //类：事件数据
        public class EventData
        {
            //索引
            private string stringKey;
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();

            //引用：数据
            public Func<System.Object> dataGetter;

            public EventData(string stringKey)
            {
                this.stringKey = stringKey;
            }

            public string Key { get => stringKey; }




            //方法：获得短名
            public string GetShortName()
            {
                //以点分割取最末尾
                return stringKey.Split('.').Last();
            }



            //方法：获得数据
            public System.Object GetData()
            {
                if (dataGetter == null)
                {
                    return null;
                }
                return dataGetter();
            }

            //方法：尝试还原为某种类型,失败则警告
            public EventData<T> ToEventData<T>()
            {
                EventData<T> eventData = this as EventData<T>;
                //成功则返回
                if (eventData != null)
                    return eventData;

                //如果自身类型和当前类的类型匹配
                if (typeof(EventData) == this.GetType())
                {
                    eventData = new EventData<T>(stringKey);
                    //复制数据
                    eventData.conditionActionList = conditionActionList;
                }
                //如果不匹配则警告
                else
                {
                    Debug.LogWarning($"当前数据类型为{this.GetType()}, 无法转换为{typeof(EventData<T>)}");
                }


                return eventData;
            }






            //静态方法：获取带参数的事件数据
            public static EventData<T> GetEventData<T>(string key, GameObject gameObject = null)
            {
                EventData<T> eventDataT;
                eventDataT = null;
                Dictionary<string, EventData> holderDict = GlobalData.holderDictStr;
                if (gameObject == null)
                {
                    if (GlobalData.holderDictStr.ContainsKey(key))
                    {
                        eventDataT = GlobalData.holderDictStr[key].ToEventData<T>();
                    }
                    else
                    {
                        eventDataT = new EventData<T>(key);
                        GlobalData.holderDictStr.Add(key, eventDataT);
                    }
                }
                else
                {
                    EventDataStoreMono eventDataMono = gameObject.GetComponent<EventDataStoreMono>();
                    if (eventDataMono == null)
                    {
                        eventDataMono = gameObject.AddComponent<EventDataStoreMono>();
                    }

                    if (eventDataMono.dateHolderDictStr.ContainsKey(key))
                    {
                        eventDataT = eventDataMono.dateHolderDictStr[key].ToEventData<T>();
                    }
                    else
                    {
                        eventDataT = new EventData<T>(key);
                        eventDataMono.dateHolderDictStr.Add(key, eventDataT);
                    }
                }
                return eventDataT;
            }

        }


        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;

            public EventData(string key) : base(key)
            {
                dataGetter = () => { return data; };
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


                //执行conditionActionList
                conditionActionList.ForEach(conditionAction =>
                {

                    conditionAction.CheckAndRun();



                });
            }
            //方法：获取数据
            public new T GetData()
            {
                return data;
            }

            //方法：数据是否相同
            public bool IsDataSame(T data)
            {
                if (data == null && this.data == null)
                {
                    return true;
                }
                if (data.Equals(this.data))
                {
                    return true;
                }
                return false;
            }
        }






        //条件操作
        public class ConditionAction
        {
            public System.Action action;
            public List<Func<bool>> conditionList = new List<Func<bool>>();


            //方法: 检测并运行
            public void CheckAndRun()
            {
                var isConditionMet = conditionList.All(condition => condition());



                if (isConditionMet)
                {
                    ActionF.QueueAction(action);
                }

            }

        }



    }
}
