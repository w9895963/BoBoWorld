using System;
using UnityEngine;



namespace Configure
{
    interface IDataSetter<T>
    {
        public Action<T> CreateDataSetter(GameObject gameObject);
    }
    interface IDataSetter
    {
        public Action<T> CreateDataSetter<T>(GameObject gameObject);
    }
}