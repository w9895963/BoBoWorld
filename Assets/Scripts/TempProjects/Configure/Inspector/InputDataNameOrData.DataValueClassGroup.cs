using System;
using UnityEngine;


namespace Configure.Inspector
{
    public partial class InputDataNameOrData
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
            [Serializable] public class DataValueTest : DataValueNormalStyle<Test> { }
        }



        [Serializable]
        public class Test{
            public int a;
            public float b;
            public string c;
            public bool d;
            public Vector2 e;
            public Vector3 f;
            public Vector4 g;
            public Color h;
            public GameObject i;
        }




    }
}