using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectWhenOnGame : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor)
        {
            gameObject.Destroy();
        }
    }
}
