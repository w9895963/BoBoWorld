using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventData;
using UnityEngine;
using static Configure.InspectorInterface.InputDataAndValue;

namespace Configure.InspectorInterface
{

    [Serializable]
    public partial class InputDataAndValue
    {

        //数据名
        private void OnDataNameChanged()
        {
            // 赋默认值
            //当前期望数据类型
            var type = DataNameF.GetType(dataName);
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
        [NaughtyAttributes.AllowNesting]
        [NaughtyAttributes.OnValueChanged(nameof(OnDataNameChanged))]
        [NaughtyAttributes.Label("数据名")]
        [NaughtyAttributes.Dropdown(nameof(dataNames))]
        public string dataName;


        //数据值
        [SerializeField]
        [SerializeReference]
        [StackableDecorator.StackableField]
        [StackableDecorator.HorizontalGroup("info1", true, "", 0, -1, prefix = true, title = "数据值")]
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
            [StackableDecorator.StackableField]
            [StackableDecorator.Label(0)]
            public T value;


            public override string ToString()
            {
                return value.ToString();
            }

            public override object Value => value;
        }
    }


    //数据值的细分类型
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




