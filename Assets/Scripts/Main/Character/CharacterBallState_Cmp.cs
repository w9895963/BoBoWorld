using System;
using System.Collections;
using System.Collections.Generic;
using SimpleTimer.Base;
using UnityEngine;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterBallState_Cmp : MonoBehaviour
{
    public float jumpTime = 0.2f;
    public float speed = 6;
    public float maxForce = 100;
    [SerializeField] private bool state;
    private Action disableAction;
    private float startTime;
    private SimpleTimer.Base.SimpleTimer jumpTimer;
    private Action fixedUpdateAction;
    private Character character;
    //刚体
    private Rigidbody2D rb;


    private OtState ballJumpState;
    private OtState jumpState;
    private OtState fallState;

    private void Awake()
    {
        character = new Character(gameObject);

        ballJumpState = new OtState(gameObject, Conf.CharacterActionName.球跳);
        jumpState = new OtState(gameObject, Conf.CharacterActionName.跳跃);
        fallState = new OtState(gameObject, Conf.CharacterActionName.动画_下落);

        //获得刚体
        rb = gameObject.GetRigidbody2D();


    }

    private void OnEnable()
    {
        character.Event.AddListener(Character.EventName.JumpEnd, () =>
        {
            EnableTrigger(true);
        }, ref disableAction);
        character.Event.AddListener(Character.EventName.GroundStepOn, () =>
        {
            EnableTrigger(false);
        }, ref disableAction);
        disableAction += () => InputF.InputActionTryRemove(Conf.InputName.Jump, Invoke);


        // fallState.Add
    }
    private void OnDisable()
    {
        disableAction?.Invoke();
    }


    private void EnableTrigger(bool v)
    {
        if (v)
        {
            InputF.InputActionTryAdd(Conf.InputName.Jump, Invoke);
        }
        else
        {
            InputF.InputActionTryRemove(Conf.InputName.Jump, Invoke);
        }


    }



    private void Invoke(InputAction.CallbackContext d)
    {

        SetEnabled(Condition());

    }



    private bool? Condition()
    {
        if (!state)
        {
            bool KeyCheck = InputF.IsLastInputKeyDownAndTimeLessThan(Conf.InputName.Jump, 0.1f);
            bool onFall = CharacterActionF.GetState(gameObject, Conf.CharacterActionName.动画_下落) == true;
            if (KeyCheck & onFall)
                return true;

        }
        else
        {
            bool KeyCheck = InputF.IsLastInputKeyUpAndTimeLessThan(Conf.InputName.Jump, 0.1f);
            bool StateCheck = true;
            if (KeyCheck & StateCheck) return false;

        }

        return null;


    }


    private void OffCondition()
    {
        SetEnabled(false);
    }




    private void SetEnabled(bool? st)
    {
        if (st == null) return;

        var v = st.Value;
        if (v)
        {
            startTime = Time.time;
            

            fixedUpdateAction = PhysicF.VelocityModify(rb, speed, maxForce, Vector2.up);
            jumpTimer = TimerF.Wait(jumpTime, () => BasicEvent.OnFixedUpdate.Remove(gameObject, fixedUpdateAction));
            BasicEvent.OnFixedUpdate.Add(gameObject, fixedUpdateAction);

            GetComponent<CharacterAnimation_Cmp>()?.PlayAnimation(Conf.AnimationName.滚动);
        }


        state = v;


        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.球跳, v);

    }


}
