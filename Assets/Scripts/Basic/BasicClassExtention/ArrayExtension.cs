using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>数组扩展 ArrayExtension</summary>
public static class ExtensionArray
{





    public static void Add<T>(this List<T> source, params T[] newMembers)
    {
        foreach (var item in newMembers)
        {
            source.Add(item);
        }
    }


    /// <summary>添加到列表中的特定位置,如果列表长度不够则填补</summary>
    public static void AddToIndex<T>(this List<T> source, int index, T addMember, T defaultMember = default)
    {
        if (source.Count <= index)
        {
            for (int i = source.Count; i < index + 1; i++)
            {
                source.Add(defaultMember);
            }
        }
        source[index] = addMember;
    }
    /// <summary>填充列表直到特定长度</summary>
    public static void AddTillLength<T>(this List<T> source, int length, T addMember)
    {
        while (source.Count < length)
        {
            source.Add(addMember);
        }
    }


    public static void AddNotHas<T>(this List<T> source, T newMember)
    {
        if (!source.Contains(newMember))
        {
            source.Add(newMember);
        }

    }
    public static void AddNotHas<T>(this List<T> source, IEnumerable<T> newMembers)
    {
        foreach (var item in newMembers)
        {
            if (!source.Contains(item))
            {
                source.Add(item);
            }
        }

    }

    public static void AddNotHas<T>(this List<T> source, T newMember, System.Predicate<T> match)
    {
        if (!source.Exists(match))
        {
            source.Add(newMember);
        }

    }
    /// <summary>添加到列表前, 要添加的元素进行一个Null检测, 为Null则不添加</summary>
    public static void AddNotNull<T>(this List<T> source, T newMember)
    {
        if (newMember != null)
        {
            source.Add(newMember);
        }
    }

    /// <summary> 添加一系列元素到列表, 要添加的元素进行一个Null检测, 为Null则不添加</summary>
    public static void AddRangeNotNull<T>(this List<T> source, IEnumerable<T> newMembers)
    {
        if (newMembers == null)
        {
            return;
        }
        source.AddRange(newMembers.WhereNotNull());
    }




    public static T FIndOrAdd<T>(this List<T> list, T newMember, System.Predicate<T> match)
    {
        T re = newMember;

        bool v = list.Exists(match);
        if (v)
        {
            re = list.Find(match);
        }
        else
        {
            list.Add(newMember);
        }

        return re;
    }


    public static T FIndType<T>(this IEnumerable source, T defaultOut = default)
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


    public static T GetPrevious<T>(this List<T> list, System.Predicate<T> predicate) where T : class
    {
        int v = list.FindIndex(predicate);
        if (v <= 0)
        {
            return null;
        }
        else
        {
            return list[v - 1];
        }
    }



    /// <summary>为Null或空</summary>
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

    /// <summary>移除所有相同单位</summary>
    public static void RemoveAll<T>(this List<T> source, T items)
    {
        source.RemoveAll((x) => x.Equals(items));
    }

    public static void RemoveEach<T>(this List<T> source, params T[] items)
    {
        foreach (var item in items)
        {
            source.Remove(item);
        }
    }

    public static List<T> RemoveNull<T>(this List<T> source)
    {
        source.RemoveAll((x) => x == null);
        return source;
    }


    public static T RandomGet<T>(this IEnumerable<T> source)
    {
        int i = UnityEngine.Random.Range(0, source.Count());
        return source.ElementAt(i);
    }




    public static void SortBy<T>(this List<T> source, IEnumerable<int> index)
    {
        var i = index.ToArray();
        var indexDic = source.ToDictionary((x, i) => (x, i));
        source.Sort((x, y) => i[indexDic[x]].CompareTo(i[indexDic[y]]));
    }
    public static void SortBy<T>(this List<T> source, Func<T, int> index)
    {
        source.Sort((x, y) => index(x).CompareTo(index(y)));
    }








    public static List<T> ToList<T>(this T[] source)
    {
        if (source == null)
        {
            return new List<T>();
        }
        return new List<T>(source);
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






    public static R TryGet<T, R>(this T[] array, int ind) where R : class
    {
        R re = null;
        if (array != null)
        {
            if (array.Length > ind)
            {
                re = array[ind] as R;
            }
        }
        return re;
    }
    public static T TryGet<T>(this T[] array, int ind) where T : class
    {
        T re = null;
        if (array != null)
        {
            if (array.Length > ind)
            {
                re = array[ind];
            }
        }
        return re;
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
    public static bool TryGet<T>(this T[] array, int ind, Action<T> onGet)
    {
        bool re = false;
        if (array != null)
        {
            if (array.Length > ind)
            {
                var tOUt = array[ind];
                if (tOUt != null)
                {
                    onGet.Invoke(tOUt);
                    re = true;
                }
            }
        }
        return re;
    }
    public static bool TryGet<T>(this System.Object[] array, int ind, Action<T> onGet)
    {
        bool re = false;
        if (array != null)
        {
            if (array.Length > ind)
            {
                var tOUt = array[ind];
                if (tOUt != null)
                {
                    Type type = tOUt.GetType();
                    Type type1 = typeof(T);
                    T v = (T)tOUt;
                    onGet.Invoke(v);
                    re = true;
                }
            }
        }
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
