using System;
using System.Collections.Generic;
using EventData.Core;
using UnityEngine;
using static EventData.Core.EventDataCoreF;

namespace EventData
{
    //*公用方法
    public static class EventDataF
    {




        ///* <summary>获得数据，根据输入判断全局与否</summary>
        public static EventDataHandler<T> GetData<T>(string dataName, GameObject gameObject = null)
        {
            EventData<T> eventData = EventDataCoreF.GetEventData<T>(dataName, gameObject);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventData);
            return dataOperator;
        }
        /// <summary>获得数据，根据输入判断全局与否，枚举版本</summary>
        public static EventDataHandler<T> GetData<T>(System.Enum dataName, GameObject gameObject = null)
        {
            EventData<T> eventData = EventDataCoreF.GetEventData<T>(dataName.ToString(), gameObject);
            EventDataHandler<T> dataOperator = new EventDataHandler<T>(eventData);
            return dataOperator;
        }
















        ///<summary> 创建数据条件,返回启用器</summary>
        public static (Action Enable, Action Disable) OnDataCondition(Action action, Action actionOnFail, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            return CreateOnDataConditionCoreEnabler(action, actionOnFail, conditionChecks);
        }
        ///<summary> 创建数据条件,设置启用器</summary>
        public static void OnDataCondition(Action action, Action actionOnFail, ref (Action Enable, Action Disable) enabler, params (Core.EventData data, Func<bool> check)[] conditionChecks)
        {
            (Action Enable, Action Disable) enableAction = CreateOnDataConditionCoreEnabler(action, actionOnFail, conditionChecks);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }
        ///<summary> 创建数据条件,返回启用器</summary>
        ///<param name="monoBehaviour">当组件是否启用加入条件中</param>
        public static (Action Enable, Action Disable) OnDataCondition(Action action, (Core.EventData data, Func<bool> check)[] conditionChecks, Action actionOnFail = null, MonoBehaviour monoBehaviour = null)
        {
            return CreateOnDataConditionCoreEnabler(action, actionOnFail, conditionChecks, monoBehaviour);
        }








    }

}