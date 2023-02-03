using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;
using static Extension.DebugLog;

public static partial class Extension
{   //生成英文文档
    ///<summary culture="en-US">Log the content with color</summary>
    ///<summary>拓展方法, 自动根据类型处理 </summary>
   
    public static T Log<T>(this T content, string label = null, bool color = true, int limitDepth = 3)
    {

        var infos = GetObjectLogInfos(content, limitDepth);

        int maxDepth = infos.Max(x => x.depth);


        var logStrs = infos.Select(x =>
         {
             string logStr;

             string type = x.type == null ? "Null" : x.type.Name;
             string index = x.index.ToString();
             string name = x.name;
             string value = x.value;
             if (color)
             {
                 type = $"<color=green>{type}</color>";
                 index = $"<color=green>{index}</color>";
                 name = $"<color=yellow>{name}</color>";
                 value = $"<color=blue>{value}</color>";
             }
             //修饰
             type = $"({type})";
             index = $"[{index}]";
             name = $"{name} : ";

             //组装
             logStr = $"{index}{type}{name}{value}";
             //缩进
             string indent = "";
             indent = indent.PadLeft((maxDepth - x.depth) * 8, ' ');
             logStr = logStr.Insert(0, indent);


             return logStr;
         });

        Debug.Log(string.Join("\n", logStrs));

        return content;
    }


    public static class DebugLog
    {   ///<summary culture="en-US">Get the log infos of an object</summary>
        ///<summary> 将一个类型实例的字段打印出来 </summary>
        public static List<(int depth, int index, string name, Type type, string value)> GetObjectLogInfos(Object content, int limitDepth = 3)
        {
            List<int> depths = new List<int>();
            List<int> indexes = new List<int>();
            List<string> names = new List<string>();
            List<Type> types = new List<Type>();
            List<string> values = new List<string>();

            List<(int depth, int index, string name, Type type, string value)> GetReturn()
            {
                return depths.Select((x, i) => (depth: x, index: indexes[i], name: names[i], type: types[i], value: values[i])).ToList(); ;
            }

            depths.Add(limitDepth);
            indexes.Add(0);
            names.Add("");

            //~根据优先级, 逐个判断类型

            if (content == null)//如果是null
            {
                types.Add(null);
                values.Add("Null");
                return GetReturn();
            }

            types.Add(content.GetType());

            if (content is string) //如果为字符串
            {
                values.Add(content.ToString());
            }

            else if (content is IEnumerable)//如果为可枚举的
            {
                values.Add("");
                if (limitDepth > 0)
                {
                    int i = 0;
                    foreach (var valueObject in content as IEnumerable)
                    {
                        var logInfos = GetObjectLogInfos(valueObject, limitDepth - 1);
                        logInfos.ForEach(x =>
                        {
                            depths.Add(x.depth);
                            indexes.Add(i);
                            names.Add(x.name);
                            types.Add(x.type);
                            values.Add(x.value);
                        });
                        i++;
                    }
                }
            }

            else if (content.GetType().FullName.StartsWith("System.ValueTuple")) //如果为值元组
            {
                values.Add("");
                if (limitDepth > 0)
                {
                    int i = 0;
                    foreach (var item in content.GetType().GetFields())
                    {
                        var valueObject = item.GetValue(content);
                        var logInfos = GetObjectLogInfos(valueObject, limitDepth - 1);
                        logInfos.ForEach(x => { depths.Add(x.depth); indexes.Add(i); names.Add(item.Name); types.Add(x.type); values.Add(x.value); });
                        i++;
                    }
                }
            }
            else if (content.GetType().IsValueType)//如果为值类型
            {
                values.Add(content.ToString());
            }
            else if (content.GetType().GetMethod("ToString").DeclaringType == content.GetType())//ToString方法是否是自己的, 即是否能得到有效的字符串
            {
                values.Add(content.ToString());
            }
            else if (content.GetType().IsClass)//获得字段
            {
                values.Add("");
                if (limitDepth > 0)
                {
                    int i = 0;
                    foreach (var item in content.GetType().GetFields())
                    {
                        var valueObject = item.GetValue(content);
                        var logInfos = GetObjectLogInfos(valueObject, limitDepth - 1);
                        logInfos.ForEach(x => { depths.Add(x.depth); names.Add(item.Name); indexes.Add(i); types.Add(x.type); values.Add(x.value); });
                        i++;
                    }
                }
            }
            else
            {
                values.Add(content.ToString());
            }





            return GetReturn();
        }

    }





}
