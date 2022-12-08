using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterDownPlatform_Cmp : MonoBehaviour
{



    private Rigidbody2D rb;
    private Action disableAction;

    private CharacterAction characterAction;
    public CharacterAction CharacterAction => characterAction;

    private void Awake()
    {
        rb = gameObject.GetRigidbody2D();
    }
    private void OnEnable()
    {
        // InputF.InputTriggerAdd(Conf.InputName.Jump, Conf.InputState.ButtonDown, ActionStartTest, ref disableAction);
        characterAction = new CharacterAction(Conf.CharacterActionName.下翻, gameObject, new CharacterAction.ConstructParm()
        {
            conditionForActionOn = ConditionEnable,
            actionEnable = ActionEnable,
        });

        InputF.InputActionTryAdd(Conf.InputName.Jump, characterAction.OnTrigger, ref disableAction);
    }



    private void OnDisable()
    {
        disableAction?.Invoke();
        characterAction = null;
    }


    private bool ConditionEnable(CharacterAction.TriggerData d)
    {
        bool 被跳跃键按下触发 = d.IsKeyPressed(Conf.InputName.Jump.ToString()) == true;
        bool 按住下键 = InputF.GetLastInputData(Conf.InputName.Move).ReadValueAsVector2_Y() < 0;

        return 被跳跃键按下触发 & 按住下键;
    }

    private void ActionEnable()
    {
        throw new NotImplementedException();
    }



    private void ActionStartTest()
    {
        bool downKeyPress = DownKeyPress();

        bool DownKeyPress()
        {
            var inputData = InputF.GetLastInputData(Conf.InputName.Move);
            bool p = inputData.ReadValueAsVector2_Y() < 0;
            bool p2 = (Time.time - inputData.time) < 0.2f;

            return p & p2;
        }
    }
}
