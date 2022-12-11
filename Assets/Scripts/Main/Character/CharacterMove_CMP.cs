using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using static CommonFunction.Static;
using System;
using Conf;

public class CharacterMove_CMP : MonoBehaviour
{

    public float moveForce = 80f;
    public float stopForce = 10f;
    public float stopForceGround = 0;
    public float maxSpeed = 10f;
    public Vector2 defaultWalkDirection = new Vector2(1, 0);

    public float walkDirection = 0;
    public bool locked = false;
    private int facedirection;



    private Rigidbody2D rb;
    private Action disableAction;
    private AnimationName? nextAnimation;

    public int FaceDirection => facedirection;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        //*move initial fixedupdate Action
        BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction, ref disableAction);

        //*StartTrigger
        InputF.InputTriggerAdd(Conf.InputName.Move, Conf.InputCondition.Changed, MoveConditionTest, ref disableAction);
        GetComponent<CharacterGroundEvent_CMP>().events.GroundStepOn.AddListener(MoveConditionTest, ref disableAction);
        GetComponent<CharacterGroundEvent_CMP>().events.GroundStepOut.AddListener(MoveConditionTest, ref disableAction);
        CharacterActionF.OnActionEndAdd(gameObject, MoveConditionTest, ref disableAction);
    }
    private void OnDisable()
    {
        disableAction?.Invoke();
        disableAction = null;
    }

    private void FixedUpdateAction()
    {

        Vector2 targetV = walkDirection * defaultWalkDirection.normalized * maxSpeed;
        Vector2 force;
        if (walkDirection != 0)
        {
            force = PhysicMathF.CalcForceByVel(rb.velocity, targetV, moveForce, defaultWalkDirection);
        }
        else
        {
            force = PhysicMathF.CalcForceByVel(rb.velocity, targetV, stopForce + stopForceGround, defaultWalkDirection);
        }
        rb.AddForce(force);
    }







    //*Condition


    private void MoveConditionTest()
    {
        if (locked)
        {
            return;
        }



        bool isInputMove = InputF.GetLastInputData(Conf.InputName.Move).ReadValueAsVector2_X() != 0;
        bool isInputStand = InputF.GetLastInputData(Conf.InputName.Move).ReadValueAsVector2_X() == 0;
        bool isInputNotJump = !InputF.GetLastInputData(Conf.InputName.Jump).IsKeyOn();
        bool isOnGround = GetComponent<CharacterGroundEvent_CMP>().isOnGround;
        bool isOnAir = !isOnGround;




        if (true)
        {
            SetWalkDirection();
        }

        if (isInputMove)
        {
            SetFace();
        }


        CharacterAnimation_Cmp cmp = GetComponent<CharacterAnimation_Cmp>();
        if (isInputMove & isOnGround & isInputNotJump)
        {
            nextAnimation = Conf.AnimationName.行走;
            cmp?.PlayAnimation(nextAnimation.Value);


        }


        if (isInputStand & isOnGround)
        {
            nextAnimation = Conf.AnimationName.站立;
            cmp?.PlayAnimation(nextAnimation.Value);
        }

    }



    //*Action

    private void SetWalkDirection()
    {
        walkDirection = InputF.GetLastInputData(Conf.InputName.Move).ReadValueAsVector2_X();

    }
    private void SetFace()
    {
        GetComponent<CharacterAnimation_Cmp>()?.SetAnimationFlipX(walkDirection < 0);
        facedirection = walkDirection < 0 ? -1 : 1;
    }


    public void StopAndLock()
    {
        walkDirection = 0;
        locked = true;
    }
    public void UnLock()
    {
        locked = false;
    }



}
