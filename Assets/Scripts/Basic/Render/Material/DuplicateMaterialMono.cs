using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateMaterialMono : MonoBehaviour
{
    void Start()
    {
        Renderer ren = GetComponent<Renderer>();
        Material sharedMaterial = ren.sharedMaterial;
        Material newMaterial = GameObject.Instantiate(sharedMaterial);
        ren.sharedMaterial = newMaterial;

    }

}
