using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Inspector;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：用来存放所有具体定义的配置项
namespace Configure.ConfigureItems
{




    [System.Serializable]
    public class ConfigureItem_SetInput : ConfigureItem, IConfigureRunnerBuilder, IConfigItemInfo
    {
        [Sirenix.OdinInspector.LabelText("输入映射")]
        public List<InputSetting> InputSettings = new List<InputSetting>();


        [Serializable]
        public class InputSetting
        {


            public Type DataType => outData.DataType;
            public string DataName => outData.DataName;
            [HideInInspector]
            public string InputAction_Name;
            [HideInInspector]
            public string InputAction_MapName;

            public Action<T> CreateDataSetter<T>(GameObject gameObject)
            {
                return outData.CreateDataSetter<T>(gameObject);
            }






            #region //&界面

            [Sirenix.OdinInspector.ValueDropdown(nameof(PlayerInputActionNames))]
            [Sirenix.OdinInspector.OnValueChanged(nameof(OnValueChanged))]
            [Sirenix.OdinInspector.LabelText("输入动作")]
            [SerializeField]
            private int actionDataIndex;
            private Sirenix.OdinInspector.ValueDropdownList<int> PlayerInputActionNames
            {
                get
                {
                    actionDatas.Clear();
                    Sirenix.OdinInspector.ValueDropdownList<int> re = new();
                    var playerInput = GameObject.FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();
                    if (playerInput != null)
                        playerInput.actions.actionMaps.SelectMany(x => x.actions).ForEach((x, i) =>
                        {
                            string text = x.actionMap.name + "/" + x.name;
                            actionDatas.Add((i, x.actionMap.name, x.name, x.expectedControlType));
                            re.Add(text, i);
                        });

                    return re;
                }
            }



            private void OnValueChanged()
            {
                if (actionDatas.TryFind(x => x.Item1 == actionDataIndex, out var data))
                {
                    InputAction_MapName = data.Item2;
                    InputAction_Name = data.Item3;
                    typeDict.TryGetValue(data.Item4, out dataType);
                    outData.DataType = dataType;
                }
                else
                {
                    InputAction_MapName = null;
                    InputAction_Name = null;
                    outData.DataType = null;
                }
            }
            private Dictionary<string, Type> typeDict = new()
            {
                {"Button",typeof(bool)},
                {"Vector2",typeof(Vector2)},
                {"Axis",typeof(float)},
            };
            [SerializeField]
            private List<(int, string, string, string)> actionDatas = new();
            [Sirenix.OdinInspector.ReadOnly]
            private Type dataType;
            [Sirenix.OdinInspector.LabelText("输出数据")]
            [SerializeField]
            private Configure.Inspector.OutDataInspector outData = new Inspector.OutDataInspector();

            #endregion
            //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




        }







        public Inspector.HelpText 说明 = new("将玩家输入数据引入到游戏数据库中");


        string IConfigItemInfo.MenuName => "玩家输入/引入输入数据";
        IConfigItemInfo.ConfigItemInfo IConfigItemInfo.OptionalInfo => null;
        IConfigureRunner IConfigureRunnerBuilder.CreateRunner(MonoBehaviour mono) => new Runner()
        {
            gameObject = mono.gameObject,
            config = this,
        };



        private class Runner : IConfigureRunner
        {
            public GameObject gameObject;
            public ConfigureItem_SetInput config;

            private Action onEnableAction;
            private Action onDisableAction;






            public bool AllowEnable => true;

            public void Init()
            {
                UnityEngine.InputSystem.InputActionAsset actions = GameObject.FindObjectOfType<UnityEngine.InputSystem.PlayerInput>().actions;
                Debug.Log("Init");

                config.InputSettings.ForEach(x =>
                {
                    string actionName = x.InputAction_Name.Log("actionName");

                    string mapName = x.InputAction_MapName.Log("mapName");
                    string dataName = x.DataName.Log("dataName");
                    Type type = x.DataType.Log("type");

                    Action<UnityEngine.InputSystem.InputAction.CallbackContext> dataSetter = null;


                    if (type == typeof(bool))
                    {
                        EventDataHandler<bool> ds = EventData.EventDataF.GetData<bool>(dataName, gameObject);
                        dataSetter = d => ds.Data = (d.ReadValueAsButton());
                    }
                    else if (type == typeof(Vector2))
                    {
                        Action<Vector2> setter = x.CreateDataSetter<Vector2>(gameObject);
                        dataSetter = d => setter(d.ReadValue<Vector2>());
                    }
                    else if (type == typeof(float))
                    {
                        Action<float> setter = x.CreateDataSetter<float>(gameObject);
                        dataSetter = d => setter(d.ReadValue<float>());
                    }
                    else
                    {
                        Debug.LogError("未知类型");
                    }




                    UnityEngine.InputSystem.InputAction action = actions.FindActionMap(mapName).FindAction(actionName);




                    Action<UnityEngine.InputSystem.InputAction.CallbackContext> performedAct = ctx =>
                    {
                        dataSetter?.Invoke(ctx);
                        Debug.Log("Performed");
                    };

                    onEnableAction += () =>
                    {
                        action.performed += performedAct;
                    };
                    onDisableAction += () =>
                    {
                        action.performed -= performedAct;
                    };








                });











            }

            public void UnInit()
            {

            }

            public void Enable()
            {
                onEnableAction?.Invoke();
                Debug.Log("Enable");
            }

            public void Disable()
            {
                onDisableAction?.Invoke();
            }





        }


    }



}

