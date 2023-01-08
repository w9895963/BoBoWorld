using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        List<int> a = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 2 };
        Dictionary<int, int> b = new() { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 }, { 5, 5 }, { 6, 6 }, { 7, 7 }, { 8, 8 }, { 9, 9 }, { 10, 10 } };

        //去重,自定义方法
        a.LogSmart();

        b.LogSmart();

    }





}
