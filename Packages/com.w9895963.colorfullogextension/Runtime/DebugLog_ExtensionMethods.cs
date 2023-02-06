using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ExtensionMethods.DebugLog;
using Object = System.Object;

public static partial class ExtensionMethods
{

    ///<summary>自动根据类型把对象打印出来,根据类型自动处理</summary>
    ///<summary culture="en-US">Get the log infos of an object</summary>
    public static T Log<T>(this T content, string label = null, bool color = true, int limitDepth = 3, bool showValueType = true, bool showName = true, bool showIndex = true)
    {

        var infos = GetLogInfos(content, limitDepth);

        int maxDepth = infos.Max(x => x.depth);


        var logStrs = infos.Select(x =>
        {
            string logStr;

            string typeOfValue = x.type == null ? "Null" : GetName(x.type);
            string index = x.index == null ? "" : x.index.ToString();
            string name = x.name;
            string value = x.value;
            if (color)
            {
                typeOfValue = $"<color=green>{typeOfValue}</color>";
                index = index != "" ? $"<color=green>{index}</color>" : "";
                name = $"<color=yellow>{name}</color>";
                value = $"<color=cyan>{value}</color>";
            }
            //修饰
            typeOfValue = $"({typeOfValue})";
            index = index != "" ? $"[{index}]" : "";
            name = $"{name} : ";

            //开关
            typeOfValue = showValueType ? typeOfValue : "";
            index = showIndex ? index : "";
            name = showName ? name : "";

            //组装
            logStr = $"{index}{name}{value}{typeOfValue}";
            //缩进
            string indent = "";
            indent = indent.PadLeft((maxDepth - x.depth) * 8, ' ');
            logStr = logStr.Insert(0, indent);


            return logStr;
        });



        string message = string.Join("\n", logStrs);

        //添加标题
        if (label != null)
        {
            if (color)
                label = $"<color=red>{label}</color>";
            message = $"{label} : {message}";
        }

        Debug.Log(message);



        return content;
    }





    public static class DebugLog
    {


        ///<summary> 将一个类型实例的字段打印出来 </summary>
        ///<summary culture="en-US">Get the log infos of an object</summary>
        public static List<(int depth, int? index, string name, Type type, string value)> GetLogInfos(
            Object content,
            int limitDepth = 3,
            bool showPrivateField = false,
            bool showDelegate = false)
        {
            List<int> depths = new List<int>();
            List<int?> indexes = new List<int?>();
            List<string> names = new List<string>();
            List<Type> types = new List<Type>();
            List<string> values = new List<string>();

            List<(int depth, int? index, string name, Type type, string value)> GetReturn()
            {
                return depths.Select((x, i) => (depth: x, index: indexes[i], name: names[i], type: types[i], value: values[i])).ToList(); ;
            }

            depths.Add(limitDepth);
            indexes.Add(null);
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
                int curIndex = values.Count - 1;

                int i = 0;
                foreach (var valueObject in content as IEnumerable)
                {
                    if (limitDepth > 0)
                    {
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1);
                        logInfos[0] = (logInfos[0].depth, i, logInfos[0].name, logInfos[0].type, logInfos[0].value);
                        logInfos.ForEach(x => { depths.Add(x.depth); names.Add(x.name); indexes.Add(x.index); types.Add(x.type); values.Add(x.value); });
                    }
                    i++;
                }
                values[curIndex] = $"({i})⇣";

            }

            else if (content.GetType().FullName.StartsWith("System.ValueTuple")) //如果为值元组
            {
                values.Add("");
                int curIndex = values.Count - 1;

                int i = 0;
                foreach (var item in content.GetType().GetFields())
                {
                    if (limitDepth > 0)
                    {
                        var valueObject = item.GetValue(content);
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1);
                        logInfos[0] = (logInfos[0].depth, i, item.Name, logInfos[0].type, logInfos[0].value);
                        logInfos.ForEach(x => { depths.Add(x.depth); indexes.Add(x.index); names.Add(x.name); types.Add(x.type); values.Add(x.value); });
                    }
                    i++;
                }
                values[curIndex] = $"({i})⇣";

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
                int curIndex = values.Count - 1;

                int i = 0;
                BindingFlags BindingAttr = BindingFlags.Instance | BindingFlags.Public;
                if (showPrivateField)
                    BindingAttr |= BindingFlags.NonPublic;
                foreach (var item in content.GetType().GetFields(BindingAttr))
                {
                    if (limitDepth > 0)
                    {
                        var valueObject = item.GetValue(content);
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1);
                        logInfos[0] = (logInfos[0].depth, i, item.Name, logInfos[0].type, logInfos[0].value);
                        if (showDelegate == false)
                        {
                            logInfos.RemoveRange(1, logInfos.Count - 1);
                        }
                        logInfos.ForEach(x => { depths.Add(x.depth); names.Add(x.name); indexes.Add(x.index); types.Add(x.type); values.Add(x.value); });
                    }
                    i++;

                }
                foreach (var item in content.GetType().GetProperties(BindingAttr))
                {
                    Object valueObject = null;

                    if (limitDepth > 0)
                    {


                        try
                        {
                            valueObject = item.GetValue(content);
                        }
                        catch (Exception)
                        {
                            valueObject = null;
                            // Debug.LogError(e);
                        }
                        if (valueObject != null)
                        {
                            var logInfos = GetLogInfos(valueObject, limitDepth - 1);
                            logInfos[0] = (logInfos[0].depth, i, item.Name, logInfos[0].type, logInfos[0].value);
                            if (showDelegate == false)
                            {
                                logInfos.RemoveRange(1, logInfos.Count - 1);
                            }
                            logInfos.ForEach(x => { depths.Add(x.depth); names.Add(x.name); indexes.Add(x.index); types.Add(x.type); values.Add(x.value); });
                        }



                    }
                    if (valueObject != null)
                    {
                        i++;
                    }


                }

                values[curIndex] = $"({i})⇣";

            }
            else
            {
                values.Add(content.ToString());
            }





            return GetReturn();
        }





    }
    ///<summary>获得类型的类型名, 带有类型参数</summary>
    private static string GetName(this Type type)
    {
        string typeName = type.Name;
        if (type.IsGenericType)
        {
            typeName = typeName.Substring(0, typeName.IndexOf('`'));
            typeName += "<";
            Type[] genericTypes = type.GetGenericArguments();
            typeName += string.Join(",", genericTypes.Select(x => GetName(x)));
            typeName += ">";
        }
        return typeName;
    }
}
