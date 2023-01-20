using System;
using System.Collections.Generic;
using System.Linq;
using Configure.ConfigureItem;

using UnityEngine;



//命名空间：配置
namespace Configure
{


    //类:配置基类
    [System.Serializable]
    public class ConfigureBase
    {
        #region //&界面部分



        //*界面:配置类型选择,用来给界面标题显示用
        [HideInInspector]
        public string insLabelConfigureType = "未选择配置类型";



        //*界面:脚本引用
        [SerializeField]
        [StackableDecorator.EnableIf(false)]
        [StackableDecorator.StackableField]
        private UnityEditor.MonoScript scriptRefer;
        //私有方法:设置脚本引用
        private void SetScriptRefer()
        {
            Type type = this.GetType();
            //在unity资源管理器里找到这个脚本文件
            string guid = UnityEditor.AssetDatabase.FindAssets($"t:MonoScript {type.Name}").FirstOrDefault();
            //把guid转换成路径
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            //把路径转换成脚本
            UnityEditor.MonoScript script = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(path);
            scriptRefer = script;
        }








        //*界面:启用配置
        [StackableDecorator.HelpBox("配置已经启用", "$interfaceEnabled", messageType = 0)]
        [StackableDecorator.Box(4, 4, 4, 4)]
        [StackableDecorator.Label(title = "启用配置")]
        [StackableDecorator.ToggleLeft]
        [StackableDecorator.StackableField]

        [SerializeField]
        private bool interfaceEnabled = true;





        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑








        public void OnCreate()
        {
            SetScriptRefer();
        }



        public bool Enabled => interfaceEnabled;


        //必要组件
        public List<System.Type> RequiredTypes => requiredTypes;
        public ConfigureRunner CreateRunner(MonoBehaviour monoBehaviour)
        {
            if (createRunner != null)
            {
                return createRunner.Invoke(monoBehaviour.gameObject);
            }
            //都不满足
            return null;
        }


        protected List<System.Type> requiredTypes = new List<System.Type>();
        protected System.Func<GameObject, ConfigureRunner> createRunner;





    }







}

