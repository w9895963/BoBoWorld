using System;
using System.Collections.Generic;
using System.Linq;
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
                if (Enum.IsDefined(typeof(DataName.Preset.PresetName), key))
                {
                    System.Type type = PresetNameF.GetDataType(key);
                    if (type != typeof(T))
                    {
                        Debug.LogError($"数据名[{key}]对应的类型为[{type}], 但是你使用的类型为[{typeof(T)}]");
                        return null;
                    }
                }







                //~获取数据
                EventData eventData;
                bool IsGlobal = PresetNameF.IsGlobal(key);
                //如果是本地数据
                if (!IsGlobal)
                {
                    eventData = DataHolder.GetEventData(key, gameObject);
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
                        Debug.LogError($" 数据 [{key}]存在, 但已被声明为类型 [{eventData.Type}], 和希望返回类型 [{typeof(T)}] 相冲突");
                        return null;
                    }
                }
                else
                {

                    //新建
                    eventData = new EventData<T>(key, gameObject);
                    //根据是否是全局数据添加到对应的数据表中
                    if (!IsGlobal)
                    {
                        DataHolder.Add(key, eventData, gameObject);
                    }
                    else
                    {
                        DataHolder.Add(key, eventData);
                    }

                }

                //~根据全局与否将数据添加到对应的本地数据表中
                if (IsGlobal)
                {
                    if (!DataHolder.ContainsKey(key, gameObject))
                    {
                        DataHolder.Add(key, eventData, gameObject);
                    }

                }







                return eventData as EventData<T>;
            }
            /// *<summary>获取事件数据,不存在则添加,不使用类型参数</summary>
            public static EventData GetEventData(string key, GameObject gameObject = null)
            {
                //~获取数据
                EventData eventData = null;
                bool IsGlobal = PresetNameF.IsGlobal(key);
                //如果是本地数据
                if (!IsGlobal)
                {
                    //获取本地数据
                    eventData = DataHolder.GetEventData(key, gameObject);
                }
                else
                {
                    //获取全局数据
                    eventData = DataHolder.GetEventData(key);
                }


                //~存在则返回,不存在则新建
                //如果存在
                if (eventData == null)
                {
                    //获得类型,没有则报错
                    System.Type type = PresetNameF.GetDataType(key);
                    if (type != null)
                    {
                        //新建
                        var classType = typeof(EventData<>).MakeGenericType(type);
                        eventData = Activator.CreateInstance(classType, key, gameObject) as EventData;
                        //根据是否是全局数据添加到对应的数据表中
                        if (eventData != null)
                        {
                            if (!IsGlobal)
                            {
                                DataHolder.Add(key, eventData, gameObject);
                            }
                            else
                            {
                                DataHolder.Add(key, eventData);
                            }
                        }
                        else
                        {
                            Debug.LogError($"数据名[{key}]对应的类型[{type}]无法创建");
                        }

                    }
                    else
                    {
                        Debug.LogError($"数据名[{key}]不存在");
                    }

                }


                //~根据全局与否将数据添加到对应的本地数据表中
                if (IsGlobal)
                {
                    if (!DataHolder.ContainsKey(key, gameObject))
                    {
                        DataHolder.Add(key, eventData, gameObject);
                    }

                }
                return eventData;
            }

            // /// *<summary>尝试根据数据名设置数据值</summary>
            // public static bool TrySetData(string key, object value, GameObject gameObject = null)
            // {
            //     bool re = false;
            //     //~获取数据
            //     EventData eventData;
            //     bool IsGlobal = DataNameF.IsGlobal(key);
            //     //如果是本地数据
            //     if (!IsGlobal)
            //     {
            //         eventData = DataHolder.GetEventData(key, gameObject);
            //     }
            //     else
            //     {
            //         //获取全局数据
            //         eventData = DataHolder.GetEventData(key);
            //     }



            //     //~存在则返回,不存在则新建
            //     //如果存在
            //     if (eventData != null)
            //     {
            //         //如果类型不同
            //         if (eventData.Type != typeof(T))
            //         {
            //             Debug.LogError($" 数据 [{key}]存在, 但已被声明为类型 [{eventData.Type}], 和希望返回类型 [{typeof(T)}] 相冲突");
            //             return null;
            //         }
            //     }
            //     else
            //     {

            //         //新建
            //         eventData = new EventData<T>(key, gameObject);
            //         //根据是否是全局数据添加到对应的数据表中
            //         if (!IsGlobal)
            //         {
            //             DataHolder.Add(key, eventData, gameObject);
            //         }
            //         else
            //         {
            //             DataHolder.Add(key, eventData);
            //         }

            //     }



            //     return re;
            // }







            /// *<summary> 创建数据条件,返回启用器 </summary>
            /// *<param name="monoBehaviour">当组件是否启用加入条件中</param>
            public static (Action Enable, Action Disable) CreateOnDataConditionCoreEnabler(Action action, Action actionOnFail, (Core.EventData data, Func<bool> check)[] conditionChecks,
            MonoBehaviour monoBehaviour = null)
            {
                //创建条件实例
                ConditionAction conditionAction = new ConditionAction();
                conditionAction.action = action;
                conditionAction.actionOnFail = actionOnFail;

                //如果组件存在则添加到条件表中
                if (monoBehaviour != null)
                {
                    conditionAction.conditionList.Add(() => monoBehaviour.enabled);
                }

                //如果组件存在则添加到conditionList
                conditionChecks.ForEach(conditionCheck =>
                {
                    //如果check存在则添加到conditionList
                    conditionAction.conditionList.AddNotNull(conditionCheck.check);
                });

                //收集所有数据
                IEnumerable<EventData> datas = conditionChecks.Select(conditionCheck => conditionCheck.data).Where(data => data != null).Distinct();

                Action enable = () =>
                {
                    datas.ForEach(data =>
                    {
                        data.conditionActionList.AddNotHas(conditionAction);
                    });
                };
                Action disable = () =>
                {
                    datas.ForEach(data =>
                    {
                        data.conditionActionList.RemoveAll(conditionAction);
                    });
                };
                return (enable, disable);
            }









        }





    }


}
