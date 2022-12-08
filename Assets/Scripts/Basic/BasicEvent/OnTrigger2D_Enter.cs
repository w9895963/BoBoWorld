using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{
    namespace Component
    {
        public class OnTrigger2D_Enter_Component : BasicEventMono
        {
            private void OnTriggerEnter2D(Collider2D other)
            {
                RunAction<Collider2D>(other);
            }
        }
    }

    public class OnTrigger2D_Enter : MonoBehaviour
    {








        public static void Add(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Method.Add<Component.OnTrigger2D_Enter_Component, Collider2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Method.Remove<Component.OnTrigger2D_Enter_Component, Collider2D>(gameObject, action);
        }

    }


}
