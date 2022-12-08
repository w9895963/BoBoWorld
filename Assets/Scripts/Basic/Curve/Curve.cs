using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Curve
{

    //* Class Definition
    public class CurveCs
    {
        public float inputMax = 1;
        public float inputMin = 0;
        public float outputMax = 1;
        public float outputMin = 0;
        public AnimationCurve curve = ZeroOne;
        public float Evaluate(float index)
        {
            float i = (index - inputMin) / (inputMax - inputMin);
            return curve.Evaluate(i) * (outputMax - outputMin) + outputMin;
        }
    }



    //* Public Property
    public static AnimationCurve Default = ZeroOne;

    public static AnimationCurve ZeroOne
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 0, 0, 0, 0, 0), new Keyframe(1, 1, 0, 0, 0, 0));
        }
    }
    public static AnimationCurve ZeroOneFastOut01
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 0, 0, 1.5f, 0, 0.4f), new Keyframe(1, 1, 0, 0, 0, 0));
        }
    }
    public static AnimationCurve ZeroOneSmooth03
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 0, 0, 0, 0, 0.3f), new Keyframe(1, 1, 0, 0, 0.3f, 0));
        }
    }
    public static AnimationCurve OneZero
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 1, 0, 0, 0, 0), new Keyframe(1, 0, 0, 0, 0, 0));
        }
    }
    public static AnimationCurve OneOneCurve
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        }
    }
    public static AnimationCurve ZeroZero
    {
        get
        {
            return new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
        }
    }



    //* Public Method
    public static void SetCurveByVector2(AnimationCurve curve, Vector2 vars)
    {
        vars.x = vars.x.Clamp(-0.999f, 0.999f);
        vars.y = vars.y.Clamp(-0.999f, 0.999f);
        Keyframe k1 = curve.keys[0];
        Keyframe k2 = curve.keys[1];
        var k1Want = vars.x.Map(-1, 1, 0, 1);
        var key1 = new Keyframe(k1.time, k1.value, 0, (k1Want - k1.value) / k1Want, 0, k1Want);
        var k2Want = vars.y.Map(-1, 1, 0, 1);
        var key2 = new Keyframe(k2.time, k2.value, -(k2Want - k2.value) / (1 - k2Want), 0, 1 - k2Want, 0);
        curve.keys = new Keyframe[] { key1, key2 };
    }
    public static Vector2 CurveToVector2(AnimationCurve curve)
    {
        Keyframe k1 = curve.keys[0];
        Keyframe k2 = curve.keys[1];
        return new Vector2(k1.outWeight.Map(0, 1, -1, 1), k2.inWeight.Map(0, 1, 1, -1));
    }
    public static float Evaluate(float index, float inputMin, float inputMax,
        float outputMin, float outputMax, AnimationCurve curve = null)
    {

        if (curve != null)
        {
            CurveCs cur = new CurveCs();
            cur.curve = curve;
            cur.inputMax = inputMax;
            cur.inputMin = inputMin;
            cur.outputMin = outputMin;
            cur.outputMax = outputMax;
            return cur.Evaluate(index);
        }
        else
        {
            return (index.Clamp(inputMin, inputMax) - inputMin) / (inputMax - inputMin)
                * (outputMax - outputMin) + outputMin;
        }
    }

    public static float Evaluate(this AnimationCurve curve, float index, float inputMin, float inputMax,
        float outputMin = 0, float outputMax = 1)
    {
        return Curve.Evaluate(index, inputMin, inputMax, outputMin, outputMax, curve);
    }
}
