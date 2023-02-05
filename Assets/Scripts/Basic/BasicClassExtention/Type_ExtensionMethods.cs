using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;





public static partial class ExtensionMethods
{


    ///<summary>获得类型的所有子类</summary>
    public static List<Type> GetSubTypes(this Type type, string nameSpace = null)
    {
        List<Type> types = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            Type[] typesInAssembly = assembly.GetTypes();
            foreach (Type t in typesInAssembly)
            {
                if (t.IsSubclassOf(type))
                {
                    types.Add(t);
                }
            }
        }
        return types;
    }

    ///<summary>获得类型的类型名, 带有类型参数</summary>
    public static string GetName(this Type type)
    {
        string typeName = type.Name;
        if (type.IsGenericType)
        {
            typeName = typeName.Substring(0, typeName.IndexOf('`'));
            typeName += "<";
            Type[] genericTypes = type.GetGenericArguments();
            typeName += genericTypes.Select(x => GetName(x)).Join(",");
            typeName += ">";
        }
        return typeName;
    }



}
