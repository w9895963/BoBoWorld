using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EventData
{
    //只在编辑器下运行
    //可视化
    namespace EventDataVisualizeGroup
    {
        //*类：数据条目
        [System.Serializable]
      
        public class DataItem
        {
            [HideInInspector]
            public string labelName;

            public string 全名;
            public string 数据;
            public bool 打印 = false;

            public Core.EventData eventData;
            //隐藏显示
            [HideInInspector]
            public string shortName;
            //是否已经添加事件
            [HideInInspector]
            public bool isAddedEvent = false;


            //构造函数
            public DataItem(Core.EventData eventData)
            {
                this.eventData = eventData;
                全名 = eventData.Key;
                数据 = ExtractData();
                //名字=名字用"."分割的最后一个
                shortName = GetShortName(eventData.Key);
                //字符串插值将名字与值加起来
                labelName = $"{shortName}:{数据}";
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
                else if (v != null)
                {
                    message += v.ToString();
                }

                return message;
            }




            private static string GetShortName(string name)
            {
                //以点分割取最末尾
                return name.Split('.').Last();
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
                    message += GetShortName(eventData.Key) + ":";

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

                    message = GetShortName(eventData.Key) + " : " + $"<color=red>{data}</color>";
                }


                return message;
            }
        }

    }



}