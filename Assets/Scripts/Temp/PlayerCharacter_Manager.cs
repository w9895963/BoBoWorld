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

        //判断是否站在地面上    
        JudgeOnGround();




        //计算移动施力
        CalculateMoveForce();





        //将玩家配置添加到事件数据 
        AddConfigToEventData();







    }


    //判断是否站在地面上
    private void JudgeOnGround()
    {
        //获得刚体
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        //获得地面法线
        EventDataHandler<Vector2> groundNormalH = EventDataF.GetData_local<Vector2>(gameObject, EventDataName.PlayerObject.地面法线);
        //获得是否站在地面上
        EventDataHandler<bool> onGroundH = EventDataF.GetData_local<bool>(gameObject, EventDataName.PlayerObject.已站在地面上);


        Enabler.Enable += () =>
        {
            BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
            BasicEvent.OnCollision2D_Exit.Add(gameObject, onCollisionStay2D);
            BasicEvent.OnCollision2D_Exit.Add(gameObject, onCollisionExit2D);
        };
        Enabler.Disable += () =>
        {
            BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
            BasicEvent.OnCollision2D_Exit.Remove(gameObject, onCollisionStay2D);
            BasicEvent.OnCollision2D_Exit.Remove(gameObject, onCollisionExit2D);
        };




        //碰撞事件
        void OnCollisionEnter2D(Collision2D collision)
        {

        }
        void onCollisionStay2D(Collision2D collision)
        {

        }
        void onCollisionExit2D(Collision2D collision)
        {

        }
    }



    //计算移动施力
    private void CalculateMoveForce()
    {
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


        //*计算移动施力
        var list = new List<(EventDataS.EventDataCore.EventData, Func<bool>)>();
        list.Add(moveInputH.OnUpdate, groundNormalH.OnUpdate, speed.OnUpdate, maxForceH.OnUpdate);
        EventDataF.OnDataCondition(CalculateMoveForce, ref Enabler, list);
        //计算移动施力
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
    }
    //将玩家配置添加到事件数据 
    private void AddConfigToEventData()
    {
        Enabler.Enable += () =>
        {
            //获取配置管理器
            ConfigManager configManager = FindObjectOfType<ConfigManager>();
            //添加配置到事件数据
            configManager.AddConfigToEventData<PlayerCharacter_Config, EventDataName.PlayerConfig>(gameObject);
        };
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
