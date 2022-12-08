using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Fc
{

    public static T FieldGetOrSet<T>(ref T field, Func<T> setField)
    {
        {
            if (field == null)
                field = setField();
        }
        return field;
    }



    
    private static Dictionary<System.Type, System.Object> FindOnlyObjectType_Dic = new Dictionary<Type, object>();
    public static T FindOnlyComponent<T>() where T : MonoBehaviour
    {
        object v;
        Dictionary<Type, object> dic = FindOnlyObjectType_Dic;

        bool hasKey = dic.TryGetValue(typeof(T), out v);
        if (!hasKey)
        {
            T cmp = GameObject.FindObjectOfType<T>();
            if (cmp != null)
            {
                v = cmp;
                dic.Add(typeof(T), v);
            }
        }
        return v as T;
    }


    
}
