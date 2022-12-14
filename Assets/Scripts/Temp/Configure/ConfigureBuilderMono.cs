using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;



//命名空间：配置
namespace ConfigureS
{

    public class ConfigureBuilderMono : MonoBehaviour
    {
        [Expandable]
        //配置管理器
        public ConfigureManager configureManager;
        //启用器
        public (Action Enable, Action Disable) enabler = (null, null);

        //苏醒
        void Awake()
        {
            //获得所有配置
            var configures = configureManager.ConfigObjects;
            //历遍配置
            foreach (var item in configures)
            {
                //创建启用器
                var en = item.CreateEnabler(gameObject);
                //启用器添加事件
                enabler.Enable += en.Enable;
                enabler.Disable += en.Disable;
            }
        }

        void OnEnable()
       => enabler.Enable?.Invoke();

        void OnDisable()
       => enabler.Disable?.Invoke();


    }
}