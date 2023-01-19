using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Configure.ConfigureItem;
using UnityEngine;
using Type = System.Type;


//命名空间：配置
namespace Configure
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
    //可脚本化对象
    public class ConfigureItemManager : ScriptableObject
    {
        //^界面:选择类型并添加

        [StackableDecorator.DropdownValue("#" + nameof(configureTypes))]
        [StackableDecorator.StackableField]
        public string 添加配置 = addTypeDefault;
        private const string addTypeDefault = "选择配置类型";
        public string[] configureTypes => configureTypeDict.Keys.ToArray();
        public static Dictionary<string, Type> configureTypeDict = new Dictionary<string, Type>() {
            { "选择配置类型",null},
            { "物理/力量施加器", typeof(ConfigureItem_ApplyForce)},
            { "物理/获取物理量",typeof(ConfigureItem_GetPhysicData)},
            { "物理/地面检测器",typeof(ConfigureItem_GroundFinder)},
            { "物理/计算行走施力",typeof(ConfigureItem_WalkFore)},
        };
        private void OnValueChanged_addType()
        {
            if (configureTypeDict.TryGetValue(添加配置, out Type type))
            {
                if (type == null)
                    return;
                ConfigureBase item = (ConfigureBase)System.Activator.CreateInstance(type);
                item.configureType = 添加配置;
                item.OnCreate();
                配置文件列表.Add(item);
                添加配置 = addTypeDefault;
            }
        }




        //^界面:配置文件列表
        [SerializeReference]
        public List<ConfigureBase> 配置文件列表 = new List<ConfigureBase>();






        //方法:热更新
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
            OnValueChanged_addType();
        }











    }



















}

