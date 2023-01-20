using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
using StackableDecorator;
using UnityEditor;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {


        [System.Serializable]
        public class ConfigureItem_GetPhysicData : ConfigureBase
        {


            // public int;
            [Header("输出参数")]
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "运动速度", tooltip = "获得物体的运动速度")]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> 运动速度 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.运动速度向量);


            [Space]
            //脚本说明
            public ShowOnlyText 说明 = new ShowOnlyText("从Unity组件中获得物理数据:", "运动速度");



            public ConfigureItem_GetPhysicData()
            {
                requiredTypes = new List<Type>() { typeof(Rigidbody2D) };


                createRunner = CreateRunner;
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

