using System;
using System.Collections.Generic;
using System.Linq;




///<summary>数组扩展</summary>
public static partial class ExtensionMethods
{



    public static void Add<T>(this List<T> source, params T[] newMembers)
    {
        foreach (var item in newMembers)
        {
            source.Add(item);
        }
    }


    ///<summary>添加到列表中的特定位置,如果列表长度不够则填补</summary>
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
    ///<summary>填充列表直到特定长度</summary>
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
    ///<summary>添加到列表前, 要添加的元素进行一个Null检测, 为Null则不添加</summary>
    public static void AddNotNull<T>(this List<T> source, T newMember)
    {
        if (newMember != null)
        {
            source.Add(newMember);
        }
    }

    ///<summary> 同 AddRange ,添加一系列元素到列表, 对参数本身以及其每个元素进行 null 检测并排除</summary>
    public static void AddRangeNotNull<T>(this List<T> source, IEnumerable<T> newMembers)
    {
        if (newMembers == null)
        {
            return;
        }
        source.AddRange(newMembers.WhereNotNull());
    }




    public static T FindOrAdd<T>(this List<T> list, T newMember, System.Predicate<T> match)
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





    ///<summary>移动列表的某个元素</summary>
    public static void Move<T>(this List<T> source, int from, int to)
    {
        if (from == to)
        {
            return;
        }
        T temp = source[from];
        source.RemoveAt(from);
        source.Insert(to, temp);
    }



    ///<summary>移除所有相同单位</summary>
    public static void RemoveAll<T>(this List<T> source, T items)
    {
        source.RemoveAll((x) => x.Equals(items));
    }

    ///<summary>移除末尾</summary>
    public static void RemoveLast<T>(this List<T> source)
    {
        if (source.Count > 0)
            source.RemoveAt(source.Count - 1);
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


    ///<summary>尝试查找</summary>
    public static bool TryFind<T>(this List<T> source, System.Predicate<T> match, out T result)
    {
        result = source.Find(match);
        return result != null;
    }

    ///<summary>尝试删除</summary>
    public static bool TryRemove<T>(this List<T> source, System.Predicate<T> match, out T result)
    {
        T t = source.Find(match);
        source.Remove(t);
        result = t;
        return t != null;
    }
    ///<summary>尝试删除</summary>
    public static bool TryRemoveAll<T>(this List<T> source, System.Predicate<T> match, out T[] result)
    {
        List<T> t = source.FindAll(match);
        foreach (var it in t)
            source.Remove(it);
        result = t?.ToArray() ?? new T[0];
        return t.IsNotEmpty();
    }





}
