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
        //配置:計算行走施力
        [CreateAssetMenu(fileName = "計算行走施力", menuName = "动态配置/計算行走施力", order = 1)]
        public partial class ConfigureItem_WalkFore : ConfigureBase
        {

            public float 行走速度 = 10;
            public float 最大加速度 = 10;

            //脚本说明

            [NaughtyAttributes.Label("其他信息")]
            public ShowOnlyText info_ = new ShowOnlyText("根据一系列参数计算出施加于物体上的用于行走的力", "输入: 输入指令_移动, 地表法线, 运动速度", "输出: 行走施力");











            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject, MonoBehaviour monoBehaviour = null)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);


                //获取数据行走输入
                EventDataHandler<Vector2> moveInput = EventDataF.GetData<Vector2>(DataName.全局_输入_移动向量);
                //获取数据运动速度
                EventDataHandler<Vector2> moveSpeed = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);
                //获取数据地表法线
                EventDataHandler<Vector2> groundNormal = EventDataF.GetData<Vector2>(DataName.地表法线, gameObject);
                //获取数据当前速度
                EventDataHandler<Vector2> currentVelocity = EventDataF.GetData<Vector2>(DataName.运动速度向量, gameObject);

                //获取数据行走施力
                EventDataHandler<Vector2> moveForce = EventDataF.GetData<Vector2>(DataName.行走施力, gameObject);

                //获取刚体
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();



                (EventData.Core.EventData data, Func<bool> check)[] checks = {
                    moveInput.OnCustomCondition(() => moveInput.Data.x != 0),
                    groundNormal.OnUpdateCondition,
                    moveSpeed.OnUpdateCondition
                };
                enabler = EventDataF.CreateConditionEnabler(CalculateMoveForce, OnFail, checks,  monoBehaviour);





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




                return enabler;
            }


        }
    }


}

