using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Bolt_BasicEventTransfer_OnCollisionEnter2D : MonoBehaviour
{
    public string customEventName;

    private void Reset()
    {
        customEventName = "OnCollisionEnter2D";
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        CustomEvent.Trigger(gameObject, customEventName, other);
    }
}
