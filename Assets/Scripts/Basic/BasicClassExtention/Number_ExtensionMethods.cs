using System;
using UnityEngine;

public static partial class ExtensionMethods
{
    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
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

    /// <summary>循环</summary>
    public static void Loop(this int times,Action<int> action)
    {
        for (int i = 0; i < times; i++)
        {
            action(i);
        }
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



    /// <summary>最大值</summary>
    public static int Max(this int i, int compareWith)
    {
        return i > compareWith ? i : compareWith;
    }
    public static float Min(this float f, float compareWith)
    {
        return f < compareWith ? f : compareWith;
    }


    public static float Sign(this float f)
    {
        return Mathf.Sign(f);
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






    public static float Floor(this float f)
    {
        return Mathf.Floor(f);
    }
    public static int FloorToInt(this float f)
    {
        return Mathf.FloorToInt(f);
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




    public static float ZeroRid(this float f, bool negative = false, float value = 0.0001f)
    {
        return f != 0 ? f : (negative == false ? Mathf.Abs(value) : -Mathf.Abs(value));
    }


}