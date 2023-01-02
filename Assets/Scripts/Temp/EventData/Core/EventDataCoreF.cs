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

            /// *<summary>获取事件数据,不存在则添加</summary>
            public static EventData GetEventData(string key, System.Type type = null, GameObject gameObject = null)
            {
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
                    if (type != null && eventData.Type != type)
                    {
                        Debug.LogError($"({eventData.Type}) EventDataCoreF.GetEventData: " + key + " is not " + type.Name);
                        return null;
                    }
                    else
                    {
                        return eventData;
                    }
                }
                else
                {

                    //新建
                    eventData = CreateEventData(key, type, gameObject);
                    DataHolder.Add(key, eventData, gameObject);
                    return eventData;

                }

            }


            /// <summary>新建事件数据</summary>
            private static EventData CreateEventData(string key, System.Type type = null, GameObject gameObject = null)
            {
                EventData eventData;

                //根据type来实例化 EventData
                if (type == null)
                {
                    //新建
                    eventData = new EventData(key, gameObject, type);
                }
                else
                {
                    //根据type来实例化 EventData<T>
                    switch (type.Name)
                    {
                        //如果是Int,则新建EventData<int>
                        case "Int32":
                            eventData = new EventData<int>(key, gameObject);
                            break;
                        //如果是Float,则新建EventData<float>
                        case "Single":
                            eventData = new EventData<float>(key, gameObject);
                            break;
                        //如果是String,则新建EventData<string>
                        case "String":
                            eventData = new EventData<string>(key, gameObject);
                            break;
                        //如果是Bool,则新建EventData<bool>
                        case "Boolean":
                            eventData = new EventData<bool>(key, gameObject);
                            break;
                        //如果是Vector2,则新建EventData<Vector2>
                        case "Vector2":
                            eventData = new EventData<Vector2>(key, gameObject);
                            break;
                        //如果是Vector3,则新建EventData<Vector3>
                        case "Vector3":
                            eventData = new EventData<Vector3>(key, gameObject);
                            break;
                        //如果是Vector4,则新建EventData<Vector4>
                        case "Vector4":
                            eventData = new EventData<Vector4>(key, gameObject);
                            break;
                        //如果是Color,则新建EventData<Color>
                        case "Color":
                            eventData = new EventData<Color>(key, gameObject);
                            break;
                        //如果是Color32,则新建EventData<Color32>
                        case "Color32":
                            eventData = new EventData<Color32>(key, gameObject);
                            break;
                        //如果是Quaternion,则新建EventData<Quaternion>
                        case "Quaternion":
                            eventData = new EventData<Quaternion>(key, gameObject);
                            break;
                        //如果是Rect,则新建EventData<Rect>
                        case "Rect":
                            eventData = new EventData<Rect>(key, gameObject);
                            break;
                        //如果是Matrix4x4,则新建EventData<Matrix4x4>
                        case "Matrix4x4":
                            eventData = new EventData<Matrix4x4>(key, gameObject);
                            break;
                        //如果是GameObject,则新建EventData<GameObject>
                        case "GameObject":
                            eventData = new EventData<GameObject>(key, gameObject);
                            break;
                        //如果是Transform,则新建EventData<Transform>
                        case "Transform":
                            eventData = new EventData<Transform>(key, gameObject);
                            break;
                        //如果是Component,则新建EventData<Component>
                        case "Component":
                            eventData = new EventData<Component>(key, gameObject);
                            break;
                        //如果是Object,则新建EventData<Object>
                        case "Object":
                            eventData = new EventData<System.Object>(key, gameObject);
                            break;
                        //如果是Texture,则新建EventData<Texture>
                        case "Texture":
                            eventData = new EventData<Texture>(key, gameObject);
                            break;
                        //其他
                        default:
                            eventData = null;
                            break;
                    }
                }
                //如果为null
                if (eventData == null)
                {
                    //用反射来新建EventData
                    Debug.Log($"({type}) EventDataCoreF.CreateEventData: " + key + " is not " + type.Name);
                    //获取EventData的构造函数
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(string), typeof(GameObject) });
                    //实例化
                    eventData = constructor.Invoke(new object[] { key, gameObject }) as EventData;
                }

                return eventData;
            }

            /// *<summary>获取事件数据,不存在则添加,使用类型参数</summary>
            public static EventData<T> GetEventData<T>(string key, GameObject gameObject = null)
            {
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
                        Debug.LogError($"({eventData.Type}) EventDataCoreF.GetEventData: " + key + " is not " + typeof(T));
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
