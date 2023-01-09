using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviour_Extension {
    public static GameObject CreateChild (this MonoBehaviour mono, GameObject source) {
        GameObject gameObject = GameObject.Instantiate (source, mono.transform);
        return gameObject;
    }

    public static List<GameObject> GetAllChild (this MonoBehaviour mono) {
        List<GameObject> list = new List<GameObject> ();
        for (int i = 0; i < mono.transform.childCount; i++) {
            list.Add (mono.transform.GetChild (i).gameObject);
        }
        return list;
    }
    public static void DestroyChildren (this MonoBehaviour mono) {
        int childCount = mono.
        transform.
        childCount;
        for (int i = 0; i < childCount; i++) {
            GameObject.Destroy (mono.transform.GetChild (i).gameObject);
        }
    }

   
    public static void DestroyObject (this MonoBehaviour component) {
        GameObject.Destroy (component.gameObject);
    }


}