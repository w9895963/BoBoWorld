using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventDataS
{
    namespace EventDataCore
    {





        // 全局事件数据存储字典
        public static class GlobalDataHolder
        {
            public static Dictionary<string, EventData> globalDict = new Dictionary<string, EventData>();
            //所有本地字典
            private static List<Dictionary<string, EventData>> localDicts = new List<Dictionary<string, EventData>>();


            public static void AddData<T>(EventData<T> globalDataT)
            {
                //*添加全局数据
                //是否已经是全局数据
                if (globalDict.Exist(globalDataT.Key, globalDataT))
                {
                    //报错
                    Debug.LogError($"当前数据{globalDataT.Key}已经是全局数据");
                    return;
                }

                //添加到全局字典
                globalDict.Add(globalDataT.Key, globalDataT);
                //*将所有本地字典同步到全局字典
                foreach (var localDic in localDicts)
                {
                    //如果本地字典中已经有了这个数据 //则合并数据
                    if (localDic.ContainsKey(globalDataT.Key))
                    {
                        localDic[globalDataT.Key] = globalDataT;
                    }
                }
                //设置全局数据完成, 执行一次数据更新
                globalDataT.ForceUpdateData();
            }
           
        }





    }
}
