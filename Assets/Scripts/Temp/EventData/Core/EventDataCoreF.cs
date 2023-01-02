using System;
using System.Collections.Generic;
using System.Reflection;
using EventDataS.Core;
using UnityEngine;

namespace EventDataS
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
                EventData eventData = DataHolder.GetEventData(key, gameObject);
                //~存在,且类型相同,则返回
                if (eventData != null && (type == null || eventData.GetType() == type))
                    return eventData;

                //~新建并添加
                eventData = CreateEventData(key, type, gameObject);
                EventData replacedData = DataHolder.AddAndReplaceData(key, eventData, gameObject);
                //~替换老数据的事件数据
                //~将所有替换数据
                if (replacedData != null)
                {
                    eventData.conditionActionList = replacedData.conditionActionList;
                    ReplaceEventDataInHandle(eventData, replacedData);
                }










                return eventData;
            }

            /// *<summary>新建事件数据</summary>
            public static EventData CreateEventData(string key, System.Type type = null, GameObject gameObject = null)
            {
                EventData eventData;

                //根据type来实例化 EventData
                if (type == null)
                {
                    //新建
                    eventData = new EventData(key, gameObject);
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
                    //获取EventData的构造函数
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(string), typeof(GameObject) });
                    //实例化
                    eventData = constructor.Invoke(new object[] { key, gameObject }) as EventData;
                }

                return eventData;
            }

            /// *<summary>字典:事件数据:事件数据处理器</summary>
            private static Dictionary<EventData, List<EventDataHandler>> EventData_HandlerDic = new Dictionary<EventData, List<EventDataHandler>>();

            /// *<summary>将EventDataHandler的引用更新,添加到字典</summary>
            public static void ReplaceEventDataInHandle(EventData eventDataTo, EventData eventDataFrom)
            {
                //如果字典中包含该事件数据
                if (EventData_HandlerDic.ContainsKey(eventDataFrom))
                {
                    //获取该事件数据的处理器
                    List<EventDataHandler> handlers = EventData_HandlerDic[eventDataFrom];
                    //遍历处理器
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        //将EventDataHandler的引用更新
                        handlers[i].eventData = eventDataTo;
                    }
                    //添加到字典
                    EventData_HandlerDic.Add(eventDataTo, handlers);
                    //移除
                    EventData_HandlerDic.Remove(eventDataFrom);
                }
            }




        }
    }


}
