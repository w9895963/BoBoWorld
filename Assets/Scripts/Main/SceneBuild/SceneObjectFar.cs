using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectFar : MonoBehaviour
{
    public Vector2 视差移动因子 = new Vector2(0.1f, 0.1f);
    [SerializeField] private Vector2 sourceP;

    void Start()
    {
        sourceP = gameObject.GetPosition2d();
    }

    void LateUpdate()
    {
        Vector2 moveFactor = 视差移动因子;
        Vector2 camP = Camera.main.gameObject.GetPosition2d();
        Vector2 difP = camP - sourceP;
        Vector2 moP = difP.MultiplyEach(moveFactor);
        gameObject.SetPosition(moP + sourceP);
        
    }
}
