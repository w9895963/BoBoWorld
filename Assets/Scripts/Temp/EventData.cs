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

        //方法：获取数据操作器，全局数据,字符串版本
        public static EventDataHandler<T> GetData_global<T>(string name)
        {
            EventData<T> eventDataT = EventDataCore.EventData.GetEventData<T>(name);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventDataT);
            return dataOperator;
        }
        //方法：获取数据操作器，局部数据,字符串版本
        public static EventDataHandler<T> GetData_local<T>(GameObject gameObject, string name)
        {
            EventData<T> eventDataT = EventDataCore.EventData.GetEventData<T>(name, gameObject);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventDataT);
            return dataOperator;
        }



        //方法：获取数据操作器，全局数据
        public static EventDataHandler<T> GetData_global<T>(System.Enum name)
        => GetData_global<T>(name.GetFullName());
        //方法：获取数据操作器，局部数据
        public static EventDataHandler<T> GetData_local<T>(GameObject gameObject, System.Enum name)
        => GetData_local<T>(gameObject, name.GetFullName());




        //方法：设置数据，物体数据，字符串版本
        public static void SetData_local<T>(GameObject gameObject, string name, T data)
        {
            EventData<T> eventDataT = EventDataCore.EventData.GetEventData<T>(name, gameObject);
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




        ///<summary> 创建数据条件,返回启用器</summary>
        public static (Action Enable, Action Disable) OnDataCondition(Action action, params (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
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

        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, params (EventDataCore.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataCondition(action, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }

        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, List<(EventDataCore.EventData data, Func<bool> check)> conditionChecks)
        {
            OnDataCondition(action, ref enabler, conditionChecks.ToArray());
        }


    }

}