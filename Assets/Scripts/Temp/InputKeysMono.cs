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


        Action<Vector2> moveSet = EventDataF.GetDataSetter<Vector2>(EventDataName.Input.移动);
        asset.FindAction("Move").performed += (d) =>
        {
            Vector2 moveData = d.ReadValueAsVector2();
            moveSet(moveData);
        };


        List<(string InputActionName, System.Enum CmDataName)> nameMapList = new List<(string InputActionName, System.Enum CmDataName)>
        {
            ("Jump", EventDataName.Input.跳跃),
            ("Dash", EventDataName.Input.冲刺),
        };
        var dataSetList = nameMapList.Select((name) => EventDataF.GetDataSetter<bool>(name.CmDataName)).ToList();

        nameMapList.ForEach((names, i) =>
        {
            asset.FindAction(names.InputActionName).performed += (d) =>
            {
                dataSetList[i](d.ReadValueAsButton());
            };
        });


    }
}
