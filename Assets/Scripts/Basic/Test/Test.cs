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
        Vector2 v2 = new Vector2(0, 2);
        Vector2 v3 = v2.Rotate(90);
        Debug.Log(v3);

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
