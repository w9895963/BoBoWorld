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
            private static List<Dictionary<string, EventData>> localDicts = new List<Dictionary<string, EventData>>();

            public static bool HasData(string dataName)
            {
                return globalDict.ContainsKey(dataName);
            }

            public static void AddData<T>(EventData<T> globalDataT)
            {
                //*添加全局数据
                //是否已经是全局数据
                if (globalDict.Exist(globalDataT.Key, globalDataT))
                {
                    //报错
                    Debug.LogError($"当前数据{globalDataT.Key}已经是全局数据");
                    return;
                }

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
            //登记本地数据库
            public static void AddLocalDict(Dictionary<string, EventData> dateHolderDictStr)
            {
                localDicts.Add(dateHolderDictStr);
            }
            //移除本地数据库
            public static void RemoveLocalDict(Dictionary<string, EventData> dateHolderDictStr)
            {
                localDicts.Remove(dateHolderDictStr);
            }

            public static List<KeyValuePair<string, EventData>> GetDictList()
            {
                return globalDict.ToList();
            }
        }

        // 基于物体的事件数据存储字典, Unity 组件
        public class EventDataLocalMono : MonoBehaviour
        {
            private Dictionary<string, EventData> dateHolderDictStr = new Dictionary<string, EventData>();

            public static Dictionary<string, EventData> GetLocalDict(GameObject gameObject)
            {
                var store = gameObject.GetComponent<EventDataLocalMono>();
                if (store == null)
                {
                    store = gameObject.AddComponent<EventDataLocalMono>();
                    //登记
                    GlobalData.AddLocalDict(store.dateHolderDictStr);
                }
                return store.dateHolderDictStr;
            }

     

            public bool HasData(string name)
            {
                return dateHolderDictStr.ContainsKey(name);
            }


            //基础事件:摧毁
            private void OnDestroy()
            {
                //移除
                GlobalData.RemoveLocalDict(dateHolderDictStr);
            }
        }








        //类：事件数据
        public class EventData
        {
            //索引
            private string stringKey;
            public bool isGlobal;
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();
            //所有本地字典
            public GameObject gameObject;

            //引用：数据
            public Func<System.Object> dataGetter;

            public EventData(string stringKey, GameObject gameObject, bool isGlobal = false)
            {
                this.stringKey = stringKey;
                this.gameObject = gameObject;
                this.isGlobal = isGlobal;

                var localDict = EventDataLocalMono.GetLocalDict(gameObject);

                //如果本地字典中已经有了这个数据 
                if (localDict.ContainsKey(stringKey))
                {
                    //如果不是同一个则
                    EventData eventData = localDict[stringKey];
                    if (eventData != this)
                    {
                        //建立同步更新 

                    }
                }
                else
                {
                    //添加到本地字典
                    localDict.Add(stringKey, this);
                }
            }

            public string Key { get => stringKey; }




            //方法：获得短名
            public string GetShortName()
            {
                //以点分割取最末尾
                return stringKey.Split('.').Last();
            }



            //方法：获得数据
            public virtual System.Object GetData()
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
                //*如果已经是这种类型
                EventData<T> eventData = this as EventData<T>;
                //成功则返回
                if (eventData != null)
                    return eventData;

                //*如果是无参数类型
                if (this.GetType() == typeof(EventData))
                {
                    eventData = new EventData<T>(stringKey, gameObject, isGlobal, conditionActionList);
                    //复制数据

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



            //构造函数
            public EventData(string key, GameObject gameObject, bool isGlobal = false, List<ConditionAction> conditionActionList = null) : base(key, gameObject, isGlobal)
            {
                Debug.Log("创建事件数据" + key);
                dataGetter = () => { return data; };
                if (conditionActionList != null)
                    this.conditionActionList = conditionActionList;
                //获得字典
                var localDict = EventDataLocalMono.GetLocalDict(gameObject);
                //添加
                localDict[key] = this;
                //如果是全局数据
                if (isGlobal)
                {
                    //添加到全局字典
                    GlobalData.AddData(this);
                }

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
