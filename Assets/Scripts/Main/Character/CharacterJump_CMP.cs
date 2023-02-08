using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleTimer.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterJump_CMP : MonoBehaviour
{
    public float jumpSpeed = 12;
    public float maxForce = 40;
    public Vector2 jumpDirection = new Vector2(0, 1);
    public float lastTimeMax = 0.4f;
    public float lastTimeMin = 0.2f;

    public bool isOnJump;

    public Events events = new Events();
    public class Events
    {
        public UnityEvent OnJumpStart = new UnityEvent();
        public UnityEvent OnJumpEnd = new UnityEvent();
    }




    private Rigidbody2D rb;
    private SimpleTimer.Base.SimpleTimer[] timers = new SimpleTimer.Base.SimpleTimer[2];
    private Action disableAction;

    private OtState jumpState;

    private void Awake()
    {
        rb = gameObject.GetRigidbody2D();


        jumpState = new OtState(gameObject, Conf.CharacterActionName.跳跃);

        OtState.Condition.Create(gameObject, (con) =>
        {
            con.SetStatesAllOn(Conf.CharacterActionName.在地上, Conf.CharacterActionName.输入跳跃);
            con.SetStatesAllOff(Conf.CharacterActionName.跳跃);
            con.SetToTrunOn(Conf.CharacterActionName.跳跃);
        });


    }


    private void OnEnable()
    {

        InputF.InputActionTryAdd(Conf.InputName.Jump, onKeyInput, ref disableAction);
        jumpState.AddFunction(JumpFunction, ref disableAction);


    }



    private void OnDisable()
    {
        disableAction?.Invoke();
        disableAction = null;
    }



    private void JumpFixedUpdateAction()
    {
        Vector2 curV = rb.velocity;
        Vector2 tarV = jumpSpeed * jumpDirection;
        float maxF = maxForce;
        Vector2 forceDir = jumpDirection;

        Vector2 forceAdd = PhysicMathF.CalcForceByVel(curV, tarV, maxF, forceDir, rb.mass);

        rb.AddForce(forceAdd);

    }




    private void OnProtectEnd()
    {
        if (!InputF.GetLastInputData(Conf.InputName.Jump).IsKeyOn())
        {
            jumpState.Enabled = false;
        }
    }
    private void onKeyInput(InputAction.CallbackContext d)
    {
        if (!d.IsKeyOn() & timers[0] == null)
        {
            jumpState.Enabled = false;
        }
    }





    public void JumpFunction(bool enabled)
    {

        BasicEvent.OnFixedUpdate.Turn(gameObject, JumpFixedUpdateAction, enabled);
        isOnJump = enabled;


        if (enabled)
        {
            GetComponent<CharacterAnimation_Cmp>()?.PlayAnimation(Conf.AnimationName.跳跃);
            timers[0] = TimerF.Wait(lastTimeMin, OnProtectEnd);
            timers[1] = TimerF.Wait(lastTimeMax, () => jumpState.Enabled = false);
            events.OnJumpStart.Invoke();
        }
        else
        {
            timers.Destroy();
            events.OnJumpEnd.Invoke();
        }
    }
}
