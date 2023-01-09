using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIF
{
    public static float GetChildrenHeight(GameObject parent)
    {
        List<GameObject> children = parent.GetDirectChildren();
        float height = 0;
        children.ForEach((x) =>
        {
            if (x.activeSelf)
                height += x.GetComponent<RectTransform>().rect.height;
        });
        return height;
    }
}
