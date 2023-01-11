using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtension
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




    public static float? TryFloat(this string str)
    {
        float? re = null;
        float f;
        bool v = float.TryParse(str, out f);
        if (v)
        {
            re = f;
        }
        return re;
    }
    public static float?[] TryFloat(this string[] strAr)
    {
        int length = strAr.Length;
        float?[] re = new float?[length];
        for (int i = 0; i < length; i++)
        {
            float f;
            bool v = float.TryParse(strAr[i], out f);
            if (v)
            {
                re[i] = f;
            }
        }

        return re;
    }

    public static bool IsMatch(this string str, string regex)
    {
        return Regex.IsMatch(str, regex);
    }




    public static string[] SplitWhite(this string str)
    {
        return Regex.Split(str, @"\s+");
    }
    public static int? TryInt(this string str)
    {
        int? re = null;
        int f;
        bool v = int.TryParse(str, out f);
        if (v)
        {
            re = f;
        }
        return re;
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