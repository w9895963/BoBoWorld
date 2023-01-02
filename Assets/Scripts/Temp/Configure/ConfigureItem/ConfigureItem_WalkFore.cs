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

            [Label("其他信息")]
            public ShowOnlyText info = new ShowOnlyText("输入: 输入指令_移动, 地表法线", "输出: 行走施力");











            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);


                //获取数据行走输入
                EventDataHandler<Vector2> moveInput = EventDataF.GetData<Vector2>(DataName.输入指令_移动);
                //获取数据地表法线
                EventDataHandler<Vector2> groundNormal = EventDataF.GetData<Vector2>(DataName.地表法线, gameObject);

                //获取数据行走施力
                EventDataHandler<Vector2> moveForce = EventDataF.GetData<Vector2>(DataName.行走施力, gameObject);

                //获取刚体
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

                // enabler = ConfigureF.OnDataCondition(CalculateMoveForce, OnFail,导入参数);
                enabler = EventDataF.OnDataCondition(CalculateMoveForce, OnFail, moveInput.OnCustom(() => moveInput.Data.x != 0), groundNormal.OnUpdate);





                //计算移动施力
                void CalculateMoveForce()
                {
                    // Debug.Log("计算移动施力");
                    Vector2 currentVelocity = rigidbody2D.velocity;
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

