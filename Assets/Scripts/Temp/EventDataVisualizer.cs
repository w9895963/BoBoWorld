using System.Collections.Generic;
using System.Linq;
using EventDataS.EventDataCore;
using NaughtyAttributes;
using UnityEngine;

namespace EventDataS
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
#if UNITY_EDITOR
            //
            [Label("全局数据")]

            //字段：全局数据条目列表
            public List<DataItem> GlobalData = new List<DataItem>();
            //字段：本地数据条目列表
            [Label("本地数据")]
            public List<DataItem> ObjectData = new List<DataItem>();


            //开始
            private void Awake()
            {
                TimerF.WaitUpdate(UpdateData);
            }



            //添加上下文菜单
            [ContextMenu("更新数据")]
            //*方法：更新数据
            public void UpdateData()
            {
                //获得事件数据存储字典
                Dictionary<string, EventDataCore.EventData> eventDataDict = EventDataCore.GlobalData.holderDictStr;
                Dictionary<string, EventDataCore.EventData> eventDataDict_this = null;
                //如果组件存在
                if (gameObject.GetComponent<EventDataCore.EventDataStoreMono>() != null)
                {
                    //获取本物体上的字典
                    eventDataDict_this = gameObject.GetComponent<EventDataCore.EventDataStoreMono>().dateHolderDictStr;
                }
                

                AddData(GlobalData, eventDataDict);
                AddData(ObjectData, eventDataDict_this);

                AddDataAutoUpdateEvent(GlobalData);
                AddDataAutoUpdateEvent(ObjectData);

            }


            //方法：往数据列表中添加数据

            private void AddData(List<DataItem> ObjectData, Dictionary<string, EventDataCore.EventData> eventDataDict)
            {
                //空则退出
                if (eventDataDict == null || eventDataDict.Count == 0)
                {
                    return;
                }
                //将所有值和索引转换成列表
                List<KeyValuePair<string, EventDataCore.EventData>> eventDataList = eventDataDict.ToList();
                //排序
                eventDataList.Sort((a, b) => { return a.Key.GetType().FullName.CompareTo(b.Key.GetType().FullName); });
                //转化成DataItem列表
                List<DataItem> ObjectDataListAll = eventDataList.Select(eventData => new DataItem(eventData.Value)).ToList();


                //*往ObjectData中添加不重复的数据
                // 获得ObjectData中所有eventData的列表
                List<EventDataCore.EventData> eventDataList_ObjectData = ObjectData.Select(dataItem => dataItem.eventData).ToList();
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
                    EventDataCore.ConditionAction conditionAction = new EventDataCore.ConditionAction();
                    conditionAction.conditionList.Add(() => true);
                    conditionAction.action = () =>
                    {
                        dataItem.数据 = dataItem.eventData.GetData().ToString();
                        dataItem.name = $"{dataItem.shortName}:{dataItem.数据}";
                    };
                    dataItem.eventData.conditionActionList.Add(conditionAction);

                }

            }



            [Button("打印数据")]
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


#endif
        }


        //*类：数据条目
        [System.Serializable]
        public class DataItem
        {
            [HideInInspector]
            public string name;

            public string 全名;
            public string 数据;
            public bool 打印 = false;

            public EventDataCore.EventData eventData;
            //隐藏显示
            [HideInInspector]
            public string shortName;
            //是否已经添加事件
            [HideInInspector]
            public bool isAddedEvent = false;


            //构造函数
            public DataItem(EventDataCore.EventData eventData)
            {
                this.eventData = eventData;
                全名 = eventData.Key;
                数据 = ExtractData();
                //名字=名字用"."分割的最后一个
                shortName = eventData.GetShortName();
                //字符串插值将名字与值加起来
                name = $"{shortName}:{数据}";
            }
            //方法：抽取数据
            private string ExtractData()
            {
                //定义输出字符串
                string message = "";
                object v = eventData.GetData();
                //分类处理
                if (v is System.Collections.IEnumerable)
                {
                    //转换成可枚举的
                    System.Collections.IEnumerable enumerable = (System.Collections.IEnumerable)v;
                    //添加名字
                    message += shortName + ":";

                    //历遍数组
                    foreach (object v1 in enumerable)
                    {
                        //添加数据
                        message += v1.ToString() + ",";
                    }
                    //去掉最后一个逗号
                    message = message.Substring(0, message.Length - 1);
                }
                else
                {
                    //添加数据
                    message += v.ToString();
                }
                return message;
            }




            //方法：输出日志
            public string GetDebugLog()
            {
                //定义输出字符串
                string message = "";
                object v = eventData.GetData();


                //如果是可枚举的
                if (v is System.Collections.IEnumerable)
                {
                    //转换成可枚举的
                    System.Collections.IEnumerable enumerable = (System.Collections.IEnumerable)v;
                    //添加名字
                    message += eventData.GetShortName() + ":";

                    //历遍数组
                    foreach (object v1 in enumerable)
                    {
                        //添加分隔符
                        if (message != "")
                            message += ",";
                        //添加数据
                        message += $"<color=red>{v1}</color>";
                    }
                }
                else
                {
                    //添加名字和数据
                    string data = v.ToString();
                    message = eventData.GetShortName() + " : " + $"<color=red>{data}</color>";
                }


                return message;
            }
        }

    }



}