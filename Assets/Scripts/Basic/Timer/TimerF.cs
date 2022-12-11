using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class TimerF
{
    public static GameObject TimerHolder => GameObjectF.GetObjectByNameOrCreate("TimerHolder");
    //计时器物体名


    public static Timer.Base.SimpleTimer Wait(float time, Action action = null)
    {
        Timer.Base.SimpleTimer simpleTimer = TimerHolder.AddComponent<Timer.Base.SimpleTimer>();
        simpleTimer.Wait(time, action);
        return simpleTimer;
    }
    public static Timer.Base.WaitUpdate WaitUpdate(Action action)
    {
        var timer = TimerHolder.AddComponent<Timer.Base.WaitUpdate>();
        timer.Setup(action);
        return timer;
    }

}


