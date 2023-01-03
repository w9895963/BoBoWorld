using System;
using System.Collections.Generic;
using System.Reflection;
using EventData.Core;
using UnityEngine;

namespace EventData
{
    namespace Core
    {
        /// *<summary>静态类：事件数据核心</summary>
        public static class EventDataCoreF
        {




            /// *<summary>获取事件数据,不存在则添加,使用类型参数</summary>
            public static EventData<T> GetEventData<T>(string key, GameObject gameObject = null)
            {
                //~判断数据类型是否符合预设,如果不符合则返回null
                //判断key是否存在DataName中
                if (Enum.IsDefined(typeof(DataName), key))
                {
                    System.Type type = DataNameAddition.DataNameType.Get((DataName)Enum.Parse(typeof(DataName), key));
                    if (type != typeof(T))
                    {
                        Debug.LogError($"数据名[{key}]对应的类型为[{type}], 但是你使用的类型为[{typeof(T)}]");
                        return null;
                    }
                }





                //~获取数据
                EventData eventData;
                //如果是本地数据
                if (gameObject != null)
                {
                    //获取全局数据
                    eventData = DataHolder.GetEventData(key);
                    //如果不存在则获取本地数据
                    if (eventData == null)
                    {
                        eventData = DataHolder.GetEventData(key, gameObject);
                    }
                }
                else
                {
                    //获取全局数据
                    eventData = DataHolder.GetEventData(key);
                }


                //~存在则返回,不存在则新建
                //如果存在
                if (eventData != null)
                {
                    //如果类型不同
                    if (eventData.Type != typeof(T))
                    {
                        Debug.LogError($" 数据 [{key}] 已被声明为类型 [{eventData.Type}], 和希望返回类型 [{typeof(T)}] 相冲突");
                        return null;
                    }
                    else
                    {
                        return eventData as EventData<T>;
                    }
                }
                else
                {

                    //新建
                    eventData = new EventData<T>(key, gameObject);
                    DataHolder.Add(key, eventData, gameObject);
                    return eventData as EventData<T>;

                }
            }







            /// *<summary> 创建数据条件,返回启用器 </summary>
            public static (Action Enable, Action Disable) OnDataConditionCore(Action action, Action actionOnFail, (Core.EventData data, Func<bool> check)[] conditionChecks)
            {
                ConditionAction conditionAction = new ConditionAction();
                conditionAction.action = action;
                conditionAction.actionOnFail = actionOnFail;
                conditionChecks.ForEach(conditionCheck =>
                {
                    //如果check存在则添加到conditionList
                    conditionAction.conditionList.AddNotNull(conditionCheck.check);
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



        }





    }


}
