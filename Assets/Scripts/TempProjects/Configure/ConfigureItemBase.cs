using System;
using Configure.ConfigureItems;

using UnityEngine;



//命名空间：配置
namespace Configure
{

    [System.Serializable]
    public abstract class ConfigureItemBase : ConfigureItem
    {
        public abstract string MenuName { get; }
        public abstract Type[] RequireComponents { get; }
        public abstract ItemRunnerBase CreateRunnerOver(GameObject gameObject);
       




        public abstract class ItemRunnerBase : IConfigureItemRunner
        {
            public GameObject gameObject;

            public abstract void Destroy();
            public abstract void Disable();
            public abstract void Enable();
            public abstract void Init();
        }

        public abstract class ItemRunnerBase<T> : ItemRunnerBase
        {
            public T config;
        }


    }






}



