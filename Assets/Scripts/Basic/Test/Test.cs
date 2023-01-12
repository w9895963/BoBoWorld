using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

public class Test : MonoBehaviour
{
    //*Unity界面
    public UnityEngine.Object obj;
    public GameObject gameObj;


    [SerializeReference, SubclassSelector]
    public TestClass test = new TestClass3();













    public void TestMethod2(ref Vector2 obj)
    {
        Type type = obj.GetType();
        type.Name.Log();
    }

    [Serializable]
    public class TestClass
    {
        public int a;
        public int b;
        public int c;
    }

    public class TestClass2<T> : TestClass
    {
        public T d;
    }

    public class TestClass3 : TestClass2<string>
    {

    }



    private void Start()
    {
        BasicEvent.OnUpdate.Add(gameObject, NewMethod);
        BasicEvent.OnUpdate.Add(gameObject, NewMethod);
        BasicEvent.OnUpdate.Remove(gameObject, NewMethod);
        BasicEvent.OnUpdate.Remove(gameObject, NewMethod);


        static void NewMethod()
        {

            //log 时间
            Debug.Log(Time.time);

        }
    }









    //*测试方法1
    [Button]
    public void 测试方法1()
    {
        //生成一个随机数然后打印到unix的的控制台。
        typeof(string).Log();
        // SpriteShape



    }







}
