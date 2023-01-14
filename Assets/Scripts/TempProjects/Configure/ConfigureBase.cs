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
        protected virtual List<System.Type> requiredTypes => new List<System.Type>();

        public virtual List<System.Type> RequiredTypes => requiredTypes;
        //必要组件






        public virtual (Action Enable, Action Disable) CreateEnabler(GameObject gameObject, MonoBehaviour monoBehaviour = null)
        {
            return (null, null);
        }


        //



    }




    //类:配置基类
    [System.Serializable]
    [AddTypeMenu("")]
    public class ConfigureBase_
    {
        //*Inspector界面元素
        [HelpBox("配置已经启用", "$interfaceEnabled", messageType = 0)]
        [StackableDecorator.Box(4, 4, 4, 4)]
        [StackableDecorator.Label(title = "启用配置")]
        [StackableDecorator.ToggleLeft]
        [StackableDecorator.IndentLevel(-1)]
        [StackableDecorator.StackableField]
        [SerializeField]
        private bool interfaceEnabled = true;













        public bool Enabled => interfaceEnabled;


        //必要组件
        protected virtual List<System.Type> requiredTypes => new List<System.Type>();
        public virtual List<System.Type> RequiredTypes => requiredTypes;

        protected System.Func<GameObject, ConfigureRunner> createRunner;
        private Func<GameObject, ConfigureBase_, ConfigureRunner> createRunnerAction;

        public ConfigureBase_()
        {
        }
        public ConfigureBase_(Func<GameObject, ConfigureBase_, ConfigureRunner> createRunnerAction)
        {
            this.createRunnerAction = createRunnerAction;
        }

        public virtual ConfigureRunner CreateRunner(GameObject gameObject, MonoBehaviour monoBehaviour)
        {
            if (createRunner != null)
            {
                return createRunner.Invoke(gameObject);
            }
            if (createRunnerAction != null)
            {
                return createRunnerAction.Invoke(gameObject, this);
            }
            return null;
        }







    }





}

