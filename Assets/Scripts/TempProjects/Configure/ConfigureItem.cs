using System;
using System.Collections.Generic;
using System.Linq;
using CoreClass;
using Sirenix.OdinInspector;
using UnityEngine;




//命名空间：配置
namespace Configure
{
    [System.Serializable]
    public abstract partial class ConfigureItem
    {
        #region //&界面部分




        //*界面:启动按钮
        [FoldoutGroup("配置属性", false)]
        [GUIColor(nameof(buttonColor))]
        [Button("@buttonName", ButtonHeight = 44)]

        [HorizontalGroup("配置属性/h")]
        [ButtonGroup("配置属性/h/1")]
        [PropertyOrder(-1)]
        // [GUIColor(nameof(buttonColor))]
        private void EnableButton()
        {
            启用配置 = !启用配置;
            OnValidate();
        }
        private string buttonName => 启用配置 ? "配置启用" : "配置停用";
        private Color buttonColor => 启用配置 ? Color.green : Color.yellow;


        //*界面:脚本引用
        [SerializeField]
        [ReadOnly]
        [BoxGroup("配置属性/h/2", false)]
        [LabelWidth(60)]
        private UnityEditor.MonoScript 脚本文件;
        //私有方法:设置脚本引用
        private void SetScriptRefer()
        {
            //有则返回
            if (脚本文件 != null)
                return;

            Type type = this.GetType();
            //在unity资源管理器里找到这个脚本文件
            string guid = UnityEditor.AssetDatabase.FindAssets($"t:MonoScript {type.Name}").FirstOrDefault();
            //把guid转换成路径
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            //把路径转换成脚本
            UnityEditor.MonoScript script = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(path);
            脚本文件 = script;
        }



        //*界面:配置类型选择,用来给界面标题显示用
        [BoxGroup("配置属性/h/2")]
        [OnValueChanged(nameof(OnTitleChanged))]
        [LabelWidth(60)]
        public string 显示标题 = "";
        private void OnTitleChanged()
        {
            //如果空了
            if (string.IsNullOrEmpty(显示标题))
            {
                CoreF.NameTypeDict.TryGetKey(this.GetType(), out 显示标题);
            }
        }



        private bool 启用配置 = true;


        //*界面:配置停用时的提示
        [InfoBox("配置已经停用", InfoMessageType.Warning, "@!" + nameof(启用配置))]
        [GUIColor(nameof(color))]
        [HideLabel]
        public PlaceHolder placeHolder;
        [Serializable] public class PlaceHolder { }
        private Color color = Color.yellow;





        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        ///<summary> 配置项目启动接口 </summary>
        public ConfigureRunner CreateRunner(MonoBehaviour monoBehaviour)
        {
            //~从下列几种方法中选择一个创建运行器



            if (this is ConfigureItemBase itemBase)
            {
                ConfigureItemBase.ItemRunnerBase itemRunnerBase = itemBase.CreateRunnerOver(monoBehaviour.gameObject);
                return new ConfigureRunner(itemRunnerBase.OnInit, itemRunnerBase.OnEnable, itemRunnerBase.OnDisable, itemRunnerBase.OnUnInit);
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
        ///<summary> 配置项目被编辑时候运行 </summary>
        public void OnValidate()
        {
            SetScriptRefer();


            autoEnabler?.Update();

        }


        public bool Enabled => 启用配置;

        ///<summary> 必要组件, 无重复 </summary>
        public List<System.Type> RequiredTypes
        {
            get
            {
                //如果是 ConfigureItemBaseAddition 的子类
                if (this is ConfigureItemBase addition)
                {

                    //添加必要组件
                    requiredTypes.AddRangeNotNull(addition.RequireComponentsOnGameObject);
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




    //*接口:配置项目启动接口
    public abstract partial class ConfigureItem : IGetter<CoreClass.AutoEnabler>
    {

        private CoreClass.AutoEnabler _autoEnabler;
        private CoreClass.AutoEnabler autoEnabler
        {
            get
            {
                if (_autoEnabler == null)
                {
                    _autoEnabler = new();
                    _autoEnabler.AccessorEnabled = () => 启用配置;

                }
                return _autoEnabler;
            }
        }

        AutoEnabler IGetter<AutoEnabler>.Get() => autoEnabler;
    }










}



