using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static partial class ExtensionMethods
{

    public static string InsertLast(this string str, string text)
    {
        return str.Insert(str.Length, text);
    }

    public static Vector2 ToVector2(this string str, char splitter = ',')
    {
        string[] vs = str.Split(splitter);
        return new Vector2(float.Parse(vs[0]), float.Parse(vs[1]));
    }






    public static bool IsMatch(this string str, string regex)
    {
        return Regex.IsMatch(str, regex);
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


    public static string[] SplitWhite(this string str)
    {
        return Regex.Split(str, @"\s+");
    }




    public static string ToPath(this string str)
    {
        return Path.GetFullPath(str);
    }




    public static bool Contains(this string str, IEnumerable<string> contents)
    {
        foreach (var item in contents)
        {
            if (!str.Contains(item))
            {
                return false;
            }
        }

        return true;
    }






}