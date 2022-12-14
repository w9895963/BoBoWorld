using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using Microsoft.CSharp;
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
        new TestClass2();
    }
    public void TestMethod2(ref Vector2 obj)
    {
        Type type = obj.GetType();
        type.Name.Log();

    }
    public class TestClass
    {
        public TestClass()
        {
            Debug.Log("TestClass");
        }
    }

    public class TestClass2:TestClass
    {
        public TestClass2()
        {
            Debug.Log("TestClass2");
        }
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
