using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravity_CMP : MonoBehaviour
{
    public Vector2 gravityDirection = new Vector2(0, -1);
    public float gravityForce = 9.8f;
    [SerializeField] private bool isFall;



    private Rigidbody2D rb;
    private Action enableAction;
    private Action disableAction;


    private OtState fallState;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GravityForce();

        FallAnimation();
    }
    private void OnEnable()
    {
        enableAction?.Invoke();
    }
    private void OnDisable()
    {
        disableAction?.Invoke();
    }



    private void GravityForce()
    {
        enableAction = () =>
        {
            BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction, ref disableAction);
        };

    }

    private void FallAnimation()
    {
        fallState = new OtState(gameObject, Conf.CharacterActionName.动画_下落);
        OtState.Condition.Create(gameObject, (con) =>
        {
            ;
        });
    }


    private void FixedUpdateAction()
    {
        Vector2 force = gravityDirection * gravityForce;
        rb.AddForce(force);
    }


    private bool GvOnCondition(CharacterAction.TriggerData d)
    {
        return true;
    }
    private bool GvOffCondition(CharacterAction.TriggerData d)
    {
        return false;
    }
    private void GvOn()
    {
        BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
    }
    private void GvOff()
    {
        BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
    }






    private void FallOnCon()
    {
        bool isNotOnGround = GetComponent<CharacterGroundEvent_CMP>()?.isOnGround == false;
        bool OffCheck = CharacterActionF.IsAllActionOff(gameObject, Conf.CharacterActionName.跳跃);
        if (isNotOnGround & OffCheck)
        {
            FallOn();
        }
    }

    private void FallOffCon()
    {
        FallOff();
    }


    private void FallOn()
    {
        isFall = true;
        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.动画_下落, true);
        GetComponent<CharacterAnimation_Cmp>()?.PlayAnimation(Conf.AnimationName.下落);
        fallState.Enabled = true;
    }


    private void FallOff()
    {
        isFall = false;
        CharacterActionF.SetState(gameObject, Conf.CharacterActionName.动画_下落, false);

        fallState.Enabled = false;
    }

}
