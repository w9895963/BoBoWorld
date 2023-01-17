using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    //*Unity界面
    public UnityEngine.Object obj;
    public UnityEditor.MonoScript monoScript;
    public GameObject gameObj;
    public string EditorButton2;
    public void 打印到控制台()
    {
        obj.GetType().Log();
        Debug.Log("obj = " + obj);
        obj.Log("obj");
        gameObj.Log("gameObj");
    }



    private void Start()
    {

    }









    //*测试方法1
    public string EditorButton;
    public void 测试方法1()
    {
        //生成一个随机数然后打印到unix的的控制台。
        typeof(string).Log();
        // SpriteShape



    }








}
