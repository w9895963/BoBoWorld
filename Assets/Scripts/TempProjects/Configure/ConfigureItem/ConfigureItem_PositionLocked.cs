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
        [StackableDecorator.TagPopup]
        [StackableDecorator.Label(title = "锁定对象的标签")]
        public string lockObjectTag = "玩家角色碰撞体";







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
            private GameObject lockTarget;

            public Runner(GameObject gameObject, ConfigureItem_PositionLocked cf)
            {
                this.gameObject = gameObject;
                this.cf = cf;
            }



            public void Initialize()
            {
                lockTarget = GameObject.FindGameObjectWithTag(cf.lockObjectTag);

            }
            public void Enable()
            {
                BasicEvent.OnUpdate.Add(gameObject, updateAction);
            }
            public void Disable()
            {
                BasicEvent.OnUpdate.Remove(gameObject, updateAction);
            }
            public void Destroy()
            {
            }


            private void updateAction()
            {
                gameObject.SetPosition(lockTarget.GetPosition2d());
            }
        }




    }
}

