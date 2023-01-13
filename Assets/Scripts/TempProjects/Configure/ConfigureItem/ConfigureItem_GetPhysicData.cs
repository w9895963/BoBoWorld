using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {
        //配置:計算行走施力
        [CreateAssetMenu(fileName = "获取物理量", menuName = "动态配置/获取物理量", order = 1)]
        public class ConfigureItem_GetPhysicData : ConfigureBase
        {
            [NaughtyAttributes.InfoBox("无参数", EInfoBoxType.Normal)]

            //脚本说明
            [NaughtyAttributes.Label("其他信息")]
            public ShowOnlyText info = new ShowOnlyText("从Unity组件中获得物理数据:", "运动速度");

            //必要组件
            protected override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D) };













            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject, MonoBehaviour monoBehaviour = null)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);


                //获取运动速度
                EventDataHandler<Vector2> speedD = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);


                //获取物理组件
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();









                enabler.Enable += () =>
                {
                    BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);
                };


                enabler.Disable += () =>
                {
                    BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdate);
                };


                void FixedUpdate()
                {
                    speedD.Data = rigidbody2D.velocity;
                }

                return enabler;
            }


        }

        [System.Serializable]
        [AddTypeMenu("物理/获取物理量")]
        public class ConfigureItem_GetPhysicData_ : ConfigureBase_
        {


            [NaughtyAttributes.InfoBox("无参数", EInfoBoxType.Normal)]

            //脚本说明
            [NaughtyAttributes.Label("其他信息")]
            public ShowOnlyText info = new ShowOnlyText("从Unity组件中获得物理数据:", "运动速度");

            //必要组件
            protected override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D) };


            public ConfigureItem_GetPhysicData_()
            {

                Construct();
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
                    return runner;
                };





                //获取物理组件
                rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            }
            private void initialize()
            {
                //获取运动速度
                speedD = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);
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

