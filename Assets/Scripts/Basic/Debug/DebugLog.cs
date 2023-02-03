using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public static class DebugF
{
    private static string logLine = "";
    private static int index = 0;
    private static float? currTime;

    public static void LogLine(System.Object logObj, string name = null, bool lineBreak = false)
    {
        if (currTime == null)
        {
            currTime = Time.time;
        }
        else
        {
            if (currTime != Time.time)
            {
                LogOut();
            }
        }
        logLine += $"{"{"} {index} {name} < {logObj.ToString()} > {"}"}  ";
        index++;

        if (lineBreak)
        {
            LogOut();
        }

        static void LogOut()
        {
            Debug.Log(logLine);
            index = 0;
            logLine = "";
            currTime = null;
        }

    }
    public static void LogLine(System.Object logObj, bool lineBreak = false)
    {
        LogLine(logObj, null, lineBreak);
    }
    public static void LogLine(System.Object logObj)
    {
        LogLine(logObj, null, false);
    }



    // ///<summary>拓展方法, 自动根据类型处理 </summary>
    // public static T Log<T>(this T content, string label = null, bool color = true, bool logFields = true, int lineBreak = 1)
    // {
    //     string v = "";

    //     //~根据优先级, 逐个判断类型
    //     if (content == null)//如果是null
    //     {
    //         v = "null";
    //     }
    //     else if (content is string) //如果为字符串
    //     {
    //         v = content.ToString();
    //     }
    //     //如果为可枚举的类型
    //     else if (content is IEnumerable)
    //     {
    //         //递增循环
    //         int i = 0;
    //         foreach (var item in content as IEnumerable)
    //         {
    //             string logStr = GetObjectLog(item, color, logFields);

    //             v += $"[<color=green>{i}</color>]{logStr} ;  ";
    //             i++;
    //         }
    //     }
    //     //如果为值元组
    //     else if (content.GetType().FullName.StartsWith("System.ValueTuple"))
    //     {
    //         //遍历
    //         int i = 0;
    //         foreach (var item in content.GetType().GetFields())
    //         {
    //             var value = item.GetValue(content);

    //             string logStr = GetObjectLog(value, color, logFields);

    //             v += $"[<color=green>{i}</color>]{logStr} ;  ";
    //             i++;
    //         }
    //     }

    //     //如果上面的都不是,则直接转换为字符串
    //     if (v == "")
    //     {
    //         v = content.ToString();
    //     }
    //     //如果有标题, 则添加标题
    //     if (label != null)
    //     {
    //         v = $"<color=yellow>{label}</color> : {v}";
    //     }
    //     //删除末尾的无意义字符
    //     v = v.TrimEnd(' ', ';', '\n');

    //     if (color)
    //     {
    //         Debug.LogFormat(v);
    //     }
    //     else
    //     {
    //         Debug.Log("<color=red>" + "123" + "</color>");
    //         Debug.Log(v);
    //     }


    //     return content;
    // }
    // ///<summary>拓展方法, 自动根据类型处理 </summary>
    // public static T Log_<T>(this T content, string label = null, bool color = true, int limitDepth = 3)
    // {

    //     var infos = GetObjectLogInfos(content, limitDepth);

    //     int maxDepth = infos.Max(x => x.depth);


    //     var logStrs = infos.Select(x =>
    //      {
    //          string logStr;

    //          string type = x.type == null ? "Null" : x.type.Name;
    //          string index = x.index.ToString();
    //          string name = x.name;
    //          string value = x.value;
    //          if (color)
    //          {
    //              type = $"<color=green>{type}</color>";
    //              index = $"<color=green>{index}</color>";
    //              name = $"<color=yellow>{name}</color>";
    //              value = $"<color=blue>{value}</color>";
    //          }
    //          //修饰
    //          type = $"({type})";
    //          index = $"[{index}]";
    //          name = $"{name} : ";

    //          //组装
    //          logStr = $"{index}{type}{name}{value}";
    //          //缩进
    //          string indent = "";
    //          indent = indent.PadLeft((maxDepth - x.depth) * 8, ' ');
    //          logStr = logStr.Insert(0, indent);


    //          return logStr;
    //      });

    //     Debug.Log(string.Join("\n", logStrs));

    //     return content;
    // }


    ///<summary> 将一个类型实例的字段打印出来 </summary>
    public static string GetClassFieldsLog<T>(T content, bool color = true)
    {
        string re = "";

        if (content == null)
        {
            re = "null";
            if (color)
            {
                re = $"<color=red>{re}</color>";
            }
        }
        else
        {
            var fields = content.GetType().GetFields();
            foreach (var item in fields)
            {
                var value = item.GetValue(content);
                if (value != null)
                {
                    re += $"{item.Name}: <color=yellow>{value.ToString()}</color> ;   ";
                }
                else
                {
                    re += $"{item.Name}: <color=red>null</color> ;   ";
                }
            }
        }
        re = re.TrimEnd(' ', ';');
        return re;
    }
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
            Debug.Log(depths.Count + "  " + indexes.Count + "  " + names.Count + "  " + types.Count + "  " + values.Count);
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




    ///<summary> 分析一个物体将其转化成 Log 字符串 </summary>
    public static string GetObjectLog<T>(T content, bool color = true, bool logFields = true)
    {
        string v = "";


        if (content == null)
        {
            string t = "null";
            if (color)
                t = $"<color=red>{t}</color>";
            v += t;
        }
        else if (content is IEnumerable)//如果为可枚举的
        {
            int i = 0;
            v += "<";
            foreach (var item in content as IEnumerable)
            {
                string logStr = GetObjectLog(item, color);
                string iStr = color ? $"<color=green>{i}</color>" : $"{i}";

                v += $"[{iStr}]{logStr} ;  ";
                i++;
            }
            v = v.TrimEnd(' ', ';');
            v += ">";
        }

        else if (content.GetType().IsValueType)//如果为值元组
        {
            v += $"{content.ToString()}";
        }
        else if (content.GetType().GetMethod("ToString").DeclaringType == content.GetType())//ToString方法是否是自己的, 即是否能得到有效的字符串
        {
            v += $"{content.ToString()}";

        }
        else if (content.GetType().IsClass)//获得字段
        {
            if (logFields)
                v += $"{GetClassFieldsLog(content, color)}";
        }

        v.TrimEnd(' ', ';', ':');
        return v;
    }





    public static void LogEach<T>(this IEnumerable<T> list, int lineBreakCount = 0)
    {
        if (list == null)
            return;

        string str = "";
        int i = 0;
        string Break() => lineBreakCount > 0 ? (i + 1) % lineBreakCount == 0 ? "\n" : null : null;


        foreach (var item in list)
        {
            str += $"{i}:{item.ToString()}; {Break()}";
            i++;
        }

        Debug.Log(str);
    }

    public static void LogEach<T, U>(this IEnumerable<T> list, Func<T, U> selection, int lineBreakCount = 0)
    {
        LogEach(list.Select((s) => selection(s)), lineBreakCount);
    }

    public static void LogEach<T, S>(this List<T> source, System.Func<T, S> selector, int lineBreak = 0)
    {
        List<S> lists = source.Select(selector).ToList();
        lists.LogEach(lineBreak);
    }




}
