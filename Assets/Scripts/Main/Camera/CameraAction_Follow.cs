using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static CommonFunction.Static;

public class CameraAction_Follow : MonoBehaviour
{




    //*Variable
    public GameObject target;
    public CameraAction_Follow_Conf config;



    [Header("内部参数")]
    [SerializeField] private float inputTime;
    [SerializeField] private Vector2 inputOffsetBlendStart;
    [SerializeField] private Vector2 lastInput;
    [SerializeField] private Vector2 inputOffsetNow;



    private CameraAction_Follow_Conf cf => config;


    private void OnEnable()
    {
        InputF.InputActionTryAdd(Conf.InputName.Move, InputAction);
    }

    private void OnDisable()
    {
        InputF.InputActionTryRemove(Conf.InputName.Move, InputAction);
    }

    private void InputAction(InputAction.CallbackContext d)
    {
        Vector2 move = d.ReadValueAsVector2();
        if (move.magnitude > 0)
        {
            if (lastInput.magnitude == 0 | move.Angle(lastInput) >= 45)
            {
                if (move.y.Abs() > move.x.Abs())
                {
                    lastInput = new Vector2(0, move.y.Sign());
                }
                else
                {
                    lastInput = new Vector2(move.x.Sign(), 0);
                }
                inputTime = Time.time;
                inputOffsetBlendStart = inputOffsetNow;

            }

        }

    }




    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector2 tarP = target.GetPosition2d();
        tarP += cf.目标偏移; //*Offset
        inputOffsetNow = InputOffset();
        tarP += inputOffsetNow;
        Vector2 p = gameObject.GetPosition2d();
        Vector2 pathV = tarP - p;
        float pathX = pathV.x;
        float pathY = pathV.y;


        void HorMove()
        {
            float velValue = cf.横轴距离速度曲线.Evaluate(pathX.Abs(), cf.横轴距离范围.x, cf.横轴距离范围.y, cf.横轴速度范围.x, cf.横轴速度范围.y) * pathX.Sign();
            float moveDist = velValue * dt;
            moveDist = moveDist.ClampAbsMax(pathX.Abs());
            gameObject.MovePosition(Vector2.right, moveDist);
        }
        HorMove();



        void VerMove()
        {
            float velValue = cf.纵轴距离速度曲线.Evaluate(pathY.Abs(), cf.纵轴距离范围.x, cf.纵轴距离范围.y, cf.纵轴速度范围.x, cf.纵轴速度范围.y) * pathY.Sign();
            float moveDist = velValue * dt;
            moveDist = moveDist.ClampAbsMax(pathY.Abs());
            gameObject.MovePosition(Vector2.up, moveDist);
        }
        VerMove();


    }

    private Vector2 InputOffset()
    {
        float t = Time.time - inputTime;
        float blend = t.Map(0, cf.跟踪过度时间, 0, 1);
        Vector2 tarFocus = new Vector2(lastInput.x * cf.按键偏移.x, lastInput.y * cf.按键偏移.y) - (lastInput.y == -1 ? cf.目标偏移 : Vector2.zero);
        tarFocus = tarFocus * blend + inputOffsetBlendStart * (1 - blend);
        return tarFocus;
    }


}
