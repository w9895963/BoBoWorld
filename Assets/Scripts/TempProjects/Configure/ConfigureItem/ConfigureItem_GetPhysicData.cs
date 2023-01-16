using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
// using NaughtyAttributes;
using StackableDecorator;
using UnityEditor;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {


        [System.Serializable]
        public class ConfigureItem_GetPhysicData_ : ConfigureBase_
        {


            [Header("导出数据")]
            [Tooltip("获得物体的运动速度2")]
            [HorizontalGroup("Split", true, "", 0, prefix = true, title = "运动速度", tooltip = "获得物体的运动速度")]
            [StackableField]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> speedIn = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.运动速度向量);


            [Space]
            //脚本说明
            public ShowOnlyText 脚本说明 = new ShowOnlyText("从Unity组件中获得物理数据:", "运动速度");

            //必要组件
            protected override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D) };


            public ConfigureItem_GetPhysicData_()
            {

                Construct();
                displaceTypeName = "物理/获取物理量";
            }








            private GameObject gameObject;
            private EventDataHandler<Vector2> speedD;
            private Rigidbody2D rigidbody2D;

            private void Construct()
            {
                //创建运行器
                ConfigureRunner runner = new ConfigureRunner(initialize, enable, disable, destroy);

                createRunner = (obj) =>
                {
                    gameObject = obj;
                    //获取物理组件
                    rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                    return runner;
                };



            }
            private void initialize()
            {
                //获取运动速度
                speedD = EventDataF.GetData<Vector2>(speedIn.dataName, gameObject);
            }
            private void destroy()
            {

            }
            private void enable()
            {
                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);
            }

            private void disable()
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

