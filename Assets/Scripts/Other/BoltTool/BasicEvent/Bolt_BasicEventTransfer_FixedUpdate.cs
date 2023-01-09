using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Bolt_BasicEventTransfer_FixedUpdate : MonoBehaviour
{
    public string customEventName;

    private void Reset()
    {
        customEventName = "FixedUpdate";
    }
    private void FixedUpdate()
    {
        CustomEvent.Trigger(gameObject, customEventName);
    }
}
