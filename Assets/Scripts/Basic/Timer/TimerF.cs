using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;



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



    ///<summary> 等待下一帧更新世执行操作, 返回一个停止计时器的操作</summary>
    public static Action WaitNextFrameUpdate(Action action)
    {
        int beginFrame = Time.frameCount;

        Action re = () =>
        {
            BasicEvent.OnUpdate.Remove(TimerHolder, OnUpdate);
        };


        BasicEvent.OnUpdate.Add(TimerHolder, OnUpdate);

        void OnUpdate()
        {
            //如果已经是下一帧了
            if (Time.frameCount != beginFrame)
            {
                BasicEvent.OnUpdate.Remove(TimerHolder, OnUpdate);
                action?.Invoke();
            }
        }


        return re;
    }

    ///<summary>延迟运行, 等待下一帧执行操作, !!编辑器使用!!</summary>
    public static void WaitUpdate_InEditor(Action action, int updateCount = 1)
    {
        int count = 0;
        UnityEditor.EditorApplication.update += MyEditorUpdateFunction;


        void MyEditorUpdateFunction()
        {

            count++;
            if (count >= updateCount)
            {
                action?.Invoke();
                UnityEditor.EditorApplication.update -= MyEditorUpdateFunction;
            }
        }


    }

}




