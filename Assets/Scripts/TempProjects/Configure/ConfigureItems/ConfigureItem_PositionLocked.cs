using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Inspector;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItems
{




    [System.Serializable]
    public class ConfigureItem_PositionLocked : ConfigureItem
    {


        [SerializeField]
        
        
        public Inspector.TagDropDown 追踪对象标签 = new Inspector.TagDropDown(UnityTag.玩家角色碰撞体);







        //脚本说明
        public Inspector.HelpText 说明 = new Inspector.HelpText("将自身位置锁定到目标位置","参数解释");







        public ConfigureItem_PositionLocked()
        {
            createRunnerFunc = CreateRunner;
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
                lockTarget = GameObject.FindGameObjectWithTag(cf.追踪对象标签.tag);

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

