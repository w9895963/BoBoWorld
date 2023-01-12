using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{
    namespace Component
    {
        public class OnTrigger2D_Enter_Component : BasicEventMono<Collider2D>
        {
            private void OnTriggerEnter2D(Collider2D other)
            {
                RunAction(other);
            }
        }
    }

    public class OnTrigger2D_Enter : MonoBehaviour
    {








        public static void Add(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Core.Method.Add<Component.OnTrigger2D_Enter_Component, Collider2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collider2D> action)
        {
            BasicEvent.Core.Method.Remove<Component.OnTrigger2D_Enter_Component, Collider2D>(gameObject, action);
        }

    }


}
