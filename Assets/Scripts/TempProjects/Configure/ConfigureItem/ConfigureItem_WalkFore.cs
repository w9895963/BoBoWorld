using System;
using System.Collections.Generic;

using EventData;

using NaughtyAttributes;

using UnityEditor;

using UnityEngine;



//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {
      

        [System.Serializable]
        [AddTypeMenu("物理/计算行走施力")]
        public class ConfigureItem_WalkFore_ : ConfigureBase_
        {
            public float 行走速度 = 10;
            public float 最大加速度 = 10;

            //脚本说明

            [NaughtyAttributes.Label("说明")]
            [AllowNesting]
            public ShowOnlyText info_ = new ShowOnlyText("根据一系列参数计算出施加于物体上的用于行走的力", "输入: 输入指令_移动, 地表法线, 运动速度", "输出: 行走施力");




            private GameObject gameObject;
            private ConfigureRunner configureRunner;

            public ConfigureItem_WalkFore_() : base(createRunner_)
            {

            }

            private static ConfigureRunner createRunner_(GameObject gameObject, ConfigureBase_ configureBase_)
            {
                ConfigureItem_WalkFore_ self = (configureBase_ as ConfigureItem_WalkFore_);
                self.gameObject = gameObject;
                self.configureRunner = new ConfigureRunner(self.initialize, self.enable, self.disable, self.destroy);
                return self.configureRunner;
            }



            //获取数据行走输入
            private EventDataHandler<Vector2> moveInput;
            //获取数据运动速度
            private EventDataHandler<Vector2> moveSpeed;
            //获取数据地表法线
            private EventDataHandler<Vector2> groundNormal;
            //获取数据当前速度
            private EventDataHandler<Vector2> currentVelocity;

            //获取数据行走施力
            private EventDataHandler<Vector2> moveForce;

            //获取刚体
            private Rigidbody2D rigidbody2D;
            private (Action Enable, Action Disable) enabler;







            private void destroy()
            {
            }

            private void disable()
            {
                enabler.Disable?.Invoke();
            }

            private void enable()
            {
                enabler.Enable?.Invoke();
            }

            private void initialize()
            {
                //获取数据行走输入
                moveInput = EventDataF.GetData<Vector2>(DataName.全局_输入_移动向量, gameObject);
                //获取数据运动速度
                moveSpeed = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);
                //获取数据地表法线
                groundNormal = EventDataF.GetData<Vector2>(DataName.地表法线, gameObject);
                //获取数据当前速度
                currentVelocity = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);

                //获取数据行走施力
                moveForce = EventDataF.GetData<Vector2>(DataName.行走施力, gameObject);

                //获取刚体
                rigidbody2D = gameObject.GetComponent<Rigidbody2D>();



                (EventData.Core.EventData data, Func<bool> check)[] checks = {
                    moveInput.OnCustomCondition(() => moveInput.Data.x != 0),
                    groundNormal.OnUpdateCondition,
                    moveSpeed.OnUpdateCondition
                };
                enabler = EventDataF.CreateConditionEnabler(CalculateMoveForce, OnFail, checks);
            }





            //计算移动施力
            void CalculateMoveForce()
            {
                // Debug.Log("计算移动施力");
                Vector2 currentVelocity = moveSpeed.Data;
                // Debug.Log("当前速度:" + currentVelocity);
                float mass = rigidbody2D.mass;
                Vector2 groundNormalV = groundNormal.Data.magnitude > 0 ? groundNormal.Data.normalized : Vector2.up;
                float moveInputV = moveInput.Data.x;

                float speed = 行走速度;
                float maxForce = 最大加速度;
                Func<Vector2> targetVelocity = () =>
                {
                    Vector2 v = default;

                    //行走方向向量
                    Vector2 direction = groundNormalV.y >= 0 ? groundNormalV.Rotate(90) : groundNormalV.Rotate(-90);
                    direction = direction.normalized;

                    //计算期望速度
                    v = direction * speed * moveInputV;

                    // Debug.Log("期望速度:" + v);

                    return v;
                };
                Vector2 projectVector = groundNormalV.Rotate(90);


                //施力赋值
                Vector2 vector2 = PhysicMathF.CalcForceByVel(currentVelocity, targetVelocity(), maxForce, projectVector, mass);
                moveForce.Data = vector2;

            }

            void OnFail()
            {
                // Debug.Log("计算移动施力失败");
                moveForce.Data = Vector2.zero;
            }

        }
    }






}

