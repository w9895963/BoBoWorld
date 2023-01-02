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
        TestClass<Action<string>> testClass = new TestClass<Action<string>>();
        testClass.obj = (s) => { Debug.Log(1); };
        var obj = testClass.obj;
        obj+= (s) => { Debug.Log(s); };
        testClass.obj = default;
        testClass.obj.Invoke("123");
    }
    public void TestMethod2(ref Vector2 obj)
    {
        Type type = obj.GetType();
        type.Name.Log();
    }

    public class TestClass<T>
    {
        public T obj;
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
