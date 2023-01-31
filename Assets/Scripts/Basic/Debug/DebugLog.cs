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



    ///<summary> 拓展方法, 自动根据类型处理 </summary>
    public static T Log<T>(this T content, string label = null, bool color = true)
    {
        string v = "";
        //如果是null
        if (content == null)
        {
            v = "null";
        }
        //如果为可枚举的类型
        if (content is IEnumerable)
        {
            //如果为字符串
            if (content is string)
            {

            }
            else
            {
                //递增循环
                int i = 0;
                foreach (var item in content as IEnumerable)
                {
                    if (item == null)
                    {
                        v += $"[<color=green>{i}</color>]<color=red>null</color> ;  ";
                        i++;
                    }
                    else if (item.GetType().IsClass)
                    {
                        v += $"[<color=green>{i}</color>]{GetClassFieldsLog(item)}, \n";
                        i++;
                    }
                    else
                    {
                        v += $"[<color=green>{i}</color>]{item.ToString()} ;  ";
                        i++;
                    }
                }


            }

        }

        //如果上面的都不是
        if (v == "")
        {
            v = content.ToString();
        }

        if (label != null)
        {
            v = $"<color=yellow>{label}</color> : {v}";
        }

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
    public static string GetClassFieldsLog<T>(T content, bool color = true) where T : class
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
