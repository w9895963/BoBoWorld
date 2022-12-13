using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension
{
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key, V defaultValue)
    {
        bool v = dict.ContainsKey(key);
        if (v == false)
        {
            dict.Add(key, defaultValue);
            
        }

        return dict[key];
    }
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key) where V : new()
    {
        return GetOrCreate<K, V>(dict, key, new V());
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
   



    public static void Set<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        bool v = dict.ContainsKey(key);
        if (v == false)
        {
            dict.Add(key, value);
        }
        else
        {
            dict[key] = value;
        }

    }
}
