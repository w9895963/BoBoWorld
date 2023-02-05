using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IEnumerable = System.Collections.IEnumerable;

///<summary>数组扩展</summary>
public static partial class ExtensionMethods
{
    public static T FindType<T>(this IEnumerable source, T defaultOut = default)
    {
        foreach (var item in source)
        {
            if (item.IsType<T>())
            {
                return (T)Convert.ChangeType(item, typeof(T));
            }
        }

        return defaultOut;
    }
  

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        if (enumeration == null)
            return;
        foreach (var t in enumeration)
        {
            action(t);
        }

    }
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
    {
        int ind = 0;
        foreach (var t in enumeration)
        {
            action(t, ind);
            ind++;
        }
    }
    public static T GetRandom<T>(this IEnumerable<T> source)
    {
        int i = UnityEngine.Random.Range(0, source.Count());
        return source.ElementAt(i);
    }

    ///<summary>为Null或空</summary>
    public static bool IsEmpty<T>(this T source) where T : System.Collections.IEnumerable
    {
        if (source != null)
        {
            foreach (var item in source)
            {
                return false;
            }
        }

        return true;
    }
    ///<summary>不为Null且不空</summary>
    public static bool IsNotEmpty<T>(this T source) where T : System.Collections.IEnumerable
    {
        return !IsEmpty(source);
    }
    ///<summary>合并所有</summary>
    public static string Join(this IEnumerable<string> source, string separator = "")
    {
       return string.Join(separator, source);
    }

    ///<summary>选择但是不包含Null</summary>
    public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      return  source.Select(selector).Where((x) => x != null);
    }


    ///<summary>SelectMany方法拓展:排除Null</summary>
    public static IEnumerable<TResult> SelectManyNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
        return source.SelectMany(selector).Where((x) => x != null);
    }




    public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> source, System.Func<T, int, (K, V)> selector)
    {

        Dictionary<K, V> re = new Dictionary<K, V>();
        source.ForEach((x, i) =>
        {
            (K, V) p = selector(x, i);
            re.Add(p.Item1, p.Item2);
        });
        return re;
    }
    public static Dictionary<K, V> ToDictionary<T, K, V>(this IEnumerable<T> source, System.Func<T, (K, V)> selector)
    {

        Dictionary<K, V> re = new Dictionary<K, V>();
        source.ForEach((x) =>
        {
            (K, V) p = selector(x);
            re.Add(p.Item1, p.Item2);
        });
        return re;
    }
    public static Dictionary<K, T> ToDictionary<K, T>(this IEnumerable<T> source, System.Func<T, K> selector)
    {

        Dictionary<K, T> re = new Dictionary<K, T>();
        source.ForEach((x) =>
        {
            re.Add(selector(x), x);
        });
        return re;
    }




    public static IEnumerable<T> WhereIsNull<T>(this IEnumerable<T> source)
    {
        return source.Where((x) => x == null);
    }
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
    {
        return source.Where((x) => x != null);
    }










}
