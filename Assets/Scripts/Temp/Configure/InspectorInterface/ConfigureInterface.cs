using System;
using System.Collections;
using System.Collections.Generic;
using EventData;
using NaughtyAttributes;
using StackableDecorator;
using UnityEngine;

namespace Configure
{
    namespace Interface
    {
        [System.Serializable]
        public class DataHold_NameOrData<T>
        {
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, 60, -1)]
            public Configure.Interface.DataGetter<T> data;

            public DataHold_NameOrData(System.Enum dataNamePreset)
            {
                data = new DataGetter<T>(dataNamePreset: dataNamePreset.ToString());
            }

            public DataHold_NameOrData(T dataPreset = default, string dataNamePreset = null)
            {
                data = new DataGetter<T>(dataPreset, dataNamePreset);
            }

            public DataHold_NameOrData(T dataPreset, System.Enum dataNamePreset, bool import)
            {
                data = new DataGetter<T>(dataPreset, dataNamePreset.ToString(), import);
            }

            public bool IsConst => !data.import;
            public T DataValue => data.dataBase.DataValue == null ? default : (T)data.dataBase.DataValue;


            public string DataName => data.dataBase.DataName;

            public (Action Enable, Action Disable) SetDataOnChanged(Action<T> action, GameObject gameObject)
            {
                (Action Enable, Action Disable) enabler = default;
                (Action Enable, Action Disable) updateEnabler = default;
                EventDataHandler<T> eventDataHandler = null;





                if (!IsConst)
                {
                    eventDataHandler = EventDataF.GetData<T>(DataName, gameObject);
                    updateEnabler = EventDataF.OnDataCondition(() => action?.Invoke(eventDataHandler.Data), null, eventDataHandler.OnUpdate);
                }

                enabler.Enable = () =>
                {
                    if (IsConst)
                    {
                        action?.Invoke(DataValue);
                    }
                    else
                    {
                        action?.Invoke(eventDataHandler.Data);
                        updateEnabler.Enable?.Invoke();
                    }
                };

                enabler.Disable = () =>
                {
                    if (!IsConst)
                    {
                        updateEnabler.Disable?.Invoke();
                    }
                };



                return enabler;
            }
        }




        [System.Serializable]
        public class DataHolder_NameDropDown<T>
        {
            // [SerializeField]
            // [StackableField]
            // [HorizontalGroup("info2", true, "", 0)]
            [AllowNesting]
            [NaughtyAttributes.Label("")]
            [Dropdown("UpdateDropdownNames")]
            [StackableField]
            public string dataName;
            public DataHolder_NameDropDown(System.Enum dataNamePreset)
            {
                dataName = dataNamePreset.ToString();
            }

            public EventDataHandler<T> GetEventDataHandler(GameObject gameObject)
            {
                return EventDataF.GetData<T>(dataName, gameObject);
            }


            private string[] UpdateDropdownNames()
            {
                return EventData.DataNameF.GetNamesOnType(typeof(T)).ToArray();
            }
        }











        [System.Serializable]
        public class DataGetter<T>
        {
            [NaughtyAttributes.AllowNesting, NaughtyAttributes.OnValueChanged("UpdateImport")]
            [StackableField]
            [StackableDecorator.Label(-1, title = "引用", tooltip = "是否从外部引用数据")]
            public bool import = false;


            [SerializeReference]
            [HorizontalGroup("info", true, "", 0, prefix = false)]
            [StackableField]
            public DataBase dataBase;


            private T dataPreset;
            private string dataNamePreset;

            public DataGetter(T dataPreset = default, string dataNamePreset = null, bool? import = null)
            {
                //根据情况判断是否引用

                if (dataNamePreset != null)
                {
                    this.import = true;
                }
                else
                {
                    this.import = false;
                }
                if (import != null)
                {
                    this.import = import.Value;
                }
                //参数全部设置
                this.dataPreset = dataPreset;
                this.dataNamePreset = dataNamePreset;

                //更新
                UpdateImport();
            }

            private void UpdateImport()
            {
                if (import)
                {
                    DataImport dataImport = new DataImport(typeof(T));
                    dataImport.dataName = dataNamePreset;
                    dataBase = dataImport;
                }
                else
                {
                    if (typeof(T) == typeof(float))
                    {
                        DataConstFloat dataConstFloat = new DataConstFloat();
                        dataConstFloat.data = (float)(object)dataPreset;
                        dataBase = dataConstFloat;
                    }
                    else if (typeof(T) == typeof(Vector2))
                    {
                        DataConstVector dataConstVector = new DataConstVector();
                        dataConstVector.data = (Vector2)(object)dataPreset;
                        dataBase = dataConstVector;
                    }
                    else if (typeof(T) == typeof(GameObject))
                    {
                        DataConstGameObject dataConstGameObject = new DataConstGameObject();
                        dataConstGameObject.data = (GameObject)(object)dataPreset;
                        dataBase = dataConstGameObject;
                    }
                    //bool
                    else if (typeof(T) == typeof(bool))
                    {
                        DataConstBool dataConstBool = new DataConstBool();
                        dataConstBool.data = (bool)(object)dataPreset;
                        dataBase = dataConstBool;
                    }

                    else
                    {
                        Debug.LogError("未知类型: " + typeof(T).ToString());
                    }
                }
            }
        }




        [System.Serializable]
        public class DataBase
        {

            //判断是否属于DataConst<T>
            public virtual bool IsConst => true;
            //用反射获得DataConst<T>的data
            public System.Object DataValue => GetData();
            public virtual string DataName => "";




            private System.Object GetData()
            {

                System.Reflection.FieldInfo[] fieldInfos = this.GetType().GetFields();
                foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
                {
                    //找到data
                    if (fieldInfo.Name == "data")
                    {
                        return fieldInfo.GetValue(this);
                    }
                }


                return null;
            }


        }






        [System.Serializable]
        public class DataConstFloat : DataBase
        {
            [StackableField, StackableDecorator.Label(0)]
            public float data;


        }
        [System.Serializable]
        public class DataConstBool : DataBase
        {
            [StackableField, StackableDecorator.Label(0)]
            public bool data;


        }
        [System.Serializable]
        public class DataConstVector : DataBase
        {
            [StackableField, StackableDecorator.Label(0)]
            public Vector2 data;

        }
        [System.Serializable]
        public class DataConstGameObject : DataBase
        {
            [StackableField, StackableDecorator.Label(0)]
            public GameObject data;

        }



        [System.Serializable]
        public class DataImport : DataBase
        {
            [AllowNesting, Dropdown("UpdateDropdownNames")]
            [NaughtyAttributes.Label("")]
            // [StackableField]
            public string dataName;

            public override bool IsConst => false;
            public override string DataName => dataName;
            public DataImport(System.Type type)
            {
                this.type = type;
                updateDropdownNames = DataNameF.GetNamesOnType(type);
            }



            private System.Type type;



            private List<string> UpdateDropdownNames => updateDropdownNames;
            private List<string> updateDropdownNames;


        }









    }
}
