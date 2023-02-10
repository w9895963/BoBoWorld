using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Type = System.Type;


//命名空间：配置
namespace Configure
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "配置/配置管理器", order = 0)]
    //可脚本化对象
    public partial class ConfigureItemManager : ScriptableObject
    {
        //^界面:选择类型并添加
        [ValueDropdown(nameof(configureTypes))]
        [OnValueChanged(nameof(OnValueChanged_addType))]
        public string 添加配置 = addTypeDefault;
        private const string addTypeDefault = "选择配置类型";
        private string[] configureTypes => configureTypeDict.Keys.ToArray();
        private static Dictionary<string, Type> configureTypeDict => ConfigureCoreF.NameTypeDict;
        private void OnValueChanged_addType()
        {

            if (configureTypeDict.TryGetValue(添加配置, out Type type))
            {

                if (type == null)
                    return;
                ConfigureItem item = (ConfigureItem)System.Activator.CreateInstance(type);
                item.显示标题 = 添加配置;
                item.OnAfterCreate();
                配置文件列表.Add(item);

                TimerF.WaitUpdate_InEditor(() =>
                {
                    添加配置 = addTypeDefault;
                }, 50);

            }

        }




        //^界面:配置项目列表
        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(HideAddButton = true, ListElementLabelName = nameof(ConfigureItem.显示标题), DraggableItems = false, Expanded = true)]
        [Space]
        public List<ConfigureItem> 配置文件列表 = new List<ConfigureItem>();






        ///<summary> 方法:热更新,对象是所有启用且有自身的组件 </summary>
        public void HotUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }


            ActionF.RunActionSafeAndDelay(() =>
            {
                //找到启用且有自身的组件
                var components2 = FindObjectsOfType<ConfigureBuilderMono>().Where(x => x.enabled & x.配置列表.Contains(this));
                components2.ForEach(x =>
                {
                    x.UpdateRunners();
                });
            });
        }



        //改动
        public void OnValidate()
        {
            HotUpdate();
            Debug.Log("配置管理器改动");



            配置文件列表.ForEach(x =>
            {
                x?.OnValidate();
            });
        }











    }







    public partial class ConfigureItemManager : IConfigureRunnerBuilders
    {
        IConfigureRunnerBuilder[] IConfigureRunnerBuilders.RunnerBuilders => 配置文件列表.SelectNotNull(x => x as IConfigureRunnerBuilder).ToArray();
    }




    public interface IConfigureRunnerManager
    {
        void AddRunner(ClassCore.IRunner runner);
        void RemoveRunner(ClassCore.IRunner runner);
        void Initialize();
        void Destroy();
        void Enable();
        void Disable();
    }




    public interface IConfigureRunnerBuilders
    {
        IConfigureRunnerBuilder[] RunnerBuilders { get; }
    }


    public interface IConfigureRunnerBuilder
    {
        ClassCore.IRunnerConfig CreateRunnerConfig(MonoBehaviour mono);
    }





    public interface IConfigItemInfo
    {

        public string MenuName { get; }

        public ConfigItemInfo OptionalInfo { get; }


        public class ConfigItemInfo
        {

        }
    }



}
