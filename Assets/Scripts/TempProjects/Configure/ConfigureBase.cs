using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorToolbox;
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
    public class ConfigureBase_
    {
        //*Inspector界面:配置选择
        [Preset(nameof(replaceSelectionOption))]
        [EditorToolbox.NewLabel("配置类型")]
        public string configTypeSelection = displaceTypeNameDefault;
        public const string displaceTypeNameDefault = "无配置";
        [HideInInspector]
        public string displaceTypeName = displaceTypeNameDefault;


        private static (Type type, string name)[] replaceSelectionInfo;
        public static (Type type, string name)[] ReplaceSelectionInfo => Fc.FieldGetOrSet(ref replaceSelectionInfo, GetReplaceSelectionInfo);
        private static (Type type, string name)[] GetReplaceSelectionInfo()
        {
            (Type type, string name)[] values = Assembly.GetAssembly(typeof(ConfigureBase_)).GetTypes().Where((x) => x.IsSubclassOf(typeof(ConfigureBase_)))
                .Select((x) =>
                    (type: x,
                    name: (Activator.CreateInstance(x) as ConfigureBase_).displaceTypeName))

                .Where((x) => x.name != displaceTypeNameDefault)

                .ToArray();

            Type displaceType = typeof(ConfigureBase_);
            //实例化 displaceType
            ConfigureBase_ displaceInstance = (Activator.CreateInstance(displaceType) as ConfigureBase_);
            return values;
        }

        public static Dictionary<string, Type> ReplaceSelectionInfoDict => Fc.FieldGetOrSet(ref replaceSelectionInfoDict, GetReplaceSelectionInfoDict);
        private static Dictionary<string, Type> replaceSelectionInfoDict;
        private static Dictionary<string, Type> GetReplaceSelectionInfoDict()
        {
            Dictionary<string, Type> re = new Dictionary<string, Type>();

            (Type type, string name)[] values = Assembly.GetAssembly(typeof(ConfigureBase_)).GetTypes().Where((x) => x.IsSubclassOf(typeof(ConfigureBase_)))
                .Select((x) =>
                    (type: x,
                    name: (Activator.CreateInstance(x) as ConfigureBase_).displaceTypeName))

                .Where((x) => x.name != displaceTypeNameDefault)

                .ToArray();

            values.ForEach((x) => re.Add(x.name, x.type));

            return re;
        }


        public string[] replaceSelectionOption => ReplaceSelectionInfo.Select((x) => x.name).ToArray();









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
        private Action<ConfigureBase_> replaceSelfWithAction;

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

