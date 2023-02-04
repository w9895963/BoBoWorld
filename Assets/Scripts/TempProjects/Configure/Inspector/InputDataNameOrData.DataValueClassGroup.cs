using System;
using UnityEngine;


namespace Configure.Inspector
{
    public partial class InputDataOrValue
    {
        //可以创建的类型
        public class DataValueClassGroup
        {
            [Serializable] public class DataValueInt : DataValueNormalStyle<int> { }
            [Serializable] public class DataValueFloat : DataValueNormalStyle<float> { }
            [Serializable] public class DataValueString : DataValueNormalStyle<string> { }
            [Serializable] public class DataValueBool : DataValueNormalStyle<bool> { }
            [Serializable] public class DataValueVector2 : DataValueNormalStyle<Vector2> { }
            [Serializable] public class DataValueVector3 : DataValueNormalStyle<Vector3> { }
            [Serializable] public class DataValueVector4 : DataValueNormalStyle<Vector4> { }
            [Serializable] public class DataValueColor : DataValueNormalStyle<Color> { }
            [Serializable] public class DataValueGameObject : DataValueNormalStyle<GameObject> { }
        }







    }
}