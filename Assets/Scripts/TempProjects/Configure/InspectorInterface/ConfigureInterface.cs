using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    updateEnabler = EventDataF.CreateConditionEnabler(() => action?.Invoke(eventDataHandler.Data), null, eventDataHandler.OnUpdateCondition);
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
            [AllowNesting]
            [NaughtyAttributes.Label("")]
            [Dropdown("UpdateDropdownNames")]
            [StackableField]
            public string dataName;

            public DataHolder_NameDropDown(System.Enum dataNamePreset)
            {
                dataName = dataNamePreset.ToString();
            }


            public DataHolder_NameDropDown(string dataNamePreset)
            {
                
                dataName = dataNamePreset;
            }

            public EventDataHandler<T> GetEventDataHandler(GameObject gameObject)
            {
                return EventDataF.GetData<T>(dataName, gameObject);
            }


            private string[] UpdateDropdownNames()
            {
                return EventData.DataNameF.GetAllNamesOnTypeRegex(typeof(T)).ToArray();
            }
        }



   






        



        [System.Serializable]
        public class DataGetter<T>
        {
            [NaughtyAttributes.AllowNesting, NaughtyAttributes.OnValueChanged("UpdateImport")]
            [StackableField]
            [StackableDecorator.Label(-1, title = "??????", tooltip = "???????????????????????????")]
            public bool import = false;


            [SerializeReference]
            [HorizontalGroup("info", true, "", 0, prefix = false)]
            [StackableField]
            public DataBase dataBase;


            private T dataPreset;
            private string dataNamePreset;

            public DataGetter(T dataPreset = default, string dataNamePreset = null, bool? import = null)
            {
                //??????????????????????????????

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
                //??????????????????
                this.dataPreset = dataPreset;
                this.dataNamePreset = dataNamePreset;

                //??????
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



                    //
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    System.Type[] types = assembly.GetTypes();
                    //???????????????
                    Type type = types.Where((t) => t.Namespace == "Configure.Interface")
                    .Where((t) => t.GetField("data") != null)
                    .First((t) => t.GetField("data").FieldType == typeof(T));
                    //?????????????????????
                    if (type != null)
                    {

                        DataBase item = System.Activator.CreateInstance(type) as DataBase;


                        dataBase = item;
                        dataBase.DataValue = dataPreset;




                    }
                    else
                    {
                        Debug.LogError("????????????: " + typeof(T).ToString());
                    }


                }
            }
        }




        [System.Serializable]
        public class DataBase
        {

            //???????????????DataConst<T>???data
            public System.Object DataValue
            {
                get
                {
                    return GetData();
                }
                set
                {
                    SetData(value);
                }
            }

            public virtual string DataName => "";




            protected virtual System.Object GetData()
            {

                System.Reflection.FieldInfo[] fieldInfos = this.GetType().GetFields();
                foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
                {
                    //??????data
                    if (fieldInfo.Name == "data")
                    {
                        return fieldInfo.GetValue(this);
                    }
                }


                return null;
            }

            protected virtual void SetData(System.Object data)
            {
                System.Reflection.FieldInfo[] fieldInfos = this.GetType().GetFields();
                foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
                {
                    //??????data
                    if (fieldInfo.Name == "data")
                    {
                        fieldInfo.SetValue(this, data);
                    }
                }
            }


        }






        [System.Serializable]
        public class DataConstFloat : DataBase
        {
            [StackableField, StackableDecorator.Label(-1, title = "??????")]
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
            [HorizontalGroup("info", true, "", 0, 30, -1, prefix = false)]
            [StackableField]
            [SerializeField]
            private Holder holder = new Holder();
            [HideInInspector]
            public Vector2 data;



            [System.Serializable]
            public class Holder
            {
                [StackableDecorator.LabelOnly]
                [StackableField]
                public int ?????? = 0;
                [StackableDecorator.Label(0)]
                [StackableField]
                public Vector2 dataInSide;
            }


            protected override System.Object GetData()
            {
                return holder.dataInSide;
            }
            protected override void SetData(System.Object data)
            {
                holder.dataInSide = (Vector2)data;
            }

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

            public override string DataName => dataName;
            public DataImport(System.Type type)
            {
                this.type = type;
                updateDropdownNames = DataNameF.GetAllNamesOnTypeRegex(type).ToList();
            }



            private System.Type type;



            private List<string> UpdateDropdownNames => updateDropdownNames;
            private List<string> updateDropdownNames;


        }









    }
}
