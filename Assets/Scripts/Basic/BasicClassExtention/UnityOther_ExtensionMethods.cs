using System;
using System.Collections.Generic;
using System.Linq;
using Global;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static partial class ExtensionMethods
{






    #region Texture
    public static Sprite ToSprite(this Texture tex, float pixelsPerUnit = 20)
    {
        Texture2D tex2d = (Texture2D)tex;
        Rect rect = new Rect(0, 0, tex2d.width, tex2d.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex2d, rect, pivot, pixelsPerUnit);
        return sprite;
    }
    #endregion
    // * Region Texture End---------------------------------- 





    #region UnityEvent
    public static void AddListener(this UnityEvent evt, UnityAction call, ref Action AddRemoveAction)
    {
        evt.AddListener(call);
        AddRemoveAction += () => evt.RemoveListener(call);
    }

    #endregion
    // * Region UnityEvent End---------------------------------- 





}



