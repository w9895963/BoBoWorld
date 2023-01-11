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
    //*Unity界面
    public UnityEngine.Object obj;
    public GameObject gameObj;


    [SerializeReference, SubclassSelector]
    public TestClass test = new TestClass3();













    public void TestMethod2(ref Vector2 obj)
    {
        Type type = obj.GetType();
        type.Name.Log();
    }

    [Serializable]
    public class TestClass
    {
        public int a;
        public int b;
        public int c;
    }

    public class TestClass2<T> : TestClass
    {
        public T d;
    }

    [Serializable]
    public class TestClass3 : TestClass2<string>
    {

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
        EventData.DataNameF.GetType(EventData.DataName.是否站在地面).Log("类型");
        /// <summary>
        /// 测试方法1
        /// </summary>

        test = new TestClass2<int>();
        test = new TestClass2<int>();
        test = new TestClass2<int>();
        // test = new TestClass2<int>();
        test = new TestClass2<int>();
        test = new TestClass2<int>();

    }







}
