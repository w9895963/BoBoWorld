using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{
    namespace Component
    {
        public class OnCollision2D_Enter_Component : BasicEventMono
        {
            private void OnCollisionEnter2D(Collision2D other)
            {
                RunAction<Collision2D>(other);
            }
        }
    }

    public class OnCollision2D_Enter
    {

        public static void Add(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.Add<Component.OnCollision2D_Enter_Component, Collision2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.Remove<Component.OnCollision2D_Enter_Component, Collision2D>(gameObject, action);
        }

    }


}
