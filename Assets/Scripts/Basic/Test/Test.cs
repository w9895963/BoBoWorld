using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Timer.Base;
using UnityEngine;
using static CommonFunction.Static;

public class Test : MonoBehaviour
{
    public Object obj;
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

    }

    private void Start()
    {
        EventData.EventDataSetter<Vector2> move = new EventData.EventDataSetter<Vector2>(EventDataName.Input.移动);
        EventData.ConditionsSetter testEventData = new EventData.ConditionsSetter();
        testEventData.SetAction(() => { Debug.Log(move.GetData()); });
        testEventData.AddCondition(move.OnDataUpdate);
        testEventData.Enable();
    }
}
