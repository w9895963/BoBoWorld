using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static partial class ExtensionMethods
{


    





    






    ///<summary>获取枚举的全名,包含类</summary>
    public static string GetFullName(this System.Enum enumName)
    {
        //获取枚举类型
        string typeName = enumName.GetType().FullName;
        //获取枚举名称
        string name = enumName.ToString();


        return typeName + "." + name;
    }
   

}
