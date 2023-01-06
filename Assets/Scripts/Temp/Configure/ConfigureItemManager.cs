using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
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
        [Expandable]
        //配置文件列表
        public List<ConfigureBase> 配置文件 = new List<ConfigureBase>();


        [SerializeReference, SubclassSelector]
        public List<ConfigureBase_> 配置文件_ = new List<ConfigureBase_>();







        [Button("生成配置文件")]
        public void 生成配置文件()
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }

    }











}

