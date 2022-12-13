using System.Collections;
using System.Collections.Generic;
using EventDataS;
using UnityEngine;




//类：配置管理器
public class ConfigManager : MonoBehaviour
{
    //字段: 配置列表,能在编辑器中显示
    [SerializeField]
    private List<ScriptableObject> configList = new List<ScriptableObject>();

    //方法：获取配置
    private T GetConfig<T>() where T : ScriptableObject
    {
        //遍历configList
        T configObj = null;
        foreach (ScriptableObject config in configList)
        {
            //如果config的类型为T
            if (config.GetType() == typeof(T))
            {
                configObj = config as T;
            }
        }
        //如果没有找到，返回null
        return configObj;
    }



    //方法：根据输入参数，将某个配置添加到eventData中
    private void AddConfigToEventData(System.Enum dataName, object dataValue, System.Type type, GameObject gameObject = null)
    {
        //如果字段的类型为int
        if (type == typeof(int))
        {
            //将字段的值添加到eventData中
            SetData<int>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为float
        else if (type == typeof(float))
        {
            //将字段的值添加到eventData中
            SetData<float>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为string
        else if (type == typeof(string))
        {
            //将字段的值添加到eventData中
            SetData<string>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为bool
        else if (type == typeof(bool))
        {
            //将字段的值添加到eventData中
            SetData<bool>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为Vector2
        else if (type == typeof(Vector2))
        {
            //将字段的值添加到eventData中
            SetData<Vector2>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为Vector3
        else if (type == typeof(Vector3))
        {
            //将字段的值添加到eventData中
            SetData<Vector3>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为Vector4
        else if (type == typeof(Vector4))
        {
            //将字段的值添加到eventData中
            SetData<Vector4>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为Color
        else if (type == typeof(Color))
        {
            //将字段的值添加到eventData中
            SetData<Color>(dataName, dataValue, gameObject);
        }
        //如果字段的类型为GameObject
        else if (type == typeof(GameObject))
        {
            //将字段的值添加到eventData中
            SetData<GameObject>(dataName, dataValue, gameObject);
        }


        static void SetData<T>(System.Enum dataName, object dataValue, GameObject gameObject)
        {

            EventDataF.GetData<T>(gameObject, dataName).Data = (T)dataValue;
        }
    }




    /// <summary>将配置添加到对应的数据中 , ConfigT:要添加的配置类型，EnumT:对应的字段类型</summary>
    public void AddConfigToEventData<ConfigT, EnumT>(GameObject gameObject = null) where ConfigT : ScriptableObject
    {

        //获取配置
        ConfigT config = GetConfig<ConfigT>();
        //如果配置存在
        if (config != null)
        {
            //用反射来历遍config的所有字段，将字段添加到eventData中
            System.Reflection.FieldInfo[] fieldInfos = config.GetType().GetFields();
            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfos)
            {
                //获取字段的值
                object fieldValue = fieldInfo.GetValue(config);
                //获得字段的类型
                System.Type fieldType = fieldInfo.FieldType;
                //获得字段的名称
                string fieldName = fieldInfo.Name;
                //历遍枚举O
                foreach (object enumValue in System.Enum.GetValues(typeof(EnumT)))
                {
                    //如果字段的名称和枚举O的名称相同
                    if (fieldName == enumValue.ToString())
                    {
                        //获得数据名称枚举
                        System.Enum dataName = (System.Enum)enumValue;
                        //获得数据的值
                        object dataValue = fieldValue;
                        //获得数据的类型
                        System.Type dataType = fieldType;
                        //将字段添加到eventData中
                        AddConfigToEventData(dataName, dataValue, dataType, gameObject);
                    }
                }


            }

        }
    }

}

