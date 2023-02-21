using System;
using UnityEngine;



namespace Configure
{
    public interface IDataSetter<T>
    {
        public Action<T> CreateDataSetter(GameObject gameObject);
    }
    public interface IDataSetter
    {
        public Action<T> CreateDataSetter<T>(GameObject gameObject);
    }
}