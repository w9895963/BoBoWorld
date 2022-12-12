using System;
using System.Collections;
using System.Collections.Generic;
using EventDataS;
using UnityEngine;

public class PlayerCharacter_Manager : MonoBehaviour
{
    //功能启用器
    private (Action Enable, Action Disable) Enabler = (default, default);
    private void Awake()
    {

        //*判断是否站在地面上


        //*行走功能

        //获得刚体
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        //获得移动输入
        EventDataHandler<Vector2> moveInputH = EventDataF.GetData_global<Vector2>(EventDataName.Input.移动);
        //获得地面法线
        EventDataHandler<Vector2> groundNormalH = EventDataF.GetData_local<Vector2>(gameObject, EventDataName.PlayerObject.地面法线);
        //获得行走速度
        EventDataHandler<float> speed = EventDataF.GetData_local<float>(gameObject, EventDataName.PlayerConfig.移动速度);
        //获得最大力
        EventDataHandler<float> maxForceH = EventDataF.GetData_local<float>(gameObject, EventDataName.PlayerConfig.移动最大施力);
        //创建施力数据
        EventDataHandler<Vector2> moveForceH = EventDataF.GetData_local<Vector2>(gameObject, EventDataName.PlayerObject.移动施力);
        //添加数据更新事件
        var list = new List<(EventDataS.EventDataUtil.EventData, Func<bool>)>();
        list.Add(moveInputH.OnUpdate, groundNormalH.OnUpdate, speed.OnUpdate, maxForceH.OnUpdate);
        EventDataF.OnDataCondition(CalculateMoveForce, ref Enabler, list);

        //方法：计算移动施力
        void CalculateMoveForce()
        {
            Vector2 currentVelocity = rigidbody2D.velocity;
            Vector2 groundNormal = groundNormalH.Data.magnitude > 0 ? groundNormalH.Data.normalized : Vector2.up;
            float moveInput = moveInputH.Data.x;
            Func<Vector2> targetVelocity = () =>
            {
                Vector2 v = default;

                //行走方向向量
                Vector2 direction = groundNormal.y >= 0 ? groundNormal.Rotate(90) : groundNormal.Rotate(-90);
                direction = direction.normalized;
                //计算期望速度
                v = direction * speed.Data * moveInput;
                return v;
            };
            Vector2 projectVector = groundNormal.Rotate(90);
            float mass = rigidbody2D.mass;
            //计算移动施力
            Vector2 moveForce = PhysicMathF.CalcForceByVel(currentVelocity, targetVelocity(), maxForceH.Data, projectVector, mass);
            //施力赋值器
            moveForceH.Data = moveForce;
        }





        //*将属性添加到事件数据 
        Enabler.Enable += AddConfigToEventData;

    }




    //将属性添加到事件数据 
    private void AddConfigToEventData()
    {
        //获取配置管理器
        ConfigManager configManager = FindObjectOfType<ConfigManager>();
        //添加配置到事件数据
        configManager.AddConfigToEventData<PlayerCharacter_Config, EventDataName.PlayerConfig>(gameObject);
    }







    private void OnEnable()
    {

        Enabler.Enable?.Invoke();
    }
    private void OnDisable()
    {
        Enabler.Disable?.Invoke();
    }
}
