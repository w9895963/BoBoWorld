using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_PlayerCharacter : MonoBehaviour
{
    private void Awake()
    {
        //获取配置管理器
        ConfigManager configManager = FindObjectOfType<ConfigManager>();
        //将属性添加到事件数据
        configManager.AddConfigToEventData<PlayerCharacter_Config, EventDataName.Player >(gameObject);

    }













}
