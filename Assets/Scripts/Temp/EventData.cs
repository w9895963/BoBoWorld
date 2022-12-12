using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventDataS.EventDataUtil;
using UnityEngine;

namespace EventDataS
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

            public EventData() : base()
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


                    var isConditionMet = conditionAction.conditionList.All(condition => condition());



                    if (isConditionMet)
                    {

                        ActionF.QueueAction(conditionAction.action);
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



        /// <summary>  当任意一个数据更新时执行action ，返回启用和禁用方法 </summary>
        public static (Action Enable, Action Disable) OnDataUpdate(Action action, params EventDataHandler[] datas)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.action = action;

            Action enable = () =>
            {
                datas.ForEach(data =>
                {
                    data.eventData.conditionActionList.AddNotHas(conditionAction);
                });
            };
            Action disable = () =>
            {
                datas.ForEach(data =>
                {
                    data.eventData.conditionActionList.Remove(conditionAction);
                });
            };
            return (enable, disable);
        }
        public static void OnDataUpdate(Action action, ref (Action Enable, Action Disable) enabler, params EventDataHandler[] datas)
        {
            (Action Enable, Action Disable) enableAction = OnDataUpdate(action, datas);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }




        ///<summary> 创建数据条件,返回启用器</summary>
        public static (Action Enable, Action Disable) OnDataCondition(Action action, params (EventDataUtil.EventData data, Func<bool> check)[] conditionChecks)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.action = action;
            conditionChecks.ForEach(conditionCheck =>
            {
                //如果check存在则添加到conditionList
                if (conditionCheck.check != null)
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

        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, params (EventDataUtil.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataCondition(action, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        
        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, List<(EventDataUtil.EventData data, Func<bool> check)> conditionChecks)
        {
            OnDataCondition(action, ref enabler, conditionChecks.ToArray());
        }









    }



    public class EventDataHandler
    {
        public EventData eventData;

    }







    //类型:数据操作器
    public class EventDataHandler<T> : EventDataHandler
    {
        //字段：事件数据
        private EventData<T> eventDataT;

        public EventDataHandler(EventData<T> eventDataT)
        {
            this.eventDataT = eventDataT;
            this.eventData = eventDataT;
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
            conditionAction.action = () => { act(Data); };
            Action enable = () => eventDataT.conditionActionList.Add(conditionAction);
            Action disable = () => { eventDataT.conditionActionList.Remove(conditionAction); };

            return (enable, disable);
        }
        public void SetDataTo(Action<T> act, ref (Action Enable, Action Disable) enabler)
        {
            (Action Enable, Action Disable) enableAction = SetDataTo(act);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }


        //属性：获得数据判断方法，数据更新
        public (EventDataUtil.EventData data, Func<bool> check) OnUpdate => (eventDataT, null);

        //属性：获得数据判断方法，数据为真
        public (EventDataUtil.EventData data, Func<bool> check) OnTrue => (eventDataT, () => { return Data.Equals(true); }
        );
        //属性：获得数据判断方法，数据为假
        public (EventDataUtil.EventData data, Func<bool> check) OnFalse => (eventDataT, () => { return Data.Equals(false); }
        );
        //方法：获得数据判断方法，自定义判断
        public (EventDataUtil.EventData data, Func<bool> check) OnCustom(Func<bool> check)
        {
            return (eventDataT, check);
        }

    }





}