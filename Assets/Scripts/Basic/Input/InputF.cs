using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static CommonFunction.Static;









namespace CommonFunction
{
    namespace Define
    {
        public class Function_Input
        {
            #region PlayerInput Asset
            private static InputActionAsset inputAsset;
            public InputActionAsset InputAsset => Fc.FieldGetOrSet(ref inputAsset, () => GameObject.FindObjectOfType<PlayerInput>().actions);
            private static GameObject assetHolder;
            public GameObject AssetHolder => Fc.FieldGetOrSet(ref assetHolder, () => GameObject.FindObjectOfType<PlayerInput>().gameObject);
            #endregion
            // * Region PlayerInput Asset End---------------------------------- 



            #region Get Last Input Data
            private static Dictionary<string, InputAction.CallbackContext> currentInputData = new Dictionary<string, InputAction.CallbackContext>();
            private static Dictionary<string, (InputAction.CallbackContext? keyDown, InputAction.CallbackContext? keyUp)> currentKeyData
                                = new Dictionary<string, (InputAction.CallbackContext? keyDown, InputAction.CallbackContext? keyUp)>();
            //



            public InputAction.CallbackContext GetLastInputData(string inputName)
            {
                InputAction.CallbackContext data2;
                currentInputData.TryGetValue(inputName, out data2);
                return data2;
            }
            public InputAction.CallbackContext GetLastInputData(Conf.InputName inputName)
            {
                return GetLastInputData(inputName.ToString());
            }






            public bool IsLastInputKeyDownAndTimeLessThan(Conf.InputName inputName, float time)
            {
                InputAction.CallbackContext keyDate;
                bool hasValue = currentInputData.TryGetValue(inputName.ToString(), out keyDate);
                if (!hasValue) return false;


                if (!keyDate.IsKeyOn()) return false;

                return (Time.time - keyDate.time) <= time;

            }
            public bool IsLastInputKeyUpAndTimeLessThan(Conf.InputName inputName, float time)
            {
                InputAction.CallbackContext keyDate;
                bool hasValue = currentInputData.TryGetValue(inputName.ToString(), out keyDate);
                if (!hasValue) return false;


                if (keyDate.IsKeyOn()) return false;

                return (Time.time - keyDate.time) <= time;

            }


            #endregion
            // * Region GetLastInputData End---------------------------------- 






            #region Input Action
            private static Dictionary<Action, Action<InputAction.CallbackContext>> InputActionTryAdd_ActionDic = new Dictionary<Action, Action<InputAction.CallbackContext>>();
            private static class InputActionTryAddMethod
            {
                public static InputActionTryAdd MainMethod = InitialMethod;


                public delegate void InputActionTryAdd(InputActionAsset asset, string inputName, System.Action<InputAction.CallbackContext> inputAction);


                private static void Initial_ForCurrentInputDataGet()
                {
                    var asset = InputF.InputAsset;
                    var dic = currentInputData;
                    asset.actionMaps.SelectMany((map) => map.actions).ForEach((act) =>
                        {
                            string key = act.name;
                            act.performed += (d) =>
                            {
                                dic[key] = d;
                            };
                        });
                }
                private static void Initial_ForCurrentKeyData()
                {
                    var asset = InputF.InputAsset;
                    var dic = currentKeyData;
                    asset.actionMaps.SelectMany((map) => map.actions).ForEach((act) =>
                        {
                            string key = act.name;
                            act.performed += (d) =>
                            {
                                bool? but = d.TryReadButton();
                                if (but == null)
                                    return;

                                (InputAction.CallbackContext? keyDown, InputAction.CallbackContext? keyUp) v = dic.GetOrCreate(key);
                                if (but == true)
                                {
                                    v.keyDown = d;
                                }
                                else
                                {
                                    v.keyUp = d;
                                }
                                dic[key] = v;
                            };
                        });
                }
                private static void InitialMethod(InputActionAsset asset, string inputName, System.Action<InputAction.CallbackContext> inputAction)
                {
                    Initial_ForCurrentInputDataGet();
                    Initial_ForCurrentKeyData();
                    NormalMethod(asset, inputName, inputAction);
                    MainMethod = NormalMethod;
                }
                private static void NormalMethod(InputActionAsset asset, string inputName, System.Action<InputAction.CallbackContext> inputAction)
                {
                    if (asset != null)
                    {
                        InputAction act = asset.FindAction(inputName);
                        if (act != null)
                        {
                            act.performed += inputAction;
                        }
                    }
                }
            }
            public void InputActionTryAdd(InputActionAsset asset, string inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                InputActionTryAddMethod.MainMethod(asset, inputName, inputAction);
            }
            public void InputActionTryAdd(InputActionAsset asset, string inputName, System.Action inputAction)
            {
                Action<InputAction.CallbackContext> act = (d) => inputAction?.Invoke();
                InputActionTryAdd_ActionDic[inputAction] = act;
                InputActionTryAddMethod.MainMethod(asset, inputName, act);
            }
            public void InputActionTryAdd(InputActionAsset asset, Conf.InputName inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                InputActionTryAdd(asset, inputName.ToString(), inputAction);
            }

