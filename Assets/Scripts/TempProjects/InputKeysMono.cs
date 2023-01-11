using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventData;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputKeysMono : MonoBehaviour
{
    private void Awake()
    {
        InputActionAsset asset = GameObject.FindObjectOfType<PlayerInput>().actions;


        EventDataHandler<Vector2> move = EventDataF.GetData<Vector2>(DataName.全局_输入_移动向量);
        asset.FindAction("Move").performed += (d) =>
        {
            move.Data = d.ReadValueAsVector2();
        };




        List<(string InputActionName, System.Enum CmDataName)> nameMapList = new List<(string InputActionName, System.Enum CmDataName)>
        {
            ("Jump", DataName.全局_输入_跳跃值),
            ("Dash", DataName.全局_输入_冲刺值),
        };


        List<EventDataHandler<bool>> list = nameMapList.Select((name) => EventDataF.GetData<bool>(name.CmDataName)).ToList();

        nameMapList.ForEach((names, i) =>
        {
            asset.FindAction(names.InputActionName).performed += (d) =>
            {
                list[i].Data = d.ReadValueAsButton();
            };
        });


    }
}
