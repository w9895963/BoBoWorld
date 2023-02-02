using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;


namespace Configure.Inspector
{
    [Serializable]
    [InlineProperty]
    [LabelWidth(120)]
    public partial class InputDataNameOrData
    {
        [SerializeField]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        private DataBox dataInputFile;





        private Type dataInputFileType;
        private Type inspectorDataType;
        private string presetDataName = "数据名或数据";
        private Object storeData;
        private Action switchToDataName;
        private Action switchToDataValue;
        private bool isStatic = true;

        public InputDataNameOrData(Type dataInputFileType, string presetDataName = null, Object presetData = null)
        {
            this.dataInputFileType = dataInputFileType;
            this.presetDataName = presetDataName;
            this.storeData = presetData;


            switchToDataName = () =>
            {
                dataInputFile = new DataName() { dataName = presetDataName };
                dataInputFile.switchDataAction += SwitchData;
                dataInputFile.dataType = dataInputFileType;
                isStatic = false;
            };
            inspectorDataType = typeof(DataValueClassGroup).GetNestedTypes().FirstOrDefault(t => t.GetField(nameof(DataValueNormalStyle<Object>.dataValue)).FieldType == dataInputFileType);
            switchToDataValue = () =>
            {
                if (inspectorDataType == null)
                {
                    Debug.LogError("Configure.Inspector.InputDataNameOrData.DataValueClassGroup 内没有找到可以创建的类型");
                }
                else
                {
                    dataInputFile = (DataBox)Activator.CreateInstance(inspectorDataType);
                    dataInputFile.switchDataAction += SwitchData;
                    dataInputFile.dataType = dataInputFileType;
                    isStatic = true;

                }
            };





            TimerF.WaitUpdate_InEditor(() =>
            {
                SwitchData(presetDataName != null);
            }, 50);



        }

        public void SwitchData(bool toNameType)
        {
            if (toNameType)
            {
                switchToDataName();

            }
            else
            {
                switchToDataValue();
            }
        }




        public (Func<T> getter, Action<T> setter) CreateGetterSetter<T>(GameObject gameObject)
        {
            (Func<T> getter, Action<T> setter) re = default;
            if (isStatic)
            {
                re.getter = () => ((DataValueNormalStyle<T>)dataInputFile).dataValue;
                re.setter = (v) => ((DataValueNormalStyle<T>)dataInputFile).dataValue = v;

            }
            else
            {
                EventData.EventDataHandler<T> eventDataHandler = EventData.EventDataF.GetData<T>(((DataName)dataInputFile).dataName, gameObject);
                re.getter = () => eventDataHandler.Data;
                re.setter = (v) => eventDataHandler.Data = v;
            }

            return re;
        }





    }









    // 这里是用来自定义每一种类型的数据输入框风格的
    public partial class InputDataNameOrData
    {
        [Serializable]
        public class DataBox
        {
            public Action<bool> switchDataAction = null;
            public Type dataType;
        }



        [Serializable]
        public class DataName : DataBox
        {
            [Button("动")]
            [PropertyOrder(-1)]
            [HorizontalGroup("A")]
            [HorizontalGroup("A/1", 40)]
            public void SwitchData()
            {
                switchDataAction?.Invoke(false);
            }
            [HideLabel]
            [ValueDropdown(nameof(dataNameList))]
            [HorizontalGroup("A/2")]
            public string dataName;


            private string[] dataNameList => EventData.DataNameF.GetAllNamesOnType(dataType).ToArray();
        }

        [Serializable]
        public class DataValueNormalStyle<T> : DataBox
        {
            [Button("静")]
            [PropertyOrder(-1)]
            [HorizontalGroup("A")]
            [HorizontalGroup("A/1", 40)]
            public void SwitchData()
            {
                switchDataAction?.Invoke(true);
            }
            [HideLabel]
            [HorizontalGroup("A/2")]
            public T dataValue;
        }
    }
}