using System.Collections;
using System.Collections.Generic;
using EventData;
using UnityEngine;




//类：配置管理器
public class ConfigManager : MonoBehaviour
{
    //字段: 配置列表
    public List<ScriptableObject> configList = new List<ScriptableObject>();

    //方法：获取配置
    public T GetConfig<T>() where T : ScriptableObject
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
    public void AddConfigToEventData(System.Enum dataName, object dataValue, System.Type type, GameObject gameObject = null)
    {
        //如果字段的类型为int
        if (type == typeof(int))
        {
            //将字段的值添加到eventData中
            EventDataF.GetData_local<int>(gameObject, dataName).Data= (int)dataValue;
        }
        //如果字段的类型为float
        else if (type == typeof(float))
        {
            //将字段的值添加到eventData中
            System.Action<float> setData = EventDataF.GetDataSetter<float>(dataName, gameObject);
            setData((float)dataValue);
        }
        //如果字段的类型为string
        else if (type == typeof(string))
        {
            //将字段的值添加到eventData中
            System.Action<string> setData = EventDataF.GetDataSetter<string>(dataName, gameObject);
            setData((string)dataValue);
        }
        //如果字段的类型为bool
        else if (type == typeof(bool))
        {
            //将字段的值添加到eventData中
            System.Action<bool> setData = EventDataF.GetDataSetter<bool>(dataName, gameObject);
            setData((bool)dataValue);
        }
        //如果字段的类型为Vector2
        else if (type == typeof(Vector2))
        {
            //将字段的值添加到eventData中
            System.Action<Vector2> setData = EventDataF.GetDataSetter<Vector2>(dataName, gameObject);
            setData((Vector2)dataValue);
        }
        //如果字段的类型为Vector3
        else if (type == typeof(Vector3))
        {
            //将字段的值添加到eventData中
            System.Action<Vector3> setData = EventDataF.GetDataSetter<Vector3>(dataName, gameObject);
            setData((Vector3)dataValue);
        }
        //如果字段的类型为Vector4
        else if (type == typeof(Vector4))
        {
            //将字段的值添加到eventData中
            System.Action<Vector4> setData = EventDataF.GetDataSetter<Vector4>(dataName, gameObject);
            setData((Vector4)dataValue);
        }
        //如果字段的类型为Color
        else if (type == typeof(Color))
        {
            //将字段的值添加到eventData中
            System.Action<Color> setData = EventDataF.GetDataSetter<Color>(dataName, gameObject);
            setData((Color)dataValue);
        }
        //如果字段的类型为GameObject
        else if (type == typeof(GameObject))
        {
            //将字段的值添加到eventData中
            System.Action<GameObject> setData = EventDataF.GetDataSetter<GameObject>(dataName, gameObject);
            setData((GameObject)dataValue);
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

