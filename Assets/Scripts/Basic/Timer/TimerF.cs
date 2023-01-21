using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class TimerF
{
    public static GameObject TimerHolder => GameObjectF.GetObjectByNameOrCreate("TimerHolder");
    //计时器物体名


    public static SimpleTimer.Base.SimpleTimer Wait(float time, Action action = null)
    {
        SimpleTimer.Base.SimpleTimer simpleTimer = TimerHolder.AddComponent<SimpleTimer.Base.SimpleTimer>();
        simpleTimer.Wait(time, action);
        return simpleTimer;
    }
    public static SimpleTimer.Base.WaitUpdate WaitUpdate(Action action)
    {
        var timer = TimerHolder.AddComponent<SimpleTimer.Base.WaitUpdate>();
        timer.Setup(action);
        return timer;
    }

}


