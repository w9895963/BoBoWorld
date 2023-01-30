using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;





public static class Extension_Type
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
}
