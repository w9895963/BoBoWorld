using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EventData.DataName
{
    public static partial class Preset
    {
        //字典:类型判断正则表达
        public static Dictionary<System.Type, string> TypeMatchDict = new Dictionary<System.Type, string>(){
            {typeof(Vector2), @"(向量|施力|法线|位置)$"},
            {typeof(bool), @"(^是否)|(键$)"},
            {typeof(GameObject), @"物体$"},
            {typeof(float), @"值$"}
        };


    }
}

