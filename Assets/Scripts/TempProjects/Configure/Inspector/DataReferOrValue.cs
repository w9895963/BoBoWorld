using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;


namespace Configure.Inspector
{
    /// <summary>
    //*公共类
    public partial class DataReferOrValue<T> : IDataGetter<T>, IDataSetter<T>
    {
        public Func<T> CreateGetter(GameObject gameObject)
        {
           if (isDataInstance)
           {
               return dataInstance.CreateDataGetter<T>(gameObject);
           }
           else
           {
               return () => value;
           }
        }

        public Action<T> CreateSetter(GameObject gameObject)
        {
            if (isDataInstance)
            {
                return dataInstance.CreateDataSetter<T>(gameObject);
            }
            else
            {
                return (v) => value = v;
            }
        }
    }





    //*界面
    [Serializable]
    [InlineProperty]
    public partial class DataReferOrValue<T>
    {
        //内部方法
        private bool isDataInstance = false;



        //界面
        [SerializeField]
        [HideLabel]
        [HorizontalGroup("A")]
        [HideIf(nameof(isDataInstance))]
        private T value;
        [SerializeField]
        [InlineProperty]
        [HideLabel]
        [HorizontalGroup("A")]
        [ShowIf(nameof(isDataInstance))]
        private DataInstance dataInstance = new DataInstance(typeof(T));
        [Button("$" + nameof(ButtonName))]
        [PropertyTooltip("切换数据类型,从动态数据和静态数据中切换")]
        [HorizontalGroup("A", width: 26)]
        [GUIColor(nameof(ButtonColor))]
        [PropertyOrder(9)]
        private void ChangedDataTypeButton()
        {
            //切换数据类型
            isDataInstance = !isDataInstance;
            //如果时静态数据, 则将动态数据从引用中移除, 否则启用
            if (!isDataInstance)
            {
                dataInstance.Disable();
            }
            else
            {
                dataInstance.Enable();
            }
        }
        private Color ButtonColor()
        {
            return isDataInstance ? Color.green : Color.yellow;
        }
        private string ButtonName => isDataInstance ? "动" : "静";





        private void InspectorInit()
        {

        }


    }





    //*构造函数
    public partial class DataReferOrValue<T>
    {
        public class Config
        {
            public Type dataType;
            public string defaultDataName;
            public T defaultValue;

        }




        public DataReferOrValue(Func<Config, Config> setConfig)
        {
            _config = setConfig(new Config());
            InspectorInit();
        }



        private Config _config;



    }







    public interface IDataGetter<T>
    {
        Func<T> CreateGetter(GameObject gameObject);




        public virtual OptionalData Optional => new();
        public class OptionalData
        {
        }
    }



    public interface IDataSetter<T>
    {
        Action<T> CreateSetter(GameObject gameObject);

    }




}