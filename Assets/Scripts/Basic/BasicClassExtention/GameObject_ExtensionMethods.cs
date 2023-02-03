using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static partial class ExtensionMethods
{
    #region Destroy
    public static void Destroy(this List<Object> objects)
    {
        foreach (var obj in objects)
        {
            GameObject.Destroy(obj);
        }
    }
    public static void Destroy(this List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            GameObject.Destroy(obj);
        }
    }
    public static void Destroy(this Object[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                GameObject.Destroy(obj);
        }
    }


    public static void Destroy(this Object obj, float timeWait = 0)
    {
        if (timeWait <= 0)
        {
            GameObject.Destroy(obj);
        }
        else
        {
            GameObject.Destroy(obj, timeWait);
        }
    }



    public static void DestroyImmediate(this Object obj)
    {
        GameObject.DestroyImmediate(obj);
    }


    #endregion
    // * Region Destroy End---------------------------------- 



    #region Component Or Relation
    public static GameObject CreateChild(this GameObject gameObject, string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = gameObject.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        return obj;
    }
    public static GameObject CreateChild(this GameObject gameObject, GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate(prefab, gameObject.transform);
        return obj;
    }


    public static GameObject[] DestroyChildren(this GameObject gameObject)
    {
        List<GameObject> gameObjectsDestroyed = gameObject.GetDirectChildren();
        gameObjectsDestroyed.Destroy();
        return gameObjectsDestroyed.ToArray();
    }


    public static List<GameObject> GetDirectChildren(this GameObject gameObject)
    {
        List<GameObject> childlist = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            childlist.Add(gameObject.transform.GetChild(i).gameObject);
        }
        return childlist;
    }
    public static List<GameObject> GetAllChildren(this GameObject gameObject)
    {
        List<GameObject> childlist = new List<GameObject>();

        List<GameObject> currCheck = GetDirectChildren(gameObject);

        do
        {
            childlist.AddRange(currCheck);
            List<GameObject> childrenCollect = new List<GameObject>();
            currCheck.ForEach((obj) =>
            {
                List<GameObject> children = GetDirectChildren(obj);
                childrenCollect.AddRange(children);
            });
            currCheck = childrenCollect;

        } while (currCheck.Count > 0);


        return childlist;
    }

    public static List<GameObject> GetAllChildAndSelf(this GameObject gameObject)
    {
        List<GameObject> gameObjects = GetAllChildren(gameObject);
        gameObjects.Add(gameObject);
        return gameObjects;
    }


    public static void SetParent(this GameObject gameObject, GameObject parent, bool stay = true)
    {
        Transform parentT = null;
        if (parent != null)
        {
            parentT = parent.transform;
        }
        gameObject.transform.SetParent(parentT, stay);
    }
    public static void SetParent(this GameObject gameObject, Component parentComp, bool stay = true)
    {
        GameObject parent = parentComp == null ? null : parentComp.gameObject;
        SetParent(gameObject, parent, stay);
    }

    public static List<GameObject> GetParentsAndSelf(this GameObject gameObject)
    {
        List<GameObject> all = GetParents(gameObject);
        if (gameObject) all.Add(gameObject);
        return all;
    }

    public static GameObject GetParent(this GameObject gameObject)
    {
        Transform parent = gameObject.transform.parent;
        if (parent == null) return null;
        return parent.gameObject;
    }

    public static List<GameObject> GetParents(this GameObject gameObject)
    {
        List<GameObject> all = new List<GameObject>();
        GameObject parent = GetParent(gameObject);
        while (parent != null)
        {
            all.Add(parent);
            parent = GetParent(parent);
        }
        return all;
    }


    public static bool IsChildOf(this GameObject gameObject, GameObject parent)
    {
        List<GameObject> parents = gameObject.GetParents();
        if (parent == null)
        {
            return parents.Count == 0;
        }
        return parents.Contains(parent);
    }


    public static T GetComponent<T>(this GameObject gameObject, bool autoAdd = false) where T : Component
    {

        T com = gameObject.GetComponent<T>();
        if (autoAdd == false)
        {
            return com;
        }
        if (com == null)
        {
            com = gameObject.AddComponent<T>();
        }
        return com;
    }
    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        T com = gameObject.GetComponent<T>();

        if (com != null)
        {
            GameObject.Destroy(com);
        }
    }
    public static void RemoveComponentAll<T>(this GameObject gameObject) where T : Component
    {
        T[] com = gameObject.GetComponents<T>();

        if (com.Length > 0)
        {
            com.Destroy();
        }
    }


    public static bool HasComponent<T>(this GameObject gameObject)
    {
        T tr;
        bool v = gameObject.TryGetComponent<T>(out tr);
        return v;
    }



  




    #endregion
    // * Region Component End---------------------------------- 





    #region Tansform


    #region +Position
    public static Vector2 GetPosition2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.position;
    }
    public static Vector2 GetPosition2dLo(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.localPosition;
    }


    public static void SetPositionLo(this GameObject gameObject, Vector3 p)
    {
        Vector2 position = p;
        gameObject.transform.localPosition = p;
    }
    public static void SetPositionLo(this GameObject gameObject, Vector2 p)
    {
        SetPositionLo(gameObject, p.ToVector3(gameObject.transform.localPosition.z));
    }

    public static void SetPositionLo(this GameObject gameObject, float? x, float? y)
    {
        Vector3 l = gameObject.transform.localPosition;
        SetPositionLo(gameObject, new Vector3(x ?? l.x, y ?? l.y, l.z));
    }
    public static void SetPosition(this GameObject gameObject, Vector3 p)
    {
        gameObject.transform.position = p;
    }
    public static void SetPosition(this GameObject gameObject, Vector2 p)
    {
        Vector2 position = p;
        Vector3 vector3 = new Vector3(position.x, position.y, gameObject.transform.position.z);
        SetPosition(gameObject, vector3);
    }
    public static void SetPosition(this GameObject gameObject, float? x, float? y)
    {
        Vector3 l = gameObject.transform.position;
        SetPosition(gameObject, new Vector2(x ?? l.x, y ?? l.y));
    }



    public static void AddPosition(this GameObject gameObject, Vector2 vector2)
    {
        gameObject.transform.position += new Vector3(vector2.x, vector2.y, 0);
    }
    public static void AddPosition(this GameObject gameObject, float x, float y)
    {
        AddPosition(gameObject, new Vector2(x, y));
    }

    public static void MovePosition(this GameObject gameObject, Vector2 direction, float distance)
    {
        Vector2 pathVc = direction.normalized * distance;
        Vector2 p = gameObject.GetPosition2d();
        p += pathVc;
        gameObject.SetPosition(p);
    }

    #endregion
    // * Region +Position End---------------------------------- 




    #region +Rotation
    public static float GetRotate1D(this GameObject gameObject)
    {
        return gameObject.transform.rotation.eulerAngles.z;
    }
    public static float GetRotate1DLo(this GameObject gameObject)
    {
        return gameObject.transform.localRotation.eulerAngles.z;
    }


    public static void SetRotateLo(this GameObject gameObject, float angle)
    {
        Quaternion rotation = gameObject.transform.localRotation;
        Vector3 angle3 = rotation.eulerAngles;
        angle3.z = angle;
        rotation.eulerAngles = angle3;
        gameObject.transform.localRotation = rotation;
    }
    public static void SetRotateLo(this GameObject gameObject, float? angle)
    {
        if (angle == null) return;
        Quaternion rotation = gameObject.transform.localRotation;
        Vector3 angle3 = rotation.eulerAngles;
        angle3.z = angle.Value;
        rotation.eulerAngles = angle3;
        gameObject.transform.localRotation = rotation;
    }
    public static void SetRotate(this GameObject gameObject, float angle)
    {
        Quaternion rotation = gameObject.transform.rotation;
        Vector3 angle3 = rotation.eulerAngles;
        angle3.z = angle;
        rotation.eulerAngles = angle3;
        gameObject.transform.rotation = rotation;
    }
    public static void SetRotate(this GameObject gameObject, Vector3 rotation)
    {
        Quaternion ro = new Quaternion();
        ro.eulerAngles = rotation;
        gameObject.transform.rotation = ro;
    }
    #endregion
    // * Region +Rotation End---------------------------------- 




    #region +Scale
    public static Vector2 GetScale2d(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.lossyScale;
    }
    public static Vector2 GetScale2dLo(this GameObject gameObject)
    {

        return (Vector2)gameObject.transform.localScale;
    }
    public static void SetScale(this GameObject gameObject, Vector3 worldScale)
    {
        Transform parent = gameObject.transform.parent;
        Vector3 loScale = worldScale;
        if (parent != null)
        {
            Vector3 parentScale = parent.lossyScale;
            loScale = worldScale.Divide(parentScale);
        }
        gameObject.transform.localScale = loScale;
    }
    public static void SetScale(this GameObject gameObject, float allScale)
    {

        Vector3 scale3 = new Vector3(allScale, allScale, allScale);
        SetScale(gameObject, scale3);
    }
    public static void SetScale(this GameObject gameObject, float x, float y)
    {

        Vector3 scale = new Vector3(x, y, gameObject.transform.localScale.z);
        SetScale(gameObject, scale);
    }
    public static void SetScale(this GameObject gameObject, float? x, float? y)
    {

        Vector3 l = gameObject.transform.localScale;
        Vector3 scale = new Vector3(x ?? l.x, y ?? l.y, l.z);
        SetScale(gameObject, scale);
    }
    public static void SetScale(this GameObject gameObject, Vector2 scale)
    {
        Vector3 scale3 = new Vector3(scale.x, scale.y, gameObject.transform.localScale.z);
        SetScale(gameObject, scale3);
    }
    public static void SetScaleLo(this GameObject gameObject, Vector3 localScale)
    {
        gameObject.transform.localScale = localScale;
    }
    public static void SetScaleLo(this GameObject gameObject, float? x, float? y, float? z)
    {
        Vector3 l = gameObject.transform.localScale;
        l = new Vector3(x ?? l.x, y ?? l.y, z ?? l.z);
        SetScaleLo(gameObject, l);
    }
    public static void SetScaleLo(this GameObject gameObject, float? x, float? y)
    {
        SetScaleLo(gameObject, x, y, null);
    }
    public static void SetScaleLo(this GameObject gameObject, Vector2 localScale)
    {
        Vector3 scale = new Vector3(localScale.x, localScale.y, gameObject.transform.localScale.z);
        SetScaleLo(gameObject, scale);
    }
    public static void SetScaleLo(this GameObject gameObject, float x, float y)
    {
        Vector3 scale = new Vector3(x, y, gameObject.transform.localScale.z);
        SetScaleLo(gameObject, scale);
    }
    #endregion
    // * Region +Scale End---------------------------------- 






    #endregion
    // * Region Tansform End---------------------------------- 






    #region Fast Visit
    public static Rigidbody2D GetRigidbody2D(this GameObject gameObject)
    {
        if (gameObject == null) return null;
        return gameObject.GetComponent<Rigidbody2D>();
    }
    #endregion
    // * Region Fast Visit End---------------------------------- 






    #region Other
    public static void SetHeight(this GameObject gameObject, float height)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        rectTransform.sizeDelta = new Vector2(width, height);
    }


    public static Vector2? GetSpriteSize(this GameObject gameObject)
    {
        Vector2? size = null;
        Bounds? bn = null;
        var renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (renderer != null) { bn = renderer.sprite.bounds; }
        size = bn?.size;
        return size;
    }
    public static Vector2? GetSize(this GameObject gameObject)
    {
        Vector2? size = null;
        Vector2? originSize = GetSpriteSize(gameObject);
        Vector2 sc = gameObject.GetScale2d();
        if (originSize != null) { size = originSize.Value.MultiplyEach(sc); }
        return size;
    }
    #endregion
    // * Region Other End---------------------------------- 



    //*Region  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    #region            Rigibody

    public static Vector2 VelocityToForce(this Rigidbody2D rb, Vector2 velocity)
    {
        return velocity / Time.fixedDeltaTime * rb.mass;
    }

    #endregion
    //*Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



}