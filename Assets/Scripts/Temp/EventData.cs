using System;
using System.Collections.Generic;
using EventDataS.EventDataCore;
using UnityEngine;

namespace EventDataS
{
    //*公用方法需要的类:EventDataHandler
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
        public (EventDataCore.EventData data, Func<bool> check) OnUpdate => (eventDataT, null);

        //属性：获得数据判断方法，数据为真
        public (EventDataCore.EventData data, Func<bool> check) OnTrue => (eventDataT, () => { return Data.Equals(true); }
        );
        //属性：获得数据判断方法，数据为假
        public (EventDataCore.EventData data, Func<bool> check) OnFalse => (eventDataT, () => { return Data.Equals(false); }
        );
        //方法：获得数据判断方法，自定义判断
        public (EventDataCore.EventData data, Func<bool> check) OnCustom(Func<bool> check)
        {
            return (eventDataT, check);
        }

    }







    //*公用方法
    public static class EventDataF
    {
        //*静态方法：新建数据

        private static EventData<T> CreateDataCore<T>(GameObject gameObject, string key, bool isGlobal = false)
        {
            EventData<T> eventDataT = new EventData<T>(key);
            EventDataStoreMono.GetLocalDict(gameObject).Add(key, eventDataT);
            if (isGlobal)
            {
                GlobalData.AddData(eventDataT);
            }
            return eventDataT;
        }

        /// <summary>新建全局数据，枚举版本</summary>
        public static EventDataHandler<T> CreateGlobalData<T>(GameObject gameObject, System.Enum key)
        {
            return new EventDataHandler<T>(CreateDataCore<T>(gameObject, key.ToString(), true));
        }





        //*静态方法：获取带参数的事件数据
        private static EventData<T> GetEventData_Core<T>(string key, GameObject gameObject = null)
        {
            EventData<T> eventDataT;
            //本地
            Dictionary<string, EventData> loDict = EventDataStoreMono.GetLocalDict(gameObject);

            //新建或获取本地数据
            eventDataT = loDict.GetOrCreate(key, new EventData<T>(key)).ToEventData<T>();

            return eventDataT;
        }


        /// <summary>获得数据，先局部后全局数据，字符串版本</summary>
        public static EventDataHandler<T> GetData<T>(GameObject gameObject, string dataName)
        {
            EventData<T> eventData = GetEventData_Core<T>(dataName, gameObject);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventData);
            return dataOperator;
        }
        /// <summary>获得数据，先局部后全局数据，枚举版本</summary>
        public static EventDataHandler<T> GetData<T>(GameObject gameObject, System.Enum dataName)
        {
            return GetData<T>(gameObject, dataName.GetFullName());
        }








        //方法：设置数据，物体数据，字符串版本
        public static void SetData_local<T>(GameObject gameObject, string name, T data)
        {
            EventData<T> eventDataT = GetEventData_Core<T>(name, gameObject);
            eventDataT.SetData(data);
        }
        //方法：设置数据，物体数据
        public static void SetData_local<T>(GameObject gameObject, System.Enum name, T data)
        => SetData_local<T>(gameObject, name.GetFullName(), data);



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




        /// <summary> 创建数据条件,返回启用器 </summary>
        private static (Action Enable, Action Disable) OnDataConditionCore(Action action, Action actionOnFail, (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.action = action;
            conditionAction.actionOnFail = actionOnFail;
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

        ///<summary> 创建数据条件,返回启用器</summary>
        public static (Action Enable, Action Disable) OnDataCondition(Action action, Action actionOnFail, params (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
        {
            return OnDataConditionCore(action, actionOnFail, conditionChecks);
        }

        public static void OnDataCondition(Action action, Action actionOnFail, ref (Action Enable, Action Disable) enabler, params (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, actionOnFail, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }

        public static void OnDataCondition(Action action, Action actionOnFail, ref (Action Enable, Action Disable) enabler, List<(EventDataCore.EventData data, Func<bool> check)> conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, actionOnFail, conditionChecks.ToArray());
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        //单参数版本
        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, params (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, null, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, List<(EventDataCore.EventData data, Func<bool> check)> conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, null, conditionChecks.ToArray());
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }


    }

}