using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timer.Base;
using UnityEngine;
using static CommonFunction.Static;

public class Test : MonoBehaviour
{
    public UnityEngine.Object obj;
    public GameObject gameObj;
    private SimpleTimer simpleTimer;


    [ContextMenu("LogObject")]
    public void LogObj()
    {
        Debug.Log(obj);

    }
    [ContextMenu("Test")]
    public void TestMethod()
    {
        EventDataName.Player.重力大小.GetType().FullName.Log();
        EventDataName.Player.重力大小.Log();


    }


    private void Start()
    {
        BasicEvent.OnUpdate.Add(gameObject, NewMethod);
        BasicEvent.OnUpdate.Add(gameObject, NewMethod);
        BasicEvent.OnUpdate.Remove(gameObject, NewMethod);
        BasicEvent.OnUpdate.Remove(gameObject, NewMethod);


        static void NewMethod()
        {

            //log 时间
            Debug.Log(Time.time);

        }
    }
}
