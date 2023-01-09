using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bolt;

public class Bolt_CallCustomEvent : MonoBehaviour
{
    public static void CallCustomEvent(UnityEvent evt, GameObject gameObject, string customEventName)
    {
        evt.AddListener(() =>
        {
            CustomEvent.Trigger(gameObject, customEventName);
        });
        
    }
}
