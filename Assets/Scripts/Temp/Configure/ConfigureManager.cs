using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CSharp;
using NaughtyAttributes;
using UnityEngine;


//命名空间：配置
namespace ConfigureS
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
    //可脚本化对象
    public class ConfigureManager : ScriptableObject
    {
        [Expandable]
        //配置文件列表
        public List<ConfigureBase> ConfigObjects = new List<ConfigureBase>();
    }









}

