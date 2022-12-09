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
        public class EventDataMono : MonoBehaviour
        {
            public Dictionary<System.Enum, EventData> dateHolderDict = new Dictionary<System.Enum, EventData>();
        }






        //类：事件数据
        public class EventData
        {

            //当数据更新时执行动作
            public List<Action> onUpdatedAction = new List<Action>();
            //条件与动作列表
            public List<ConditionAction> conditionActionList = new List<ConditionAction>();







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
                    EventDataMono eventDataMono = gameObject.GetComponent<EventDataMono>();
                    if (eventDataMono == null)
                    {
                        eventDataMono = gameObject.AddComponent<EventDataMono>();
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
                if (gameObject == null)
                {
                    if (GlobalData.holderDict.ContainsKey(name))
                    {
                        eventDataT = GlobalData.holderDict[name] as EventData<T>;
                    }
                    else
                    {
                        eventDataT = new EventData<T>();
                        GlobalData.holderDict.Add(name, eventDataT);
                    }
                }
                else
                {
                    EventDataMono eventDataMono = gameObject.GetComponent<EventDataMono>();
                    if (eventDataMono == null)
                    {
                        eventDataMono = gameObject.AddComponent<EventDataMono>();
                    }

                    if (eventDataMono.dateHolderDict.ContainsKey(name))
                    {
                        eventDataT = eventDataMono.dateHolderDict[name] as EventData<T>;
                    }
                    else
                    {
                        eventDataT = new EventData<T>();
                        eventDataMono.dateHolderDict.Add(name, eventDataT);
                    }
                }
                return eventDataT;
            }

        }

        
        //类：带参数的事件数据
        public class EventData<T> : EventData
        {
            public T data;

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
            public T GetData()
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


        //方法：生成事件数据限制条件
        public static void CreateDataCondition(Action action, Enum[] dataIsTrue, Enum[] dataIsFalse, (Enum dataName, Func<bool> conditionCheck)[] customConditions = null)
        {


            //收集所有数据的枚举
            List<Enum> dataNameList = new List<Enum>();
            dataNameList.AddRange(dataIsTrue);
            dataNameList.AddRange(dataIsFalse);
            if (customConditions != null)
            {
                dataNameList.AddRange(customConditions.Select(condition => condition.dataName));
            }

            //收集所有数据的事件数据
            List<EventDataUtil.EventData> eventDataList = new List<EventDataUtil.EventData>();
            List<EventDataUtil.EventData<bool>> eventDataList_true_bool = dataIsTrue.Select(dataName => EventDataUtil.EventData.GetEventData<bool>(dataName)).ToList();
            List<EventDataUtil.EventData<bool>> eventDataList_false_bool = dataIsFalse.Select(dataName => EventDataUtil.EventData.GetEventData<bool>(dataName)).ToList();
            List<EventDataUtil.EventData> eventDataList_custom = null;
            if (customConditions != null)
            {
                eventDataList_custom = customConditions.Select(condition => EventDataUtil.EventData.GetEventData(condition.dataName)).ToList();
            }
            eventDataList.AddRange(eventDataList_true_bool);
            eventDataList.AddRange(eventDataList_false_bool);
            if (eventDataList_custom != null)
            {
                eventDataList.AddRange(eventDataList_custom);
            }

            //收集所有数据的条件
            ConditionAction conditionAction = new ConditionAction();
            eventDataList_true_bool.ForEach(eventData => conditionAction.conditionList.Add(() => { return eventData.IsDataSame(true); }));
            eventDataList_false_bool.ForEach(eventData => conditionAction.conditionList.Add(() => { return eventData.IsDataSame(false); }));
            if (customConditions != null)
            {
                customConditions.ToList().ForEach(condition => conditionAction.conditionList.Add(condition.conditionCheck));
            }


            conditionAction.action = action;

            //将条件添加到所有数据的事件数据中
            eventDataList.ForEach(eventData =>
            {
                eventData.conditionActionList.Add(conditionAction);
            });

        }


        //方法：生成事件数据限制条件，当数据更新时执行
        public static void CreateDataCondition_Update<T>(Action<T> action, Enum dataName)
        {
            EventDataUtil.EventData<T> eventData = EventDataUtil.EventData.GetEventData<T>(dataName);

            eventData.onUpdatedAction.Add(() =>
            {
                action(eventData.GetData());
            });
        }

    }










}