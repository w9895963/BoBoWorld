using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorToolbox;
using Microsoft.CSharp;
using StackableDecorator;
using UnityEngine;

//命名空间：配置
namespace Configure
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
    //可脚本化对象
    public class ConfigureItemManager : ScriptableObject
    {




        public List<ConfigureBase> 配置文件 = new List<ConfigureBase>();





        [EditorToolbox.ReorderableList]
        [SerializeReference]
        public List<ConfigureBase_> 配置文件_ = new List<ConfigureBase_>();





        private void OnListChanged()
        {
            //~将列表里所有的null替换为新的实例
            配置文件_ = 配置文件_.Select((x) => x ?? new ConfigureBase_()).ToList();



            //~将列表里所有的项目替换成对应的类型
            Dictionary<string, Type> replaceSelectionInfoDict = ConfigureBase_.ReplaceSelectionInfoDict;
            //找到类型与选择类型不相同的项目
            IEnumerable<ConfigureBase_> notSameType = 配置文件_.Where((x) => x.displaceTypeName != x.configTypeSelection);
            notSameType.ToArray().ForEach((x) =>
            {

                if (replaceSelectionInfoDict.TryGetValue(x.configTypeSelection, out Type newType))
                {
                    if (newType != null)
                    {
                        //实例化newType
                        ConfigureBase_ configureBase_ = (ConfigureBase_)Activator.CreateInstance(newType);
                        configureBase_.configTypeSelection = x.configTypeSelection;
                        //替换
                        配置文件_[配置文件_.IndexOf(x)] = configureBase_;



                    }
                }


            });
        }












        //*在编辑器中运行

        //方法:热更新
        public void HotUpdate()
        {
            //如果游戏不在运行则返回且报错
            if (!Application.isPlaying)
            {
                return;
            }


            ActionF.RunActionSafeAndDelay(() =>
            {
                //找到启用且有自身的组件
                var components2 = FindObjectsOfType<ConfigureBuilderMono>().Where(x => x.enabled & x.configList.Contains(this));
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
            OnListChanged();
        }

    }



















}

