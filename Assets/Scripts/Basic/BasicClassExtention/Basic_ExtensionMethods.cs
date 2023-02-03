using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static partial class ExtensionMethods
{







    #region System.Object
    public static bool IsType<T>(this System.Object obj)
    {
        return obj.GetType() == typeof(T);
    }

    public static bool TryConvert<T>(this System.Object obj, out T outData)
    {
        bool isType = obj.GetType() == typeof(T);
        if (isType)
        {
            outData = (T)obj;
        }
        else
        {
            outData = default;
        }
        return isType;
    }




    #endregion




    #region System.Action
    //这个很有可能是没用的
    public static void Invoke(this Action del, bool setNullAfterInvoke = false)
    {
        del.Invoke();
        if (setNullAfterInvoke) del = null;
    }

    #endregion
    // * Region System.Action End---------------------------------- 






    ///<summary>获取枚举的全名,包含类</summary>
    public static string GetFullName(this System.Enum enumName)
    {
        //获取枚举类型
        string typeName = enumName.GetType().FullName;
        //获取枚举名称
        string name = enumName.ToString();


        return typeName + "." + name;
    }
    ///<summary>获取枚举类型的所有字符串值</summary>
    public static string[] GetAllNamesToString(this System.Enum enumName)
    {
        //为空则返回空的字符串数组
        if (enumName == null) return new string[0];
        //设定返回列表
        List<string> list = new List<string>();
        //历遍枚举enumName类型的所有值
        foreach (System.Enum e in System.Enum.GetValues(enumName.GetType()))
        {
            list.Add(e.ToString());
        }



        return list.ToArray();
    }

}
