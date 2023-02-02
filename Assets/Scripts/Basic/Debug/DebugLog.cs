using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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



    ///<summary>拓展方法, 自动根据类型处理 </summary>
    public static T Log<T>(this T content, string label = null, bool color = true, bool logFields = true)
    {
        string v = "";

        //~根据优先级, 逐个判断类型
        if (content == null)//如果是null
        {
            v = "null";
        }
        else if (content is string) //如果为字符串
        {
            v = content.ToString();
        }
        //如果为可枚举的类型
        else if (content is IEnumerable)
        {
            //递增循环
            int i = 0;
            foreach (var item in content as IEnumerable)
            {
                string logStr = GetObjectLog(item, color, logFields);

                v += $"[<color=green>{i}</color>]{logStr} ;  ";
                i++;
            }
        }
        //如果为值元组
        else if (content.GetType().FullName.StartsWith("System.ValueTuple"))
        {
            //遍历
            int i = 0;
            foreach (var item in content.GetType().GetFields())
            {
                var value = item.GetValue(content);

                string logStr = GetObjectLog(value, color, logFields);

                v += $"[<color=green>{i}</color>]{logStr} ;  ";
                i++;
            }
        }

        //如果上面的都不是,则直接转换为字符串
        if (v == "")
        {
            v = content.ToString();
        }
        //如果有标题, 则添加标题
        if (label != null)
        {
            v = $"<color=yellow>{label}</color> : {v}";
        }
        //删除末尾的无意义字符
        v = v.TrimEnd(' ', ';', '\n');

        if (color)
        {
            Debug.LogFormat(v);
        }
        else
        {
            Debug.Log(v);
        }


        return content;
    }


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
