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


        EventDataHandler<Vector2> move = EventDataF.GetDataGlobal<Vector2>(DataName.全局_输入_移动向量);
        EventDataHandler<float> moveX = EventDataF.GetDataGlobal<float>(DataName.全局_输入_移动横向值);
        EventDataHandler<float> moveY = EventDataF.GetDataGlobal<float>(DataName.全局_输入_移动纵向值);
        asset.FindAction("Move").performed += (d) =>
        {
            Vector2 vector2 = d.ReadValueAsVector2();
            move.Data = vector2;
            moveX.Data = vector2.x;
            moveY.Data = vector2.y;
        };




        List<(string InputActionName, System.Enum CmDataName)> nameMapList = new List<(string InputActionName, System.Enum CmDataName)>
        {
            ("Jump", DataName.全局_输入_跳跃键),
            ("Dash", DataName.全局_输入_冲刺键),
        };


        List<EventDataHandler<bool>> list = nameMapList.Select((name) => EventDataF.GetDataGlobal<bool>(name.CmDataName)).ToList();

        nameMapList.ForEach((names, i) =>
        {
            asset.FindAction(names.InputActionName).performed += (d) =>
            {
                list[i].Data = d.ReadValueAsButton();
            };
        });


    }
}
