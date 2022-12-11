using System;
using System.Collections;
using System.Collections.Generic;
using EventData;
using UnityEngine;

public class PlayerCharacter_Manager : MonoBehaviour
{
    private void Awake()
    {

        //*判断是否站在地面上


        //*行走功能
        //获得移动输入
        float moveInput = default;
        EventDataF.OnDataUpdate<Vector2>((d) => moveInput = d.x, EventDataName.Input.移动);
        //获得移动数据
        EventDataHandler<Vector2> move = EventDataF.GetData_global<Vector2>(EventDataName.Input.移动);
        // move.OnDataUpdateDo((d) => moveInput = d.x);
        //获得地面法线
        Vector2 groundNormal = Vector2.up;
        EventDataF.OnDataUpdate<Vector2>((d) => groundNormal = d, EventDataName.PlayerObject.地面法线, gameObject);
        //获得速度
        float speed = 0;
        EventDataF.OnDataUpdate<float>((d) => speed = d, EventDataName.PlayerConfig.移动速度, gameObject);
        //获得最大力
        float maxForce = 0;
        EventDataF.OnDataUpdate<float>((d) => maxForce = d, EventDataName.PlayerConfig.移动最大施力, gameObject);
        //获得刚体
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();



        //施力赋值器
        System.Action<Vector2> moveForceSetter = EventDataF.GetDataSetter<Vector2>(EventDataName.PlayerObject.移动施力, gameObject);
        EventDataF.CreateDataCondition(CalculateMoveForce, dataIsUpdate: new System.Enum[] { EventDataName.Input.移动 });
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
            moveForceSetter(moveForce);
        }





       


    }


    private void Start()
    {
        //*将属性添加到事件数据
        //获取配置管理器
        ConfigManager configManager = FindObjectOfType<ConfigManager>();
        //添加配置到事件数据
        configManager.AddConfigToEventData<PlayerCharacter_Config, EventDataName.PlayerConfig>(gameObject);
    }













}
