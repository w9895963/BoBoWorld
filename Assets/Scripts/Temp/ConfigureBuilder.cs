using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ConfigureBuilder : MonoBehaviour
{
    [Expandable]
    //配置管理器
    public ConfigureManager configureManager;

    void Start()
    {
        //如果配置管理器不为空
        if (configureManager != null)
        {   //遍历功能启用器
            foreach (var item in configureManager.functionEnablers)
            {   //启用
                item.Enable();
            }
        }
    }

    void OnDisable()
    {
        //如果配置管理器不为空
        if (configureManager != null)
        {   //遍历功能启用器
            foreach (var item in configureManager.functionEnablers)
            {   //禁用
                item.Disable();
            }
        }
    }
}
