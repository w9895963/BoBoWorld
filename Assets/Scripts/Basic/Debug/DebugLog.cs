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







    public static T LogSmart<T>(this T content)
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
                foreach (var item in content as IEnumerable)
                {
                    v += $"{item.ToString()}, ";
                }
            }

        }


        if (v == "")
        {
            v = content.ToString();
        }


        Debug.Log(v);

        return content;
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
