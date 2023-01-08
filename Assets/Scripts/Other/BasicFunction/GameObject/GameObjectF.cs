using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectF
{




    public static T FindComponentOrCreateObject<T>() where T : UnityEngine.Component
    {
        T comp = GameObject.FindObjectOfType<T>();
        if (comp == null)
        {
            comp = new GameObject(typeof(T).Name).AddComponent<T>();
        }
        return comp;
    }




    public static T FindComponentOrCreate<T>(string objectName) where T : UnityEngine.Component
    {
        var comps = GameObject.FindObjectsOfType<T>();
        T comp = comps.FirstOrDefault((x) => x.name == objectName);
        if (comp == null)
        {
            comp = new GameObject(objectName).AddComponent<T>();
        }
        return comp;
    }





    public static T FindObjectOfType<T>(string name = null) where T : UnityEngine.Object
    {
        if (name == null)
        {
            return GameObject.FindObjectOfType<T>();
        }
        else
        {
            T[] ts = GameObject.FindObjectsOfType<T>();
            T t = ts.ToList().Find((x) => x.name == name);
            return t;
        }

    }

    ///<summary>安全创建一个空游戏物体</summary>
    public static GameObject CreateGameObjectSafe(string name = null)
    {
        string path = "Prefab/GameObject";
        //载入资源，从Resources文件夹下
        GameObject prefab = Resources.Load<GameObject>(path);
        //实例化
        GameObject obj = GameObject.Instantiate(prefab);
        if (name != null)
        {
            obj.name = name;
        }

        return obj;

    }


    ///<summary>获得某个名字的物体，如果没有就创建一个</summary>
    private static Dictionary<string, GameObject> nameToGameObject = new Dictionary<string, GameObject>();
    public static GameObject GetObjectByNameOrCreate(string name)
    {
        //要找到的物体
        GameObject gameObject = null;
        nameToGameObject.TryGetValue(name, out gameObject);
        //如果字典里没有
        if (gameObject == null)
        {
            gameObject = GameObject.Find(name);

            if (gameObject == null)
            {
                //创建一个
                gameObject = CreateGameObjectSafe(name);
            }
            //添加到字典
            nameToGameObject[name] = gameObject;

        }

        return gameObject;
    }




    public static GameObject[] GetObjectsInLayer(Layer layer)
    {
        var ts = GameObject.FindObjectsOfType<GameObject>();
        return ts.Where((x) => x.layer == (int)layer).ToArray();
    }



    public static GameObject CreateGameObjectWithComponent<T>(string name = null) where T : MonoBehaviour
    {
        GameObject obj = new GameObject(name);
        obj.AddComponent<T>();
        return obj;
    }


    public static T CreateComponent<T>(GameObject gameObject = null) where T : MonoBehaviour
    {
        if (gameObject == null)
        {
            gameObject = new GameObject();
        }


        T cmp = gameObject.AddComponent<T>();
        return cmp;
    }



}
