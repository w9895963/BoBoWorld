using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventData.EventDataUtil;
using UnityEngine;

namespace EventData
{


    //*Region  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    #region            EventDataUtil
    namespace EventDataUtil
    {
        // 全局事件数据存储字典
        public static class GlobalData
        {
            public static Dictionary<Enum, EventData> holderDict = new Dictionary<Enum, EventData>();
        }

        // 基于物体的事件数据存储字典, Unity 组件
        public class EventDataStoreMono : MonoBehaviour
        {
            public Dictionary<System.Enum, EventData> dateHolderDict = new Dictionary<System.Enum, EventData>();
        }








        //类：事件数据
        public class EventData
        {
            //枚举
            public System.Enum enumKey;
            //当数据更新时执行动作
            public List<Action> onUpdatedAction = new List<Action>();
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();

            //引用：数据
            public Func<System.Object> dataGetter;








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
                    eventData = new EventData<T>();
                    //复制数据
                    eventData.enumKey = enumKey;
                    eventData.onUpdatedAction = onUpdatedAction;
                    eventData.conditionActionList = conditionActionList;
                }
                //如果不匹配则警告
                else
                {
                    Debug.LogWarning($"当前数据类型为{this.GetType()}, 无法转换为{typeof(EventData<T>)}");
                }


                return eventData;
            }




            //静态方法：获取事件数据
            public static EventData GetEventData(System.Enum name, GameObject gameObject = null)
            {
                EventData eventData = null;
                if (gameObject == null)
                {
                    if (GlobalData.holderDict.ContainsKey(name))
                    {
                        eventData = GlobalData.holderDict[name];
                    }
                    else
                    {
                        eventData = new EventData();
                        GlobalData.holderDict.Add(name, eventData);
                    }
                }
                else
                {
                    EventDataStoreMono eventDataMono = gameObject.GetComponent<EventDataStoreMono>();
                    if (eventDataMono == null)
                    {
                        eventDataMono = gameObject.AddComponent<EventDataStoreMono>();
                    }
                    if (eventDataMono.dateHolderDict.ContainsKey(name))
                    {
                        eventData = eventDataMono.dateHolderDict[name];
                    }
                    else
                    {
                        eventData = new EventData();
                        eventDataMono.dateHolderDict.Add(name, eventData);
                    }
                }
                return eventData;
            }

