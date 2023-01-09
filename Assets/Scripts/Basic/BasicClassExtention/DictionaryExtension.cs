using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension
{
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key, Func<V> defaultValue)
    {
        bool v = dict.ContainsKey(key);
        if (v == false)
        {
            dict.Add(key, defaultValue());

        }

        return dict[key];
    }
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key) where V : new()
    {
        return GetOrCreate<K, V>(dict, key, () => new V());
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

    /// *<summary>添加并返回被覆盖的,返回是否替换成功</summary>
    public static bool AddAndReplace<K, V>(this Dictionary<K, V> dict, K key, V value, out V oldValue)
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

}
