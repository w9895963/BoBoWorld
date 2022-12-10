using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class TimerF
{
    private static GameObject timerHolder;
    public static GameObject TimerHolder => Fc.FieldGetOrSet(ref timerHolder, () => new GameObject("TimerHolder"));


    public static Timer.Base.SimpleTimer Wait(float time, Action action = null)
    {
        GameObject hol = TimerHolder;
        //如果空退出
        if (TimerHolder == null)
            return null;
        Timer.Base.SimpleTimer simpleTimer = hol.AddComponent<Timer.Base.SimpleTimer>();
        simpleTimer.Wait(time, action);
        return simpleTimer;
    }
    public static Timer.Base.WaitUpdate WaitUpdate(Action action)
    {
        //如果空退出
        if (TimerHolder == null)
            return null;

        var timer = TimerHolder.AddComponent<Timer.Base.WaitUpdate>();
        timer.Setup(action);
        return timer;
    }

}


