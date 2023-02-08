using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EaseTool;
using UnityEngine;
using static ExtensionMethods.DebugLog;
using Object = System.Object;

public static partial class ExtensionMethods
{

   


    ///<summary>自动根据类型把对象打印出来,根据类型自动处理</summary>
    ///<summary culture="en-US">log an object</summary>
    public static T Log<T>(this T content, EaseTool.EaseLog.LogParam logParam = null)
    {
        if (logParam == null)
            logParam = new EaseTool.EaseLog.LogParam();


        string message = "";

        List<EaseTool.EaseLog.LogInfo> infos = EaseTool.EaseLog.GetLogInfos(content, logParam);
        //将深度反转, 使其从0开始
        int maxDepth = infos.Max(x => x.depth);
        infos.ForEach(x => x.depth = maxDepth - x.depth);




        //获取所有要打印的行
        IEnumerable<string> texts = infos.Select(x =>
        {
            string re = "";

            string indent = logParam.indentContent.Repeat(logParam.indent * x.depth);
            string typeStr = x.type == null ? "Null" : GetName(x.type);
            string indexStr = x.index == null ? "" : x.index.ToString();

            string nameString = EaseLog.GetLogString(x.name, logParam.showName, logParam.nameColor, logParam.showColor, logParam.nameTemplate);
            string valueString = EaseLog.GetLogString(x.value, logParam.showValue, logParam.valueColor, logParam.showColor, logParam.valueTemplate);
            string typeString = EaseLog.GetLogString(typeStr, logParam.showType, logParam.typeColor, logParam.showColor, logParam.typeTemplate);
            string indexString = EaseLog.GetLogString(indexStr, logParam.showIndex, logParam.indexColor, logParam.showColor, logParam.indexTemplate);

            re = indent + string.Format(logParam.lineTemplate, indexString, nameString, typeString, valueString);


            return re;
        });


        //组合行
        message = string.Join("\n", texts);


        //添加标题
        string label = EaseLog.GetLogString(logParam.label,  logParam.label != null, logParam.labelColor, logParam.showColor, logParam.labelTemplate);
        message = $"{label}{message}";


        Debug.Log(message);

        return content;
    }



    ///<summary>自动根据类型把对象打印出来,根据类型自动处理</summary>
    ///<summary culture="en-US">log an object</summary>
    public static T Log<T>(this T content, string label, EaseTool.EaseLog.LogParam logParam = null)
    {
        if (logParam == null)
            logParam = new EaseTool.EaseLog.LogParam();

        logParam.label = label;

        return Log(content, logParam);
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

    /// <summary>复制字符串一定数量的次数</summary>
    public static string Repeat(this string source, int repeatTimes)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < repeatTimes; i++)
        {
            sb.Append(source);
        }

        return sb.ToString();
    }





    public static class DebugLog
    {


        ///<summary> 将一个类型实例的字段打印出来 </summary>
        ///<summary culture="en-US">Get the log infos of an object</summary>
        public static List<(int depth, int? index, string name, Type type, string value)> GetLogInfos(
            Object content, int limitDepth = 3, bool showPrivateField = false, bool showDelegate = false)
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
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1, showPrivateField, showDelegate);
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
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1, showPrivateField, showDelegate);
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
                        var logInfos = GetLogInfos(valueObject, limitDepth - 1, showPrivateField, showDelegate);
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
                            var logInfos = GetLogInfos(valueObject, limitDepth - 1, showPrivateField, showDelegate);
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
}