            //静态方法：获取带参数的事件数据
            public static EventData<T> GetEventData<T>(System.Enum name, GameObject gameObject = null)
            {
                EventData<T> eventDataT;
                eventDataT = null;
                Dictionary<Enum, EventData> holderDict = GlobalData.holderDict;
                if (gameObject == null)
                {
                    if (GlobalData.holderDict.ContainsKey(name))
                    {
                        eventDataT = GlobalData.holderDict[name].ToEventData<T>();
                    }
                    else
                    {
                        eventDataT = new EventData<T>();
                        GlobalData.holderDict.Add(name, eventDataT);
                    }
                }
                else
                {
                    EventDataStoreMono eventDataMono = gameObject.GetComponent<EventDataStoreMono>();
                    if (eventDataMono == null)
                    {
                        eventDataMono = gameObject.AddComponent<EventDataStoreMono>();
                    }

                    if (eventDataMono.dateHolderDict.ContainsKey(name))
                    {
                        eventDataT = eventDataMono.dateHolderDict[name].ToEventData<T>();
                    }
                    else
                    {
                        eventDataT = new EventData<T>();
                        eventDataMono.dateHolderDict.Add(name, eventDataT);
                    }
                }
                eventDataT.enumKey = name;
                return eventDataT;
            }

        }


        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;
            public List<Action<T>> onUpdatedActionT = new List<Action<T>>();

            public EventData() : base()
            {
                dataGetter = () => { return data; };
            }



            //方法：设置数据
            public void SetData(T data)
            {
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


                //执行onUpdatedAction
                onUpdatedAction.ForEach(action => action());
                onUpdatedActionT.ForEach(action => action(data));

                //执行conditionActionList
                conditionActionList.ForEach(conditionAction =>
                {
                    bool isConditionMet = conditionAction.conditionList.All(condition => condition());


                    if (isConditionMet)
                    {
                        conditionAction.action();
                    }
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
        }




    }


    #endregion
    //*Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑










    //类型： EventDataF
    public class EventDataF
    {


        //方法：获取事件数据赋值Action
        public static Action<T> GetDataSetter<T>(System.Enum name, GameObject gameObject = null)
        {
            EventData<T> eventDataT = EventDataUtil.EventData.GetEventData<T>(name, gameObject);


            return (T data) => { eventDataT.SetData(data); };
        }

        //方法：获取数据操作器，全局数据
        public static EventDataHandler<T> GetData_global<T>(System.Enum name)
        {
            EventData<T> eventDataT = EventDataUtil.EventData.GetEventData<T>(name);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventDataT);
            return dataOperator;
        }
        //方法：获取数据操作器，局部数据
        public static EventDataHandler<T> GetData_local<T>(GameObject gameObject, System.Enum name)
        {
            EventData<T> eventDataT = EventDataUtil.EventData.GetEventData<T>(name, gameObject);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventDataT);
            return dataOperator;
        }




        ///<summary> 创建数据条件 </summary>
        public static void CreateDataCondition(Action action, Enum[] dataIsTrue = null, Enum[] dataIsFalse = null, Enum[] dataIsUpdate = null, (Enum dataName, Func<bool> conditionCheck)[] customConditions = null)
        {


            //收集所有数据的枚举
            List<Enum> dataNameList = new List<Enum>();
            if (dataIsTrue != null)
                dataNameList.AddRange(dataIsTrue);
            if (dataIsFalse != null)
                dataNameList.AddRange(dataIsFalse);
            if (dataIsUpdate != null)
                dataNameList.AddRange(dataIsUpdate);
            if (customConditions != null)
                dataNameList.AddRange(customConditions.Select(condition => condition.dataName));

            //收集所有数据的事件数据
            List<EventDataUtil.EventData> eventDataList = new List<EventDataUtil.EventData>();

            List<EventDataUtil.EventData<bool>> eventDataList_true_bool = new List<EventDataUtil.EventData<bool>>();
            if (dataIsTrue != null)
                eventDataList_true_bool = dataIsTrue.Select(dataName => EventDataUtil.EventData.GetEventData<bool>(dataName)).ToList();
            List<EventDataUtil.EventData<bool>> eventDataList_false_bool = new List<EventDataUtil.EventData<bool>>();
            if (dataIsFalse != null)
                eventDataList_false_bool = dataIsFalse.Select(dataName => EventDataUtil.EventData.GetEventData<bool>(dataName)).ToList();
            List<EventDataUtil.EventData> eventDataList_update = new List<EventDataUtil.EventData>();
            if (dataIsUpdate != null)
                eventDataList_update = dataIsUpdate.Select(dataName => EventDataUtil.EventData.GetEventData(dataName)).ToList();
            List<EventDataUtil.EventData> eventDataList_custom = new List<EventDataUtil.EventData>();
            if (customConditions != null)
                eventDataList_custom = customConditions.Select(condition => EventDataUtil.EventData.GetEventData(condition.dataName)).ToList();


            eventDataList.AddRange(eventDataList_true_bool);
            eventDataList.AddRange(eventDataList_false_bool);
            eventDataList.AddRange(eventDataList_update);
            eventDataList.AddRange(eventDataList_custom);

            //收集所有数据的条件
            ConditionAction conditionAction = new ConditionAction();
            eventDataList_true_bool.ForEach(eventData => conditionAction.conditionList.Add(() => { return eventData.IsDataSame(true); }));
            eventDataList_false_bool.ForEach(eventData => conditionAction.conditionList.Add(() => { return eventData.IsDataSame(false); }));
            eventDataList_update.ForEach(eventData => conditionAction.conditionList.Add(() => { return true; }));
            customConditions.ForEach(condition => conditionAction.conditionList.Add(condition.conditionCheck));


            conditionAction.action = action;

            //将条件添加到所有数据的事件数据中
            eventDataList.ForEach(eventData =>
            {
                eventData.conditionActionList.Add(conditionAction);
            });

        }

        ///<summary> 创建数据条件,返回运行器 </summary>
        public static (Action Enable, Action Disable) CreateDataCondition(Action action, params (EventDataUtil.EventData data, Func<bool> check)[] conditionChecks)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.action = action;
            conditionChecks.ForEach(conditionCheck =>
            {
                conditionAction.conditionList.Add(conditionCheck.check);
            });
            Action enable = () =>
            {
                conditionChecks.ForEach(conditionCheck =>
                {
                    conditionCheck.data.conditionActionList.Add(conditionAction);
                });
            };
            Action disable = () =>
            {
                conditionChecks.ForEach(conditionCheck =>
                {
                    conditionCheck.data.conditionActionList.Remove(conditionAction);
                });
            };
            return (enable, disable);
        }






        ///<summary> 数据执行器，更新 </summary>
        public static void OnDataUpdate<T>(Action<T> action, Enum dataName, GameObject gameObject = null)
        {
            EventDataUtil.EventData<T> eventData = EventDataUtil.EventData.GetEventData<T>(dataName, gameObject);

            eventData.onUpdatedAction.Add(() =>
            {
                action(eventData.GetData());
            });
        }

        /// <summary>数据执行器，更新,本地数据,返回运行器 </summary>
        public static (Action Enable, Action Disable) OnDataUpdate_Local<T>(GameObject gameObject, Enum dataName, Action<T> action)
        {
            EventDataUtil.EventData<T> eventData = EventDataUtil.EventData.GetEventData<T>(dataName, gameObject);

            Action act = () => action(eventData.GetData());

            //返回运行器
            return
            (
            Enable: () => eventData.onUpdatedAction.Add(act),
            Disable: () => eventData.onUpdatedAction.Remove(act)
            );
        }


        ///<summary> 数据执行器，更新,全局数据,返回运行器 </summary>

        public static (Action Enable, Action Disable) OnDataUpdate_Global<T>(Enum dataName, Action<T> action)
        {

            EventDataUtil.EventData<T> eventData = EventDataUtil.EventData.GetEventData<T>(dataName);

            Action act = () => action(eventData.GetData());


            //返回运行器
            return (
            Enable: () => eventData.onUpdatedAction.Add(act),
            Disable: () => eventData.onUpdatedAction.Remove(act)
            );
        }


    }





    //类型:数据操作器
    public class EventDataHandler<T>
    {
        //字段：事件数据
        private EventData<T> eventDataT;

        public EventDataHandler(EventData<T> eventDataT)
        {
            this.eventDataT = eventDataT;
        }


        //属性：数据
        public T Data
        {
            get => eventDataT.GetData();
            set => eventDataT.SetData(value);
        }

        //方法：同步数据
        public (Action Enable, Action Disable) SetDataTo(Action<T> act)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.conditionList.Add(() => { return true; });
            conditionAction.action = () => { act(Data); };

            return (Enable: () => eventDataT.conditionActionList.Add(conditionAction), Disable: () => eventDataT.conditionActionList.Remove(conditionAction));
        }


        //方法：获得数据判断方法，数据更新
        public (EventDataUtil.EventData data, Func<bool> check) OnUpdate()
        {
            return (eventDataT, () => { return true; });
        }
        //方法：获得数据判断方法，数据为真
        public (EventDataUtil.EventData data, Func<bool> check) OnTrue()
        {
            return (eventDataT, () => { return Data.Equals(true); });
        }
        //方法：获得数据判断方法，数据为假
        public (EventDataUtil.EventData data, Func<bool> check) OnFalse()
        {
            return (eventDataT, () => { return Data.Equals(false); });
        }
        //方法：获得数据判断方法，自定义判断
        public (EventDataUtil.EventData data, Func<bool> check) OnCheck(Func<bool> check)
        {
            return (eventDataT, check);
        }

    }





}