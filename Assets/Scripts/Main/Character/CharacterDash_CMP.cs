using System;
using System.Collections;
using System.Collections.Generic;
using Timer.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterDash_CMP : MonoBehaviour
{
    public float inputDelay = 0.4f;

    public float dashSpeed = 30f;
    public Vector2 defaultDashDirection = new Vector2(1, 0);
    public float dashTime = 0.4f;
    public float maxForce = 240f;
    public bool ready = true;
    public UnityEvent onDashStart;
    public UnityEvent onDashEnd;


    public bool inputValue;
    public bool forceOn = false;
    public float faceDirection;


    private SimpleTimer dashTimer;
    private Rigidbody2D rb;
    private Vector2 positionStart;
    private SimpleTimer inputTimer;

    private void Reset()
    {
    }

    private void Awake()
    {
        rb = gameObject.GetRigidbody2D();
    }


    private void OnEnable()
    {
        InputF.InputActionTryAdd(Conf.InputName.Dash, InputAction);
        GetComponent<CharacterGroundEvent_CMP>().events.GroundStepOn.AddListener(OnGroundAction);
    }
    private void OnDisable()
    {
        InputF.InputActionTryRemove(Conf.InputName.Dash, InputAction);
        GetComponent<CharacterGroundEvent_CMP>().events.GroundStepOn.RemoveListener(OnGroundAction);
    }




    private void InputAction(InputAction.CallbackContext d)
    {
        inputTimer.DestroyImmediate();
        inputValue = d.IsKeyOn();
        if (inputValue == true)
        {
            inputTimer = TimerF.Wait(inputDelay, () => inputValue = false);
        }
    }

    
    private void OnGroundAction()
    {
        ready = true;
        
    }



    private void FixedUpdate()
    {
        bool isDashTrigger = inputValue & !forceOn & ready;
        if (isDashTrigger)
        {
            faceDirection = GetComponent<CharacterMove_CMP>().FaceDirection;
            DoDash();
        }


        if (forceOn)
        {
            Vector2 dashDir = defaultDashDirection * faceDirection;
            Vector2 force = PhysicMathF.CalcForceByVel(rb.velocity, dashSpeed * dashDir, maxForce, defaultDashDirection, rb.mass);

            rb.AddForce(force);

        }
    }







    public void DoDash()
    {
        forceOn = true;
        ready = false;
        dashTimer = TimerF.Wait(dashTime, DoBreakDash);
        rb.velocity = rb.velocity.Project(Vector2.right);
        onDashStart.Invoke();

    }

    public void DoBreakDash()
    {
        forceOn = false;
        onDashEnd.Invoke();
        bool isOnGround = GetComponent<CharacterGroundEvent_CMP>().isOnGround;
        if (isOnGround)
        {
            ready = true;
        }
    }



}
