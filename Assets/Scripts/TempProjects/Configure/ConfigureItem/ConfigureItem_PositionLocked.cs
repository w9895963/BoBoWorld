using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Interface;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItem
{




    [System.Serializable]
    public class ConfigureItem_PositionLocked : ConfigureItemBase
    {

        [SerializeField]
        [StackableDecorator.HorizontalGroup("info1", true, "", 0, prefix = true, title = "锁定目标", tooltip = "将自身位置锁定到目标位置")]
        private DataHolder_NameDropDown<Vector2> 锁定目标;
        public string lockTargetDataName => 锁定目标.dataName;







        //脚本说明
        public Interface.ShowOnlyText 说明 = new Interface.ShowOnlyText("将自身位置锁定到目标位置");







        public ConfigureItem_PositionLocked()
        {
            createRunner = CreateRunner;
        }


        private ConfigureRunner CreateRunner(GameObject gameObject)
        {
            Runner runner = new Runner(gameObject, this);
            return new ConfigureRunner(runner.Initialize, runner.Enable, runner.Disable, runner.Destroy);

        }


        private class Runner
        {
            private GameObject gameObject;
            private ConfigureItem_PositionLocked cf;
            private EventDataHandler<Vector2> lockTargetD;

            public Runner(GameObject gameObject, ConfigureItem_PositionLocked cf)
            {
                this.gameObject = gameObject;
                this.cf = cf;
                lockTargetD = EventDataF.GetData<Vector2>(cf.lockTargetDataName, gameObject);
            }



            public void Initialize()
            {
            }
            public void Enable()
            {
            }
            public void Disable()
            {
            }
            public void Destroy()
            {
            }
        }




    }



}

