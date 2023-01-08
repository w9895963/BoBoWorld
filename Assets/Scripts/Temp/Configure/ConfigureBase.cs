using System;
using System.Collections.Generic;
using StackableDecorator;
using UnityEngine;



//命名空间：配置
namespace Configure
{
    //类:配置基类
    public class ConfigureBase : ScriptableObject
    {
        //字段:启用器
        public virtual List<System.Type> requiredTypes => new List<System.Type>();
        //必要组件






        public virtual (Action Enable, Action Disable) CreateEnabler(GameObject gameObject, MonoBehaviour monoBehaviour = null)
        {
            return (null, null);
        }
       





    }




    //类:配置基类
    [System.Serializable]
    [AddTypeMenu("")]
    public class ConfigureBase_
    {
        [HelpBox("配置已经启用", "$interfaceEnabled", messageType = 0)]
        [StackableDecorator.Box(4, 4, 4, 4)]
        [StackableDecorator.Label(title = "启用配置")]
        [StackableDecorator.ToggleLeft]
        [StackableDecorator.IndentLevel(-1)]
        [StackableDecorator.StackableField]
        [SerializeField]
        private bool interfaceEnabled = true;












        //字段:启用器
        public (Action Enable, Action Disable) enabler = (null, null);
        public bool Enabled => interfaceEnabled;


        //必要组件
        public virtual List<System.Type> requiredTypes => new List<System.Type>();



        public virtual (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
        {
            return (null, null);
        }

        public virtual ConfigureRunner CreateRunner(GameObject gameObject)
        {
            return null;
        }







    }





}

