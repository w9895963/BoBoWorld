using System;
using System.Collections.Generic;
using EventDataS.Core;
using UnityEngine;

namespace EventDataS
{
    //*公用方法
    public static class EventDataF
    {




        ///* <summary>获得数据，根据输入判断全局与否</summary>
        public static EventDataHandler<T> GetData<T>(string dataName, GameObject gameObject = null)
        {
            EventData<T> eventData = EventDataCoreF.GetEventData(dataName, typeof(T), gameObject) as EventData<T>;
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventData);
            return dataOperator;
        }
        /// <summary>获得数据，根据输入判断全局与否，枚举版本</summary>
        public static EventDataHandler<T> GetData<T>(System.Enum dataName, GameObject gameObject = null)
        {
            EventData<T> eventData = EventDataCoreF.GetEventData(dataName.ToString(), typeof(T), gameObject) as EventData<T>;
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventData);
            return dataOperator;
        }










        //方法：设置数据，物体数据，字符串版本
        public static void SetData_local<T>(GameObject gameObject, string name, T data)
        {
            EventData<T> eventDataT = EventDataCoreF.GetEventData(name, typeof(T), gameObject) as EventData<T>;
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








        /// *<summary> 创建数据条件,返回启用器 </summary>
        private static (Action Enable, Action Disable) OnDataConditionCore(Action action, Action actionOnFail, (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            Debug.Log("创建条件");
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
        public static (Action Enable, Action Disable) OnDataCondition(Action action, Action actionOnFail, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            return OnDataConditionCore(action, actionOnFail, conditionChecks);
        }

        public static void OnDataCondition(Action action, Action actionOnFail, ref (Action Enable, Action Disable) enabler, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, actionOnFail, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }

        public static void OnDataCondition(Action action, Action actionOnFail, ref (Action Enable, Action Disable) enabler, List<(Core.EventData data, Func<bool> check)> conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, actionOnFail, conditionChecks.ToArray());
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        //单参数版本
        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, null, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        public static void OnDataCondition(Action action, ref (Action Enable, Action Disable) enabler, List<(Core.EventData data, Func<bool> check)> conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = OnDataConditionCore(action, null, conditionChecks.ToArray());
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }




        /// <summary> 创建数据条件,返回启用器 </summary>
        public static (Action Enable, Action Disable) GetLocalDict(Action action, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            return OnDataConditionCore(action, null, conditionChecks);
        }


    }

}