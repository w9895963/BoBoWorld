using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class ExtensionMethodBasic
{




    #region Float & Int





    public static float Min(this float f, float compareWith)
    {
        return f < compareWith ? f : compareWith;
    }
    public static float Sign(this float f)
    {
        return Mathf.Sign(f);
    }
    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
    }

    public static float Sqrt(this float f)
    {
        return Mathf.Sqrt(f);
    }
    public static float PowSafe(this float f, float p)
    {
        return Mathf.Pow(f.Abs(), p) * f.Sign();
    }
    public static float Shape(this float f, float pow, float move = 0, float div = 1)
    {
        float abs = Mathf.Abs(f);
        float sign = Mathf.Sign(f);
        float p1 = (Mathf.Pow((abs + move) / div, pow) * div) - move;
        return p1 * sign;
    }



    public static float Clamp(this float f, float min, float max)
    {
        return Mathf.Clamp(f, min, max);
    }
    public static int Clamp(this int f, int min, int max)
    {
        return Mathf.Clamp(f, min, max);
    }
    public static float ClampMin(this float f, float min)
    {
        return f > min ? f : min;
    }
    public static float ClampAbsMin(this float f, float min)
    {
        float fas = Mathf.Abs(f);
        float fsi = Mathf.Sign(f);
        fas = fas < min ? min : fas;
        return fas * fsi;
    }
    public static float ClampMax(this float f, float max)
    {
        return f < max ? f : max;
    }
    public static float ClampAbsMax(this float f, float max)
    {
        return f.Abs() < max.Abs() ? f : max.Abs() * f.Sign();
    }
    public static int ClampMax(this int i, int max)
    {
        return i < max ? i : max;
    }
    public static int ClampMin(this int i, int Min)
    {
        return i > Min ? i : Min;
    }

    public static float ZeroRid(this float f, bool negative = false, float value = 0.0001f)
    {
        return f != 0 ? f : (negative == false ? Mathf.Abs(value) : -Mathf.Abs(value));
    }



    public static Vector2 Map(this int f, int inputStart, int inputEnd, Vector2 outputStart, Vector2 outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
    }
    public static float Map(this float f, float inputStart, float inputEnd, float outputStart, float outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
    }

    public static Vector2 Map(this float f, float inputStart, float inputEnd, Vector2 outputStart, Vector2 outputEnd, bool clamp = true)
    {
        float f1 = f;
        if (clamp) f1 = f.Clamp(inputStart, inputEnd);
        return (f1 - inputStart) / (inputEnd - inputStart) * (outputEnd - outputStart) + outputStart;
    }


    public static float Floor(this float f)
    {
        return Mathf.Floor(f);
    }
    public static int FloorToInt(this float f)
    {
        return Mathf.FloorToInt(f);
    }
    public static float Ceil(this float f)
    {
        return Mathf.Ceil(f);
    }
    public static int CeilToInt(this float f)
    {
        return (int)Mathf.Ceil(f);
    }
    public static float Ceil(this float f, float step)
    {
        return Mathf.Ceil(f / step) * step;
    }

    public static bool ToBool(this float f)
    {
        return f > 0 ? true : false;
    }
    public static float[] ToArray(this float f)
    {
        return new float[1] { f };
    }



    public static Vector2 ToVector2(this float fl)
    {
        return new Vector2(fl, fl);
    }

    #endregion




    #region Bool
    public static float ToFloat(this bool boo)
    {
        return boo == true ? 1f : 0f;
    }
    public static bool Revert(this bool boo)
    {
        return boo == true ? false : true;
    }


    #endregion





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
    //  Region   End---------------------------------- 



    #region System.Action
    public static void Invoke(this Action del, bool setNullAfterInvoke = false)
    {
        del.Invoke();
        if (setNullAfterInvoke) del = null;
    }

    #endregion
    //  Region System.Action End---------------------------------- 




    #region UnityEvent
    private static Dictionary<UnityAction, UnityAction> addListenerOnce_Dic = new Dictionary<UnityAction, UnityAction>();
    public static void AddListenerOnce(this UnityEvent evt, UnityAction act)
    {
        UnityAction onceAct = () =>
        {
            act?.Invoke();
            evt.RemoveListenerOnce(act);
        };
        addListenerOnce_Dic[act] = onceAct;

        evt.AddListener(onceAct);
    }
    public static void RemoveListenerOnce(this UnityEvent evt, UnityAction act)
    {
        UnityAction ac;
        bool isHas = addListenerOnce_Dic.TryGetValue(act, out ac);
        if (isHas)
        {
            evt.RemoveListener(ac);
        }


    }
    #endregion
    //  Region UnityEvent End---------------------------------- 




    /// <summary>获取枚举的全名,包含类</summary>
    public static string GetFullName(this System.Enum enumName)
    {
        //获取枚举类型
        string typeName = enumName.GetType().FullName;
        //获取枚举名称
        string name = enumName.ToString();


        return typeName + "." + name;
    }
    /// <summary>获取枚举类型的所有字符串值</summary>
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



