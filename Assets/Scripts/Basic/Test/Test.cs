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
       
    }
}
