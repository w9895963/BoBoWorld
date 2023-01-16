using System;
using EditorToolbox;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    //*Unity界面

    public UnityEngine.Object obj;

    public GameObject gameObj;














    [EditorButton(nameof(测试方法1))]
    [Hide]
    public int 测试方法1_;


    //*测试方法1
    public void 测试方法1()
    {
        //生成一个随机数然后打印到unix的的控制台。
        string v = nameof(测试方法1);
        v.Log();
        // SpriteShape



    }







}
