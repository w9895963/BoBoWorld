using System;
using System.Collections;
using System.Collections.Generic;
using EventData;
using UnityEngine;

public class PlayerCharacter_Manager : MonoBehaviour
{
    //功能启用器
    private (Action Enable, Action Disable) Enabler = (default, default);
    private void Awake()
    {

        //*判断是否站在地面上


        //*行走功能
        //获得移动输入
        float moveInput = default;
        EventDataHandler<Vector2> move = EventDataF.GetData_global<Vector2>(EventDataName.Input.移动);
        move.SetDataTo((d) => moveInput = d.x, ref Enabler);
        //获得地面法线
        Vector2 groundNormal = Vector2.up;
        EventDataF.GetData_local<Vector2>(gameObject, EventDataName.PlayerObject.地面法线).SetDataTo((d) => groundNormal = d, ref Enabler);
        //获得速度
        float speed = 0;
        EventDataF.GetData_local<float>(gameObject, EventDataName.PlayerConfig.移动速度).SetDataTo((d) => speed = d, ref Enabler);
        //获得最大力
        float maxForce = 0;
        EventDataF.GetData_local<float>(gameObject, EventDataName.PlayerConfig.移动最大施力).SetDataTo((d) => maxForce = d, ref Enabler);
        //获得刚体
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();



        //创建施力数据
        EventDataHandler<Vector2> moveForceH = EventDataF.GetData_local<Vector2>(gameObject, EventDataName.PlayerObject.移动施力);


        EventDataF.CreateDataCondition(CalculateMoveForce, ref Enabler, move.OnUpdate());
        //方法：计算移动施力
        void CalculateMoveForce()
        {
            Vector2 currentVelocity = rigidbody2D.velocity;
            Func<Vector2> targetVelocity = () =>
            {
                Vector2 v = default;
                //行走方向向量
                Vector2 direction = groundNormal.y >= 0 ? groundNormal.Rotate(90) : groundNormal.Rotate(-90);
                direction = direction.normalized;
                //计算期望速度
                v = direction * speed * moveInput;
                return v;
            };
            Vector2 projectVector = groundNormal.Rotate(90);
            float mass = rigidbody2D.mass;
            //计算移动施力
            Vector2 moveForce = PhysicMathF.CalcForceByVel(currentVelocity, targetVelocity(), maxForce, projectVector, mass);
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
