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
            List<string> types = configList.SelectMany(x => x.配置文件).SelectMany(x => x.requiredTypes.Where(y => gameObject.GetComponent(y) == null)).
            Select(x => x.ToString()).ToList();

            requiredTypes = types.Count == 0 ? "无" : string.Join("\n", types);

        }
        [NaughtyAttributes.Label("缺失组件")]
        [ReadOnly]
        [ResizableTextArea]
        public string requiredTypes = "无";

        //*按钮:更新配置
        [Button("热更新配置")]
        [Tooltip("修改内容后,可以用这个按钮更新配置")]
        private void UpdateConfigures()
        {
            enabled = false;
            UpdateEnablers();
            enabled = true;
        }


        //*配置列表
        [NaughtyAttributes.Label("配置列表")]
        [OneLine.ReadOnlyExpandable]

        public List<ConfigureItemManager> configList = new List<ConfigureItemManager>();



        //字典:配置启用器字典
        private Dictionary<ConfigureBase, (Action Enable, Action Disable)> enablers = new Dictionary<ConfigureBase, (Action Enable, Action Disable)>();
        //列表:配置启用器列表
        private List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>();






        //*公共方法:更新配置启用器字典
        public void UpdateEnablers()
        {
            enablerList = configList.SelectMany(x => x.配置文件).Select(x => x.CreateEnabler(gameObject)).ToList();

            List<ConfigureBase> configures = configList.SelectMany(x => x.配置文件).ToList();
            //~添加新的配置启用器
            foreach (var item in configures)
            {
                //若配置启用器字典中不包含配置
                if (!enablers.ContainsKey(item))
                {
                    //创建启用器
                    var en = item.CreateEnabler(gameObject);
                    //配置启用器字典添加配置
                    enablers.Add(item, en);
                }
            }
            //~删除配置中不包含的启用器
            foreach (var item in enablers.Keys.ToList())
            {
                if (!configures.Contains(item))
                {
                    enablers.Remove(item);
                }
            }



        }



        //*Unity事件
        //苏醒
        void Awake()
        {
            UpdateEnablers();
        }
        void OnEnable()
             => enablerList.ForEach(x => x.Enable?.Invoke());

        void OnDisable()
            => enablerList.ForEach(x => x.Disable?.Invoke());







    }
}