using System;
using System.Collections.Generic;

using EventData;
using StackableDecorator;
using UnityEditor;

using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItem
{




    [System.Serializable]
    public class ConfigureItem_WalkFore: ConfigureBase
    {
        [Header("固定参数")]
        public float 行走速度 = 10;
        [Tooltip("计算出的行走力大小依据这个加速度计算")]
        public float 最大加速度 = 10;
        [Tooltip("生成一个和地面法线相反的里，将物体固定在地上")]
        public float 向地施力大小 = 10;

        [Header("动态参数")]
        
        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "移动指令", tooltip = "获得移动指令")]
        public Configure.Interface.DataHolder_NameDropDown < float > 移动指令 = new Configure.Interface.DataHolder_NameDropDown < float > (DataName.全局_输入_移动横向值);
        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得脚下的地面法线")]
        public Configure.Interface.DataHolder_NameDropDown < Vector2 > 地表法线 = new Configure.Interface.DataHolder_NameDropDown < Vector2 > (DataName.地表法线);
        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "运动速度", tooltip = "获得物体的运动速度")]
        public Configure.Interface.DataHolder_NameDropDown < Vector2 > 运动速度 = new Configure.Interface.DataHolder_NameDropDown < Vector2 > (DataName.运动速度向量);











        [Header("输出参数")]

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, tooltip = "根据输入计算出行走施力")]
        public Configure.Interface.DataHolder_NameDropDown < Vector2 > 行走施力 = new Configure.Interface.DataHolder_NameDropDown < Vector2 > (DataName.行走施力);

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, tooltip = "根据输入计算出向地施力，将角色保持压在路面上")]
        public Configure.Interface.DataHolder_NameDropDown < Vector2 > 获取数据行走向地施力 = new Configure.Interface.DataHolder_NameDropDown < Vector2 > (DataName.行走向低施力);



        //脚本说明
        public ShowOnlyText info_ = new ShowOnlyText("根据一系列参数计算出施加于物体上的用于行走的力", "输入: 输入指令_移动, 地表法线, 运动速度", "输出: 行走施力");










        private GameObject gameObject;
        private ConfigureRunner configureRunner;

        public ConfigureItem_WalkFore(): base(createRunner_) {}

        private static ConfigureRunner createRunner_(GameObject gameObject, ConfigureBase configureBase_) {
            ConfigureItem_WalkFore self = (configureBase_ as ConfigureItem_WalkFore);
            self.gameObject = gameObject;
            self.configureRunner = new ConfigureRunner(self.initialize, self.enable, self.disable, self.destroy);
            return self.configureRunner;
        }



        //获取数据行走输入
        private EventDataHandler < float > moveInput;

        //获取数据运动速度
        private EventDataHandler < Vector2 > moveSpeed;
        //获取数据地表法线
        private EventDataHandler < Vector2 > groundNormal;
        //获取数据当前速度

        //获取数据行走施力
        private EventDataHandler < Vector2 > moveForce;
        //获取数据行走向地施力
        private EventDataHandler < Vector2 > groundDownForce;
        
        //获取刚体
        private Rigidbody2D rigidbody2D;
        private (Action Enable, Action Disable) enabler;







        private void destroy() {}

        private void disable() {
            enabler.Disable?.Invoke();
        }

        private void enable() {
            enabler.Enable?.Invoke();
        }

        private void initialize() {
            //获取数据行走输入
            moveInput = EventDataF.GetData < float > (移动指令.dataName, gameObject);
            //获取数据运动速度
            moveSpeed = EventDataF.GetData < Vector2 > (运动速度.dataName, gameObject);
            //获取数据地表法线
            groundNormal = EventDataF.GetData < Vector2 > (地表法线.dataName, gameObject);

            //获取数据行走施力
            moveForce = EventDataF.GetData < Vector2 > (行走施力.dataName, gameObject);
            //获取数据行走向地施力
            groundDownForce = EventDataF.GetData < Vector2 > (行走向地施力.dataName, gameObject);

            //获取刚体
            rigidbody2D = gameObject.GetComponent < Rigidbody2D > ();



            (EventData.Core.EventData data, Func < bool > check)[] checks = {
                moveInput.OnCustomCondition(() => moveInput.Data != 0),
                groundNormal.OnUpdateCondition,
                moveSpeed.OnUpdateCondition
            };
            enabler = EventDataF.CreateConditionEnabler(CalculateMoveForce, OnFail, checks);
        }





        //计算移动施力
        void CalculateMoveForce() {
            // Debug.Log("计算移动施力");
            Vector2 currentVelocity = moveSpeed.Data;
            // Debug.Log("当前速度:" + currentVelocity);
            float mass = rigidbody2D.mass;
            Vector2 groundNormalV = groundNormal.Data.magnitude > 0 ? groundNormal.Data.normalized: Vector2.up;
            float moveInputV = moveInput.Data;

            float speed = 行走速度;
            float maxForce = 最大加速度;
            Func < Vector2 > targetVelocity = () =>
            {
                Vector2 v = default;

                    //行走方向向量
                    Vector2 direction = groundNormalV.y >= 0 ? groundNormalV.Rotate(90): groundNormalV.Rotate(-90);
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
                groundDownForce.Data = 向地施力大小 * groundNormalV * -1;

            }

            void OnFail() {
                // Debug.Log("计算移动施力失败");
                moveForce.Data = Vector2.zero;
                groundDownForce.Data = Vector2.zero;
                
            }

        }



    }