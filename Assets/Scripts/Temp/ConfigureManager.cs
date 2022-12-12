using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
//可脚本化对象
public class ConfigureManager : ScriptableObject
{
    [Expandable]
    //功能启用器
    public List<ConfigureBase> functionEnablers = new List<ConfigureBase>();




}



//类:配置基类
public class ConfigureBase : ScriptableObject
{
    public virtual void Enable()
    {

    }
    public virtual void Disable()
    {

    }

}


//类：配置项目,可脚本化对象
[CreateAssetMenu(fileName = "行走配置", menuName = "动态配置/行走配置", order = 1)]
public class WalkConfig : ConfigureBase
{
    //行走速度
    public float 行走速度 = 10;

}