            public void InputActionTryAdd(Conf.InputName inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                InputActionTryAdd(InputAsset, inputName.ToString(), inputAction);
            }
            public void InputActionTryAdd(Conf.InputName inputName, System.Action<InputAction.CallbackContext> inputAction, ref Action disableAction)
            {
                InputActionTryAdd(InputAsset, inputName.ToString(), inputAction);
                disableAction += () => InputActionTryRemove(InputAsset, inputName.ToString(), inputAction);
            }
            public void InputActionTryAdd(Conf.InputName inputName, System.Action inputAction, ref Action disableAction)
            {
                InputActionTryAdd(InputAsset, inputName.ToString(), inputAction);
                disableAction += () => InputActionTryRemove(InputAsset, inputName.ToString(), inputAction);
            }




            public void InputActionTryRemove(InputActionAsset asset, string inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                if (asset != null)
                {
                    InputAction act = asset.FindAction(inputName);
                    if (act != null)
                    {
                        act.performed -= inputAction;
                    }
                }
            }
            public void InputActionTryRemove(InputActionAsset asset, string inputName, System.Action inputAction)
            {
                Action<InputAction.CallbackContext> action = InputActionTryAdd_ActionDic.TryGetValue(inputAction);
                InputActionTryRemove(asset, inputName, action);
            }

            public void InputActionTryRemove(InputActionAsset asset, Conf.InputName inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                InputActionTryRemove(asset, inputName.ToString(), inputAction);
            }

            public void InputActionTryRemove(Conf.InputName inputName, System.Action<InputAction.CallbackContext> inputAction)
            {
                InputActionTryRemove(InputAsset, inputName.ToString(), inputAction);
            }
            #endregion
            // * Region Input Action End---------------------------------- 



            #region Input Trigger
            private static Dictionary<Action, Action<InputAction.CallbackContext>> keyAction_inputActionDict = new Dictionary<Action, Action<InputAction.CallbackContext>>();
            public void InputTriggerAdd(Conf.InputName inputName, Conf.InputCondition inputState, Action keyAction)
            {

                Dictionary<Action, Action<InputAction.CallbackContext>> dic = keyAction_inputActionDict;
                Action<InputAction.CallbackContext> inputAction = (d) =>
                {

                    bool keyConditionIsRight = false;
                    switch (inputState)
                    {
                        case Conf.InputCondition.Changed:
                            keyConditionIsRight = true;
                            break;
                        case Conf.InputCondition.ButtonUp:
                            keyConditionIsRight = !d.IsKeyOn();
                            break;
                        case Conf.InputCondition.ButtonDown:
                            keyConditionIsRight = d.IsKeyOn();
                            break;
                    }

                    if (keyConditionIsRight)
                    {
                        keyAction();
                    }

                };
                bool hasAction = dic.ContainsKey(keyAction);


                if (hasAction)
                {
                    InputActionTryRemove(InputAsset, inputName.ToString(), dic[keyAction]);
                }


                dic[keyAction] = inputAction;
                InputActionTryAdd(InputAsset, inputName.ToString(), inputAction);
            }
            public void InputTriggerAdd(Conf.InputName inputName, Conf.InputCondition inputState, Action keyAction, ref Action AddRemoveAction)
            {
                InputTriggerAdd(inputName, inputState, keyAction);
                AddRemoveAction += () => InInputTriggerRemove(inputName, inputState, keyAction);
            }
            public void InInputTriggerRemove(Conf.InputName inputName, Conf.InputCondition inputState, Action keyAction)
            {
                Dictionary<Action, Action<InputAction.CallbackContext>> dic = keyAction_inputActionDict;
                bool hasAction = dic.ContainsKey(keyAction);

                if (hasAction)
                {
                    InputActionTryRemove(InputAsset, inputName.ToString(), dic[keyAction]);
                    dic.Remove(keyAction);
                }
            }
            #endregion
            // * Region Input Trigger End---------------------------------- 






        }
    }

}


