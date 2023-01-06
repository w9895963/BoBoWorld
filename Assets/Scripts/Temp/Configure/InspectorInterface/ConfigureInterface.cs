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
        public class DataGetterHold<T>
        {
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, 60, -1)]
            public Configure.Interface.DataGetter<T> data;


            public DataGetterHold(T dataPreset = default, string dataNamePreset = "", bool import = false)
            {
                data = new DataGetter<T>(dataPreset, dataNamePreset, import);
            }


            public bool IsConst => !data.import;
            public T DataValue => data.dataBase.DataValue == null ? default : (T)data.dataBase.DataValue;


            public string DataName => data.dataBase.DataName;
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

            public DataGetter(T dataPreset = default, string dataNamePreset = "", bool import = false)
            {
                this.dataPreset = dataPreset;
                this.dataNamePreset = dataNamePreset;
                this.import = import;
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
        public class DataConstVector : DataBase
        {
            [StackableField, StackableDecorator.Label(0)]
            public Vector2 data;

        }



        [System.Serializable]
        public class DataImport : DataBase
        {
            [AllowNesting, Dropdown("UpdateDropdownNames"), NaughtyAttributes.Label("")]
            [StackableField]
            public string dataName;

            public override bool IsConst => false;
            public override string DataName => dataName;

            public DataImport(System.Type type)
            {
                this.type = type;
            }

            private System.Type type;



            private List<string> UpdateDropdownNames => DataNameF.GetNamesOnType(type);


        }



    }
}
