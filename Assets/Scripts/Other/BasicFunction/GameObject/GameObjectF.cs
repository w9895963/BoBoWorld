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

    public static GameObject FindObjectByNameOrCreate(string name)
    {
        GameObject gameObject = GameObject.Find(name);
        if (gameObject == null)
        {
            gameObject = new GameObject(name);
        }
        return gameObject;
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
