using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventData;
using UnityEngine;

namespace Configure.Inspector
{
    [System.Serializable]
    public class DataHold_NameOrData<T>
    {
        
        
        public Configure.Inspector.DataGetter<T> data;

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
    public class DataGetter<T>
    {
        
        
        public bool import = false;


        [SerializeReference]
        
        
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



                //
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Type[] types = assembly.GetTypes();
                //历遍所有类
                Type type = types.Where((t) => t.Namespace == "Configure.Interface")
                .Where((t) => t.GetField("data") != null)
                .First((t) => t.GetField("data").FieldType == typeof(T));
                //如果存在则新建
                if (type != null)
                {

                    DataBase item = System.Activator.CreateInstance(type) as DataBase;


                    dataBase = item;
                    dataBase.DataValue = dataPreset;




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

        //用反射获得DataConst<T>的data
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
                //找到data
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
                //找到data
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
        
        public float data;


    }
    [System.Serializable]
    public class DataConstBool : DataBase
    {
        
        public bool data;


    }
    [System.Serializable]
    public class DataConstVector : DataBase
    {
        
        
        [SerializeField]
        private Holder holder = new Holder();
        [HideInInspector]
        public Vector2 data;



        [System.Serializable]
        public class Holder
        {
            
            
            public int 向量 = 0;
            
            
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
        
        public GameObject data;

    }



    [System.Serializable]
    public class DataImport : DataBase
    {
        // [StackableField]
        public string dataName;

        public override string DataName => dataName;
        public DataImport(System.Type type)
        {
            this.type = type;
            updateDropdownNames = PresetNameF.GetAllNamesOnType(type).ToList();
        }



        private System.Type type;



        private List<string> UpdateDropdownNames => updateDropdownNames;
        private List<string> updateDropdownNames;


    }









}
