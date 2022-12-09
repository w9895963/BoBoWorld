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
                        //将enumValue转换为类型enum
                        
                    }
                }


            }

        }
    }

    //方法：根据输入参数，将某个配置添加到eventData中
  /*   public void AddConfigToEventData(object fieldValue, System.Type type, GameObject gameObject = null)
    {
        //如果字段的类型为int
        if (type == typeof(int))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<int>(fieldValue, gameObject);
        }
        //如果字段的类型为float
        else if (type == typeof(float))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<float>(fieldValue, gameObject);
        }
        //如果字段的类型为string
        else if (type == typeof(string))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<string>(fieldValue, gameObject);
        }
        //如果字段的类型为bool
        else if (type == typeof(bool))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<bool>(fieldValue, gameObject);
        }
        //如果字段的类型为Vector2
        else if (type == typeof(Vector2))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<Vector2>(fieldValue, gameObject);
        }
        //如果字段的类型为Vector3
        else if (type == typeof(Vector3))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<Vector3>(fieldValue, gameObject);
        }
        //如果字段的类型为Vector4
        else if (type == typeof(Vector4))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<Vector4>(fieldValue, gameObject);
        }
        //如果字段的类型为Color
        else if (type == typeof(Color))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<Color>(fieldValue, gameObject);
        }
        //如果字段的类型为GameObject
        else if (type == typeof(GameObject))
        {
            //将字段的值添加到eventData中
            EventDataF.GetDataSetter<GameObject>(fieldValue, gameObject);
        }
    } */

}

