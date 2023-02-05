using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventData;
using Sirenix.OdinInspector;
using UnityEngine;
using static Configure.Inspector.InputDataAndValue;

namespace Configure.Inspector
{

    [Serializable]
    public partial class InputDataAndValue
    {

        //数据名
        private void OnDataNameChanged()
        {
            // 赋默认值
            //当前期望数据类型
            var type = DataNameF.GetDataType(dataName);
            //当前数据类型
            Type valueType = value?.GetType();
            //如果不一致则新建对应类型的数据
            if (type != valueType)
            {
                //遍历InputItem找到对应类型的InputValue
                foreach (var item in typeof(InputItem).GetNestedTypes())
                {
                    //获得父类型的类型参数
                    var genericType = item.BaseType.GetGenericArguments()[0];
                    //如果类型参数一致则新建
                    if (genericType == type)
                    {
                        value = (InputValue)Activator.CreateInstance(item);
                        break;
                    }
                }
            }



        }
        private string[] dataNames => DataNameF.GetDataNamesList();



        [ValueDropdown(nameof(dataNames))]
        [LabelText("数据名")]
        [OnValueChanged(nameof(OnDataNameChanged))]
        public string dataName;


        //数据值
        [SerializeField]
        [SerializeReference]
        [InlineProperty]
        [HideLabel] 
        [HideReferenceObjectPicker]
        private InputValue value = new InputItem.InputValueInt();


        public System.Object dataValue => value.Value;



        [Serializable]
        public class InputValue
        {
            public virtual object Value => null;
        }
        [Serializable]
        public class InputValueT<T> : InputValue
        {

            [LabelText("数据值")]
            public T value;


            public override string ToString()
            {
                return value.ToString();
            }

            public override object Value => value;
        }
    }


    ///<summary> 数据值的细分类型 </summary>
    public partial class InputDataAndValue
    {
        [Serializable]
        public class InputItem
        {
            [Serializable]
            public class InputValueString : InputValueT<string>
            {
            }
            [Serializable]
            public class InputValueInt : InputValueT<int>
            {
            }
            [Serializable]
            public class InputValueFloat : InputValueT<float>
            {
            }
            [Serializable]
            public class InputValueBool : InputValueT<bool>
            {
            }
            [Serializable]
            public class InputValueVector2 : InputValueT<Vector2>
            {
            }
            [Serializable]
            public class InputValueVector3 : InputValueT<Vector3>
            {
            }

        }
    }

}




