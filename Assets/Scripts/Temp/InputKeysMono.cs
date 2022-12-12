using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventDataS;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputKeysMono : MonoBehaviour
{
    private void Awake()
    {
        InputActionAsset asset = GameObject.FindObjectOfType<PlayerInput>().actions;


        EventDataHandler<Vector2> move = EventDataF.GetData_global<Vector2>(EventDataName.Input.移动);
        asset.FindAction("Move").performed += (d) =>
        {
            move.Data = d.ReadValueAsVector2();
        };




        List<(string InputActionName, System.Enum CmDataName)> nameMapList = new List<(string InputActionName, System.Enum CmDataName)>
        {
            ("Jump", EventDataName.Input.跳跃),
            ("Dash", EventDataName.Input.冲刺),
        };


        List<EventDataHandler<bool>> list = nameMapList.Select((name) => EventDataF.GetData_global<bool>(name.CmDataName)).ToList();
        nameMapList.ForEach((names, i) =>
        {
            asset.FindAction(names.InputActionName).performed += (d) =>
            {
                list[i].Data = d.ReadValueAsButton();
            };
        });


    }
}
