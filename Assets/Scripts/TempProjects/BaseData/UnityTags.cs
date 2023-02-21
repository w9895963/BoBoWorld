using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public static partial class BaseData
{
    /// <summary> 所有Unity内置Tag </summary>
    public static string[] UnityTags => unityTags ?? unityTags_Init();
    //Unity Tag
    private static string[] unityTags;
    private static string[] unityTags_Init()
    {
        unityTags = new string[0];
#if UNITY_EDITOR
        unityTags = UnityEditorInternal.InternalEditorUtility.tags;
#endif
        return unityTags;
    }

}
