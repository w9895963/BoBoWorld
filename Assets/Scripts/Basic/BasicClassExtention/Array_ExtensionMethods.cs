using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


///<summary>数组扩展 ArrayExtension</summary>
public static partial class ExtensionMethods
{




    public static List<T> ToList<T>(this T[] source)
    {
        if (source == null)
        {
            return new List<T>();
        }
        return new List<T>(source);
    }





    public static bool TryGet<T>(this T[] array, int ind, out T t)
    {
        bool re = false;
        T tOUt = default;
        if (array != null)
        {
            if (array.Length > ind)
            {
                tOUt = array[ind];
                re = true;
            }
        }
        t = tOUt;
        return re;
    }




}
