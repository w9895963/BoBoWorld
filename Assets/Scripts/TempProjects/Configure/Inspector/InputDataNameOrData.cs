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
    // [LabelWidth(120)]
    // [HorizontalGroup("A", 0.5f)]
    public partial class InputDataOrValue
    {
        [SerializeField]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        private DataBox dataInputFile;




        //
        private Type dataInputFile_DataType;
        private string presetDataName;
        private Object storeData;
        private bool isStatic = true;
        private Type dataInputFile_ObjectType;
        private DataBox dataInputFile_DataObject;
        private DataBox dataInputFile_NameObject;
        private Action switchToDataName;
        private Action switchToDataValue;

        public InputDataOrValue(Type dataInputFileType, string presetDataName = null, Object presetData = null, bool? forceStatic = null, bool hideSwicher = false, bool isUseDataForTrigger = true)
        {
            this.dataInputFile_DataType = dataInputFileType;
            this.presetDataName = presetDataName;
            this.storeData = presetData;
            this.isStatic = forceStatic ?? (presetData != null);



            dataInputFile_ObjectType = typeof(DataValueClassGroup).GetNestedTypes().FirstOrDefault(t => t.GetField(nameof(DataValueNormalStyle<Object>.dataValue)).FieldType == dataInputFileType);
            if (dataInputFile_ObjectType != null)
            {
                dataInputFile_DataObject = (DataBox)Activator.CreateInstance(dataInputFile_ObjectType);
                dataInputFile_DataObject.hideSwicher = hideSwicher;
            }
            dataInputFile_NameObject = new DataName()
            {
                dataName = presetDataName,
                isUseDataForCondition = isUseDataForTrigger,
                hideSwicher = hideSwicher,
            };



            switchToDataName = () =>
            {
                dataInputFile = dataInputFile_NameObject;
                dataInputFile.switchDataAction += SwitchData;
                dataInputFile.dataType = dataInputFileType;
                dataInputFile.SetDataName(presetDataName);
            };

            switchToDataValue = () =>
            {
                if (dataInputFile_ObjectType == null)
                {
                    Debug.LogError("Configure.Inspector.InputDataNameOrData.DataValueClassGroup 内没有找到可以创建的类型");
                }
                else
                {
                    dataInputFile = dataInputFile_DataObject;
                    dataInputFile.switchDataAction += SwitchData;
                    dataInputFile.dataType = dataInputFileType;
                    dataInputFile.SetDataValue(storeData);

                }
            };




            TimerF.WaitUpdate_InEditor(() =>
            {
                SwitchData(this.isStatic);
            }, 50);



        }

        public void SwitchData(bool toNameType)
        {
            if (toNameType)
            {
                switchToDataName();
                isStatic = false;

            }
            else
            {
                switchToDataValue();
                isStatic = true;
            }
        }






        public EventData.EventDataHandler<T> CreateDataHandler<T>(GameObject gameObject)
        {
            EventData.EventDataHandler<T> re = null;
            if (isStatic)
            {
                re = EventData.EventDataF.CreateSimpleData<T>((T)dataInputFile.GetDataValue());

            }
            else
            {
                re = EventData.EventDataF.GetData<T>(((DataName)dataInputFile).dataName, gameObject);
            }



            return re;
        }

        public OutDataHolder<T> CreateDataHandlerInstance<T>(GameObject gameObject)
        {
            EventData.EventDataHandler<T> dataHander = null;
            if (isStatic)
            {
                dataHander = EventData.EventDataF.CreateSimpleData<T>((T)dataInputFile.GetDataValue());

            }
            else
            {
                dataHander = EventData.EventDataF.GetData<T>(((DataName)dataInputFile).dataName, gameObject);
            }

            OutDataHolder<T> re = new OutDataHolder<T>(dataHander);

            return re;
        }


    }


    public class OutDataHolder<T>
    {

        public T data { get => dataHandler.Data; set => dataHandler.Data = value; }
        public (EventData.Core.EventData, Func<bool>) condition => dataHandler.OnUpdateCondition;


        public OutDataHolder(EventData.EventDataHandler<T> dataHandler)
        {
            this.dataHandler = dataHandler;
        }



        private EventData.EventDataHandler<T> dataHandler;
    }






    // 这里是用来创建不同类型的数据输入界面的
    public partial class InputDataOrValue
    {
        [Serializable]
        public class DataBox
        {
            protected Func<Object> dataValueGetter = null;
            protected Action<Object> dataValueSetter = null;

            [HideInInspector]
            public bool hideSwicher = false; //是否隐藏切换按钮
            public Action<bool> switchDataAction = null;
            public Type dataType;


            public void SetDataName(string dataName)
            {
                if (this is DataName)
                {
                    ((DataName)this).dataName = dataName;
                }
            }

            public void SetDataValue(Object dataValue)
            {
                if (dataValue != null)
                    dataValueSetter?.Invoke(dataValue);
            }

            public Object GetDataValue()
            {
                return dataValueGetter?.Invoke();
            }
        }



        [Serializable]
        public class DataName : DataBox
        {


            [HideLabel]
            [ValueDropdown(nameof(dataNameList))]
            [HorizontalGroup("A", 0)]
            public string dataName;



            [Button("更")]
            [PropertyTooltip("是否将数据作为更新的触发条件")]
            [GUIColor(nameof(SwitcGenerateCondition_ButtonColor))]
            [PropertyOrder(1)]
            [HorizontalGroup("A", 40, MinWidth = 25)]
            [HideIf(nameof(hideSwicher))]
            public void SwitcGenerateCondition()
            {
                isUseDataForCondition = !isUseDataForCondition;
            }



            [Button("动")]
            [PropertyTooltip("动态数据:会在运行时根据名字获取数据\n静态数据:会在运行时直接获取设定的数据")]
            [PropertyOrder(1)]
            [HorizontalGroup("A", 40, MinWidth = 25)]
            [HideIf(nameof(hideSwicher))]
            private void SwitchData()
            {
                switchDataAction?.Invoke(false);
            }






            //是否生成更新条件
            [HideInInspector]
            public bool isUseDataForCondition = false;


            private Color SwitcGenerateCondition_ButtonColor => isUseDataForCondition ? Color.green : Color.grey;
            private string[] dataNameList => EventData.DataNameF.GetAllNamesOnTypeWithGroup(dataType).ToArray();

        }

        [Serializable]
        public class DataValueNormalStyle<T> : DataBox
        {



            [HideLabel]
            [HorizontalGroup("A")]
            public T dataValue;

            public DataValueNormalStyle()
            {
                dataValueGetter = () => dataValue;
                dataValueSetter = (v) => dataValue = (T)v;
            }

            [Button("静")]
            [PropertyTooltip("动态数据:会在运行时根据名字获取数据\n静态数据:会在运行时直接获取设定的数据")]
            [PropertyOrder(1)]
            [HorizontalGroup("A", 40, MinWidth = 25)]
            [HideIf(nameof(hideSwicher))]
            public void SwitchData()
            {
                switchDataAction?.Invoke(true);
            }
        }
    }
}