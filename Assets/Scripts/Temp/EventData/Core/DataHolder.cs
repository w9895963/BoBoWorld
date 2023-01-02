using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EventDataS
{
    namespace Core
    {
        public static class DataHolder
        {

            private static Dictionary<string, EventData> globalDict = new Dictionary<string, EventData>();
            //所有本地字典
            private static Dictionary<GameObject, Dictionary<string, EventData>> localDicts = new Dictionary<GameObject, Dictionary<string, EventData>>();




            /// *<summary>获取数据</summary>
            public static EventData GetEventData(string dataName, GameObject gameObject = null)
            {
                //~判断是否全局数据
                if (gameObject == null)
                {
                    globalDict.TryGetValue(dataName, out EventData eventData);
                    return eventData;
                }
                else
                {
                    localDicts.GetOrCreate(gameObject).TryGetValue(dataName, out EventData eventData);
                    return eventData;

                }
            }
            /// *<summary>数据是否存在</summary>
            public static bool HasData(string dataName, GameObject gameObject = null)
            {
                //~判断是否全局数据
                if (gameObject == null)
                {
                    return globalDict.ContainsKey(dataName);
                }
                else
                {
                    return localDicts.GetOrCreate(gameObject).ContainsKey(dataName);
                }
            }

            ///*<summary>添加数据,覆盖,返回被覆盖的,如果是全局数据则替换所有本地数据</summary>
            public static EventData AddAndReplaceData(string dataName, EventData eventData, GameObject gameObject = null)
            {
                EventData replaced;
                //~判断是否全局数据,各自添加
                if (gameObject == null)
                {
                    //是否替换成功
                    globalDict.AddAndReplace(dataName, eventData, out replaced);
                }
                else
                {
                    localDicts.GetOrCreate(gameObject).AddAndReplace(dataName, eventData, out replaced);
                }

                return replaced;
            }


            ///*<summary>获取本地字典</summary>
            public static Dictionary<string, EventData> GetLocalDict(GameObject gameObject)
            {
                Dictionary<string, EventData> dictionary = localDicts.TryGetValue(gameObject);
                return dictionary;
            }
            ///*<summary>获取全局字典</summary>
            public static Dictionary<string, EventData> GetGlobalDict()
            {
                return globalDict;
            }



        }





    }
}
