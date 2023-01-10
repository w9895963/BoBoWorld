using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;
using Microsoft.CSharp;
using NaughtyAttributes;
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

    public void TestMethod2(ref Vector2 obj)
    {
        Type type = obj.GetType();
        type.Name.LogSmart();
    }

    public class TestClass
    {
        public int a;
        public int b;
        public int c;
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




    //*测试方法1
    [Button]
    public void 测试方法1()
    {
        List<int> list = new List<int>();
        int[] array = new int[10];
        var f = array.GetType().GetTypeInfo();


        this.GetType().GetTypeInfo().GetField("gameObj").GetValue(this).LogSmart();





    }







}
