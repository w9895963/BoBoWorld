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
            System.Action<int> setData = EventDataF.GetDataSetter<int>(dataName, gameObject);
            setData(1);
        }

    }


    //方法：将某个配置添加到eventData中
    public void AddConfigToEventData<T, O>(GameObject gameObject = null) where T : ScriptableObject
    {
        //获取配置
        T config = GetConfig<T>();
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
                foreach (object enumValue in System.Enum.GetValues(typeof(O)))
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

