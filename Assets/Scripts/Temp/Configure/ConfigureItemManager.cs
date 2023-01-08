using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;
using NaughtyAttributes;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
    //可脚本化对象
    public class ConfigureItemManager : ScriptableObject
    {



        // [NaughtyAttributes.ReorderableList]
        //配置文件列表
        [Expandable]
        public List<ConfigureBase> 配置文件 = new List<ConfigureBase>();




        [SerializeReference]
        [SubclassSelector]
        [InspectorName("配置文件列表")]
        public List<ConfigureBase_> 配置文件_ = new List<ConfigureBase_>();




















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
        }

    }



















}

