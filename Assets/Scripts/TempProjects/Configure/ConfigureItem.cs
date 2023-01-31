using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;



//命名空间：配置
namespace Configure
{
    [System.Serializable]
    public abstract class ConfigureItem
    {
        #region //&界面部分



        //*界面:配置类型选择,用来给界面标题显示用
        // [HideInInspector]
        [NaughtyAttributes.AllowNesting]
        [NaughtyAttributes.Label("标题")]
        public string insLabelConfigureType = "未选择配置类型";



        //*界面:脚本引用
        [SerializeField]
        [StackableDecorator.EnableIf(false)]
        [StackableDecorator.Label(title = "Script")]
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


        ///<summary> 配置项目启动接口 </summary>
        public ConfigureRunner CreateRunner(MonoBehaviour monoBehaviour)
        {
            //~从下列几种方法中选择一个创建运行器


            if (this is ConfigureItemBase)
            {
                Type runnerType;
                runnerType = this.GetType().GetNestedTypes()
                // .Log("GetNestedTypes")//Test
                .FirstOrDefault(t => t.IsSubclassOf(typeof(ConfigureItemBase.ItemRunnerBase)));
                // runnerType.Log();//Test

                var obj = Activator.CreateInstance(runnerType);
                runnerType.GetField(nameof(ConfigureItemBase.ItemRunnerBase.config)).SetValue(obj, this);
                runnerType.GetField(nameof(ConfigureItemBase.ItemRunnerBase.gameObject)).SetValue(obj, monoBehaviour.gameObject);


                IConfigureItemRunner r = (IConfigureItemRunner)obj;


                return new ConfigureRunner(r.Init, r.Enable, r.Disable, r.Destroy);
            }










            if (createRunnerFunc != null)
            {
                return createRunnerFunc.Invoke(monoBehaviour.gameObject);
            }





            //都不满足
            return null;
        }

        ///<summary> 配置项目启动接口 </summary>
        public void OnAfterCreate()
        {
            //设置脚本引用
            SetScriptRefer();
            onAfterCreate?.Invoke();
        }



        public bool Enabled => interfaceEnabled;

        ///<summary> 必要组件, 无重复 </summary>
        public List<System.Type> RequiredTypes
        {
            get
            {
                //如果是 ConfigureItemBaseAddition 的子类
                if (this is ConfigureItemBase addition)
                {

                    //添加必要组件
                    requiredTypes.AddRangeNotNull(addition.RequireComponents);
                    //除重
                    requiredTypes = requiredTypes.Distinct().ToList();
                }

                return requiredTypes;
            }
        }

        //创建运行器



        protected List<System.Type> requiredTypes = new List<System.Type>();
        protected Action onAfterCreate;
        protected System.Func<GameObject, ConfigureRunner> createRunnerFunc;


        //用来为生成运行器生成委托
        protected void CreateRunnerFunc<R, C>() where R : ConfigureRunnerT<C>, new() where C : ConfigureItem, new()
        {
            createRunnerFunc = (gameObject) =>
            {
                R runner = new R();
                runner.gameObject = gameObject;
                runner.config = this as C;
                return runner;
            };
        }


    }






}



