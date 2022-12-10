using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventData
{

    //可视化
    namespace EventDataVisualizeGroup
    {
        //属性：只在编辑器下可见
        [ExecuteInEditMode]
        //类型： 事件数据可视化
        public class EventDataVisualizer : MonoBehaviour
        {
            //字段：全局数据条目列表
            public List<DataItem> GlobalData = new List<DataItem>();
            //字段：本地数据条目列表
            public List<DataItem> ObjectData = new List<DataItem>();

            private void Awake()
            {
                TimerF.WaitUpdate(() =>
                {
                    UpdateData();
                });
            }


            //添加上下文菜单
            [ContextMenu("更新数据")]
            //方法：更新数据
            public void UpdateData()
            {
                //获得事件数据存储字典
                Dictionary<System.Enum, EventDataUtil.EventData> eventDataDict = EventDataUtil.GlobalData.holderDict;
                Dictionary<System.Enum, EventDataUtil.EventData> eventDataDict_this = null;
                //如果组件存在
                if (gameObject.GetComponent<EventDataUtil.EventDataMono>() != null)
                {
                    //获取本物体上的字典
                    eventDataDict_this = gameObject.GetComponent<EventDataUtil.EventDataMono>().dateHolderDict;
                }


                AddData(GlobalData, eventDataDict);
                AddData(ObjectData, eventDataDict_this);

                AddDataAutoUpdateEvent(GlobalData);
                AddDataAutoUpdateEvent(ObjectData);

            }


            //方法：往数据列表中添加数据
            private void AddData(List<DataItem> ObjectData, Dictionary<System.Enum, EventDataUtil.EventData> eventDataDict)
            {
                //空则退出
                if (eventDataDict == null || eventDataDict.Count == 0)
                {
                    return;
                }
                //将所有值和索引转换成列表
                List<KeyValuePair<System.Enum, EventDataUtil.EventData>> eventDataList = eventDataDict.ToList();
                //排序
                eventDataList.Sort((a, b) => { return a.Key.GetType().FullName.CompareTo(b.Key.GetType().FullName); });
                //转化成DataItem列表
                List<DataItem> ObjectDataListAll = eventDataList.Select(eventData => new DataItem()
                {
                    全名 = eventData.Key.GetType().FullName + "." + eventData.Key.ToString(),
                    数据 = eventData.Value.GetData().ToString(),
                    dataName = eventData.Key.ToString(),
                    //字符串插值将名字与值加起来
                    name = $"{eventData.Key.ToString()}:{eventData.Value.GetData().ToString()}",
                    eventData = eventData.Value,
                }).ToList();
                //往ObjectData中添加不重复的数据
                ObjectData.AddRange(ObjectDataListAll.Where(dataItem => !ObjectData.Contains(dataItem)));
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
                    dataItem.eventData.onUpdatedAction.Add(() =>
                    {
                        //更新值
                        dataItem.数据 = dataItem.eventData.GetData().ToString();
                        //更新名字
                        dataItem.name = $"{dataItem.dataName}:{dataItem.数据}";

                    });
                }

            }




        }


        //类：数据条目
        [System.Serializable]
        public class DataItem
        {
            [HideInInspector]
            public string name;

            public string 全名;
            public string 数据;

            public EventDataUtil.EventData eventData;
            //隐藏显示
            [HideInInspector]
            public string dataName;
            //是否已经添加事件
            [HideInInspector]
            public bool isAddedEvent = false;
        }

    }



}