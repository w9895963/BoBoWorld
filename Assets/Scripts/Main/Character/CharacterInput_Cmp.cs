using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonFunction.Static;

public class CharacterInput_Cmp : MonoBehaviour
{
    [SerializeField] private float keyWaitTime = 0.1f;
    private Action disableAction;
    private OtState inputJumpState;

    private void Awake()
    {
        inputJumpState = OtState.GetOrCreate(gameObject, Conf.CharacterActionName.输入跳跃);
    }
    private void OnEnable()
    {
        InputF.InputActionTryAdd(Conf.InputName.Jump, (d) =>
        {
            bool keyOn = d.IsKeyOn();
            if (keyOn)
            {
                inputJumpState.Enabled = true;
                TimerF.Wait(keyWaitTime, () => inputJumpState.Enabled = false);
            }
            else
            {

            }

        }, ref disableAction);



    }
    private void OnDisable()
    {
        disableAction?.Invoke();
    }
}
