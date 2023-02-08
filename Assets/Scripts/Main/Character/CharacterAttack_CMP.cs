using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CharacterAttack_CMP : MonoBehaviour
{



    [SerializeField] private float lastTime = 0.3f;
    public bool isAttack = false;
    private Action disableAction;

    private void OnEnable()
    {
        InputF.InputTriggerAdd(Conf.InputName.Attack, Conf.InputCondition.ButtonDown, AttackTest, ref disableAction);
    }


    private void OnDisable()
    {
        disableAction?.Invoke();
        disableAction = null;
    }



    private void AttackTest()
    {
        bool isAttackKeyDown = InputF.GetLastInputData(Conf.InputName.Attack).IsKeyOn();
        bool isNotAttack = !isAttack;

        if (isAttackKeyDown & isNotAttack)
        {
            DoAttack();
        }
    }



    private void DoAttack()
    {
        isAttack = true;
        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.攻击, true);
        GetComponent<CharacterMove_CMP>()?.StopAndLock();

        GetComponent<CharacterAnimation_Cmp>().PlayAnimation(Conf.AnimationName.攻击);
        TimerF.Wait(lastTime, DoAttackEnd);

    }

    private void DoAttackEnd()
    {
        isAttack = false;
        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.攻击, false);
        GetComponent<CharacterMove_CMP>()?.UnLock();

        CharacterActionF.OnActionEndInvoke(gameObject);
    }




}
