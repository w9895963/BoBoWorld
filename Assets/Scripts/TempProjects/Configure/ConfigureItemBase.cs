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
        public abstract Type[] RequireComponentsOnGameObject { get; }
        // public abstract Type[] RequireComponents (GameObject gameObject);
        public abstract ItemRunnerBase CreateRunnerOver(GameObject gameObject);





        public abstract class ItemRunnerBase : IConfigureItemRunner
        {
            public GameObject gameObject;

            public abstract void OnUnInit();
            public abstract void OnDisable();
            public abstract void OnEnable();
            public abstract void OnInit();
        }

        public abstract class ItemRunnerBase<T> : ItemRunnerBase
        {
            public T config;
        }


    }

   






}



