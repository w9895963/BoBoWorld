using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraAction_Follow_Conf", menuName = "ScriptableObjects/Camera/CameraAction_Follow_Conf")]
public class CameraAction_Follow_Conf : ScriptableObject
{
    public Vector2 目标偏移 = new Vector2(0, 3);
    public Vector2 按键偏移 = new Vector2(5.5f, 2.5f);
    public float 跟踪过度时间 = 0.4f;
    public AnimationCurve 横轴距离速度曲线 = Curve.ZeroOneSmooth03;
    public Vector2 横轴距离范围 = new Vector2(0, 15);
    public Vector2 横轴速度范围 = new Vector2(0, 40);
    public AnimationCurve 纵轴距离速度曲线 = Curve.ZeroOneSmooth03;
    public Vector2 纵轴距离范围 = new Vector2(0, 3);
    public Vector2 纵轴速度范围 = new Vector2(0, 20);

}
