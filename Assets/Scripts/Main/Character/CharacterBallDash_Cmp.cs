using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterBallDash_Cmp : MonoBehaviour
{
    public float keyTimeAllow = 0.1f;
    private Action disableAction;



    private void OnEnable()
    {
        InputF.InputActionTryAdd(Conf.InputName.Attack, Condition, ref disableAction);
    }
    private void OnDisable()
    {
        disableAction?.Invoke();
    }




    private void Condition(InputAction.CallbackContext d)
    {
        bool KeyTrigger = d.IsKeyOn();
        bool OnJump = CharacterActionF.GetState(gameObject, Conf.CharacterActionName.跳跃) == true;
        bool stateOn = CharacterActionF.GetState(gameObject, Conf.CharacterActionName.球冲刺) == true;




        bool OnState = KeyTrigger & OnJump & stateOn;
        bool OffState = false;




        if (OnState)
        {
            BallDash(true);
        }
        if (OffState)
        {
            BallDash(false);
        }
    }




    private void BallDash(bool enabled)
    {
        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.球冲刺, enabled);
    }



}
