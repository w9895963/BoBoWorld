using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public partial class Test : MonoBehaviour
{


    //start
    private void Start()
    {
    }


    private void Reset()
    {

    }


    private void OnValidate()
    {

    }










    public void 本地测试方法()
    {

    }


    public static void 测试方法()
    {


    }





}





public partial class Test : MonoBehaviour
{
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
