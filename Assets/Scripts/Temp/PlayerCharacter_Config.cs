using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "玩家角色配置", menuName = "配置/玩家角色配置", order = 0)]
public class PlayerCharacter_Config : ScriptableObject
{
    public float 重力大小 = 9.8f;
    public Vector2 重力方向 = Vector2.down;
    //test
}
