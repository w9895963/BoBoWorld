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
            public static Dictionary<string, EventData> globalDict = new Dictionary<string, EventData>();
            //所有本地字典
            public static List<Dictionary<string, EventData>> localDicts = new List<Dictionary<string, EventData>>();

            public static bool HasData(string dataName)
            {
                return globalDict.ContainsKey(dataName);
            }

            public static void AddData<T>(EventData<T> globalDataT)
            {
                //*添加全局数据
                //是否已经是全局数据
                if (globalDataT.isGlobal)
                {
                    //报错
                    Debug.LogError($"当前数据{globalDataT.Key}已经是全局数据，有冲突风险");
                }
                //设为全局数据
                globalDataT.isGlobal = true;
                //添加到全局字典
                globalDict.Add(globalDataT.Key, globalDataT);
                //*将所有本地字典同步到全局字典
                foreach (var localDic in localDicts)
                {
                    //如果本地字典中已经有了这个数据 //则合并数据
                    if (localDic.ContainsKey(globalDataT.Key))
                    {
                        //如果不是同一个则
                        EventData eventData = localDic[globalDataT.Key];
                        if (eventData != globalDataT)
                        {
                            //建立同步更新 
                            globalDataT.LinkTo(eventData.ToEventData<T>());
                        }
                    }
                }
                //设置全局数据完成, 执行一次数据更新
                globalDataT.UpdateData();
            }
        }

        // 基于物体的事件数据存储字典, Unity 组件
        public class EventDataStoreMono : MonoBehaviour
        {
            public Dictionary<string, EventData> dateHolderDictStr = new Dictionary<string, EventData>();

            public static Dictionary<string, EventData> GetLocalDict(GameObject gameObject)
            {
                var store = gameObject.GetComponent<EventDataStoreMono>();
                if (store == null)
                {
                    store = gameObject.AddComponent<EventDataStoreMono>();
                    //登记
                    GlobalData.localDicts.Add(store.dateHolderDictStr);
                }
                return store.dateHolderDictStr;
            }

            public bool HasData(string name)
            {
                return dateHolderDictStr.ContainsKey(name);
            }
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










        }


        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;
            public bool isGlobal;

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

                //更新数据
                UpdateData();

            }

            public void UpdateData()
            {
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



            //方法：建立同步更新    
            public void LinkTo(EventData<T> eventData)
            {
                ConditionAction conditionAction = new ConditionAction();
                conditionAction.action = () => eventData.SetData(this.data);
                conditionActionList.Add(conditionAction);
            }




        }






        //条件操作
        public class ConditionAction
        {
            public System.Action action;
            public System.Action actionOnFail;
            public List<Func<bool>> conditionList = new List<Func<bool>>();


            //方法: 检测并运行
            public void CheckAndRun()
            {
                var isConditionMet = conditionList.All(condition => condition());



                if (isConditionMet)
                {
                    ActionF.QueueAction(action);
                }
                else
                {

                    ActionF.QueueAction(actionOnFail);
                }

            }

        }



    }
}
