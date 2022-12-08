using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Bolt_BasicEventTransfer_OnCollisionExit2D : MonoBehaviour
{
    public string customEventName;

    private void Reset()
    {
        customEventName = "OnCollisionExit2D";
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        CustomEvent.Trigger(gameObject, customEventName,other);
    }

}
