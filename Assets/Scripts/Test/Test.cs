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
    public void 打印到控制台()
    {
    }



    private void Start()
    {

    }









    //测试方法1
    [NaughtyAttributes.Button]
    public void 测试方法1()
    {
        (int one, int two) test = (1, 2);


        Action<(int, int)> action = (d) =>
        {
            Debug.Log(d.Item1);
            Debug.Log(d.Item2);
        };

        action(test);

    }








}
