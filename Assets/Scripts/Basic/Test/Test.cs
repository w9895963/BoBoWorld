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
        EventData.EventDataUtil.EventData eventData = new EventData.EventDataUtil.EventData();
        EventData.EventDataUtil.EventData<Vector2> eventData2 = new EventData.EventDataUtil.EventData<Vector2>();
        eventData.GetType().FullName.Log();
        eventData2.GetType().FullName.Log();
        eventData = eventData2;
        eventData.GetType().FullName.Log();
        Debug.Log(eventData.GetType() == typeof(EventData.EventDataUtil.EventData<Vector2>));

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
