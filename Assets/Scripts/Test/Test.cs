using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Test : MonoBehaviour
{


    public Configure.Inspector.OutDataInspector<Vector2> outData = new();




    private void OnValidate()
    {
        Debug.Log("OnValidate");
        outData.OnValidate();
        outData.Enabled=enabled;
    }



    //unity事件:OnEnable
    private void OnEnable()
    {
        Debug.Log("OnEnable");
        outData.Enabled = true;
    }
    private void OnDisable()
    {
        outData.Enabled = false;
    }




    public void 本地测试方法()
    {
        EventData.DataName.CustomDataName.AllNames.Log();


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
