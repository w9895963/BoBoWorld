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
        //历遍  EventDataName.Player
        foreach (var item in Enum.GetValues(typeof(EventDataName.Player)))
        {
            Debug.Log(item);
            Debug.Log(EventDataName.Player.重力大小);
            //判断item是否等于EventDataName.Player.重力大小
            if (item.Equals(EventDataName.Player.重力大小))
            {
                Debug.Log("相等");
            }
            else
            {
                Debug.Log("不相等");
            }

        }
    }


    private void Start()
    {

    }
}
