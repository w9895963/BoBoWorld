using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ExtensionMethods
{



    public static bool Exist<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        bool v = dict.ContainsKey(key);
        if (v == false)
        {
            return false;
        }
        else
        {
            return dict[key].Equals(value);
        }

    }

    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key, Func<V> defaultValue = null)
    {


        if (dict.TryGetValue(key, out V re))
        {

        }
        else
        {
            if (defaultValue != null)
            {
                re = defaultValue();
            }
            dict.Add(key, re);
        }


        return re;

    }





    public static V TryGetValue<K, V>(this Dictionary<K, V> dict, K key) where V : class
    {
        bool v = dict.ContainsKey(key);
        if (v == false)
        {
            return null;
        }

        return dict[key];
    }
    public static bool TryRemove<K, V>(this Dictionary<K, V> dict, K key, out (K key, V value) item)
    {
        if (dict.ContainsKey(key) == false)
        {
            item = (default, default);
            return false;
        }
        item = (key, dict[key]);
        dict.Remove(key);
        return true;
    }

    ///<summary>添加并返回被覆盖的,返回是否替换成功</summary>
    public static bool TryAddAndReplace<K, V>(this Dictionary<K, V> dict, K key, V value, out V oldValue)
    {
        bool isExist = dict.ContainsKey(key);
        //如果存在
        if (isExist)
        {
            //如果不同,则替换
            if (dict[key].Equals(value) == false)
            {
                oldValue = dict[key];
                dict[key] = value;
                return true;
            }
        }
        dict[key] = value;
        oldValue = default;
        return false;

    }

    ///<summary>尝试从值获得对应的索引</summary>
    public static bool TryGetKey<K, V>(this Dictionary<K, V> dict, V value, out K key)
    {
        foreach (var item in dict)
        {
            if (System.Object.Equals(item.Value, value))
            {
                key = item.Key;
                return true;
            }
        }
        key = default;
        return false;
    }



}
