using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
using UnityEditor;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    namespace ConfigureItems
    {


        [System.Serializable]
        public class ConfigureItem_GetPhysicData : ConfigureItem
        {


            // public int;
            [Header("输出参数")]
            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<Vector2> 运动速度 = new Configure.Inspector.DataNameDropDown<Vector2>(DataNamePreset.运动速度向量);


            [Space]
            //脚本说明
            public Inspector.HelpText 说明 = new Inspector.HelpText("从Unity组件中获得物理数据:", "运动速度");



            public ConfigureItem_GetPhysicData()
            {
                requiredTypes = new List<Type>() { typeof(Rigidbody2D) };


                createRunnerFunc = CreateRunner;
            }

            private ConfigureRunner CreateRunner(GameObject obj)
            {

                var runner = new runner(this, obj);
                return new ConfigureRunner(runner.initialize, runner.enable, runner.disable, runner.destroy);

            }

            private class runner
            {
                private ConfigureItem_GetPhysicData cf;
                private GameObject gameObject;
                private Rigidbody2D rigidbody2D;

                public runner(ConfigureItem_GetPhysicData configureItem_GetPhysicData, GameObject gameObject)
                {
                    cf = configureItem_GetPhysicData;
                    this.gameObject = gameObject;
                    rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

                }




                private EventDataHandler<Vector2> speedD;



                public void initialize()
                {
                    //获取运动速度
                    speedD = EventDataF.GetData<Vector2>(cf.运动速度.dataName, gameObject);
                }
                public void destroy()
                {

                }
                public void enable()
                {
                    BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);
                }

                public void disable()
                {
                    BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdate);
                }



                private void FixedUpdate()
                {
                    speedD.Data = rigidbody2D.velocity;
                }




            }




        }




    }


}

