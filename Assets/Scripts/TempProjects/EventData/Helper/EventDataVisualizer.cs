using System.Collections.Generic;
using System.Linq;
using EventData.Core;
using UnityEngine;

namespace EventData
{
    //只在编辑器下运行


    //可视化
    namespace EventDataVisualizeGroup
    {
        //添加组件名
        [AddComponentMenu("辅助工具/事件数据可视化")]

        //*类型： 事件数据可视化
        public class EventDataVisualizer : MonoBehaviour
        {
            //字段：全局数据条目列表
            public List<DataItem> GlobalData = new List<DataItem>();
            //字段：本地数据条目列表
            public List<DataItem> ObjectData = new List<DataItem>();











            //*方法：更新数据
            
            public void UpdateData()
            {
                GlobalData.Clear();
                ObjectData.Clear();

                //获得事件数据存储字典
                List<KeyValuePair<string, Core.EventData>> eventDataGo = Core.DataHolder.GetGlobalDict().ToList();
                List<KeyValuePair<string, Core.EventData>> eventDataLo = Core.DataHolder.GetLocalDict(gameObject)?.ToList();
                //如果组件存在


                AddData(GlobalData, eventDataGo);
                AddData(ObjectData, eventDataLo);

                AddDataAutoUpdateEvent(GlobalData);
                AddDataAutoUpdateEvent(ObjectData);
                

                //~排序
                var list = DataNameF.GetDataNamesList().ToList();
                ObjectData.SortBy((a) => list.IndexOf(a.eventData.Key));
                GlobalData.SortBy((a) => list.IndexOf(a.eventData.Key));

            }




            public void DebugLog()
            {
                //打印内容
                string log = "";
                //选择所有打印的数据
                List<DataItem> dataItemList = GlobalData.Where(dataItem => dataItem.打印).ToList();
                //如果有
                if (dataItemList.Count > 0)
                {
                    //添加标题
                    log += $"全局数据{dataItemList.Count}个:";
                    //历遍数据条目
                    foreach (DataItem dataItem in dataItemList)
                    {
                        //打印数据
                        log += "\n";
                        log += dataItem.GetDebugLog();
                    }
                }
                //选择所有打印的本地数据
                List<DataItem> dataItemList_ObjectData = ObjectData.Where(dataItem => dataItem.打印).ToList();
                //如果有
                if (dataItemList_ObjectData.Count > 0)
                {
                    //如果上面有数据
                    if (log != "")
                    {
                        log += "\n";
                    }
                    //添加标题
                    log += $"本地数据{dataItemList_ObjectData.Count}个:";
                    //历遍数据条目
                    foreach (DataItem dataItem in dataItemList_ObjectData)
                    {
                        //打印数据
                        log += "\n";
                        log += dataItem.GetDebugLog();
                    }
                }
                //如果没有数据
                if (log == "")
                {
                    log = "没有勾选数据";
                }
                Debug.LogFormat(log);
            }

            //Unity事件:变动事件
            public void OnValidate()
            {

            }




            //开始
            private void Awake()
            {
                TimerF.WaitNextFrameUpdate(UpdateData);
            }






            //方法：往数据列表中添加数据
            private void AddData(List<DataItem> ObjectData, List<KeyValuePair<string, Core.EventData>> eventDataDict)
            {
                //空则退出
                if (eventDataDict == null || eventDataDict.Count == 0)
                {
                    return;
                }
                //将所有值和索引转换成列表
                List<KeyValuePair<string, Core.EventData>> eventDataList = eventDataDict;
                //排序
                eventDataList.Sort((a, b) => { return a.Key.GetType().FullName.CompareTo(b.Key.GetType().FullName); });
                //转化成DataItem列表
                List<DataItem> ObjectDataListAll = eventDataList.Select(eventData => new DataItem(eventData.Value)).ToList();


                //*往ObjectData中添加不重复的数据
                // 获得ObjectData中所有eventData的列表
                List<Core.EventData> eventDataList_ObjectData = ObjectData.Select(dataItem => dataItem.eventData).ToList();
                ObjectData.AddRange(ObjectDataListAll.Where(dataItem => !eventDataList_ObjectData.Contains(dataItem.eventData)));
            }

            //方法：添加自动更新事件
            private void AddDataAutoUpdateEvent(List<DataItem> ObjectData)
            {
                //历遍
                foreach (DataItem dataItem in ObjectData)
                {
                    //如果已经添加事件则跳过
                    if (dataItem.isAddedEvent)
                    {
                        continue;
                    }
                    //标记已经添加事件
                    dataItem.isAddedEvent = true;


                    //添加事件
                    (Core.EventData data, System.Func<bool> check)[] checks = { (dataItem.eventData, null) };
                    System.Action action = () =>
                    {
                        dataItem.数据 = dataItem.eventData.GetData()?.ToString();
                        dataItem.labelName = $"{dataItem.shortName}:{dataItem.数据}";
                    };
                    EventDataCoreF.CreateOnDataConditionCoreEnabler(action, null, checks, this).Enable();


                }

            }
        }

    }



}