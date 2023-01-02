using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;



//命名空间：配置
namespace Configure
{
    //属性:组件名,配置安装器
    [AddComponentMenu("配置/配置安装器")]
    public class ConfigureBuilderMono : MonoBehaviour
    {
        //*检查缺失组件
        [Button("检查缺失组件")]
        private void CheckRequiredTypes()
        {
            //~缺失组件
            List<string> types = 配置表.配置文件.SelectMany(x => x.requiredTypes.Where(y => gameObject.GetComponent(y) == null)).
            Select(x => x.ToString()).ToList();

            requiredTypes = types.Count == 0 ? "无" : string.Join("\n", types);

        }
        [Label("缺失组件")]
        [ReadOnly]
        [ResizableTextArea]
        public string requiredTypes = "无";


        //*配置管理器
        [Expandable]
        public ConfigureItemManager 配置表;
        //启用器
        public (Action Enable, Action Disable) enabler = (null, null);

        //苏醒
        void Awake()
        {
            //获得所有配置
            var configures = 配置表.配置文件;
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


        void Reset()
        {
            CheckRequiredTypes();
        }

        void OnEnable()
       => enabler.Enable?.Invoke();

        void OnDisable()
       => enabler.Disable?.Invoke();


    }
}