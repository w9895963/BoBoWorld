using System;
using UnityEngine;


//命名空间：配置
namespace ConfigureS
{
    //类:配置基类
    public class ConfigureBase : ScriptableObject
    {
        //字段:启用器
        public (Action Enable, Action Disable) enabler = (null, null);





        public virtual (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
        {
            return (null, null);
        }





    }

}

