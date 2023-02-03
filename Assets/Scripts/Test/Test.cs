using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Test : MonoBehaviour
{


    public Configure.Inspector.ConditionTrigger conditionTrigger = new Configure.Inspector.ConditionTrigger();


    public Configure.Inspector.ConditionTriggerList conditionTriggers = new Configure.Inspector.ConditionTriggerList();



    private void Start()
    {

    }




    public void 本地测试方法()
    {
        conditionTrigger.GetCondition(gameObject).Log("conditionTrigger");
        

    }


    public static void 测试方法()
    {

    }



    public static class TestMethodInMenu
    {
        [UnityEditor.MenuItem("测试/测试方法")]
        public static void 测试方法()
        {
            GameObject.FindObjectsOfType<Test>().ForEach(obj => obj.本地测试方法());
            Test.测试方法();
        }
    }

}
