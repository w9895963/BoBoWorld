using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{
    namespace Component
    {
        public class OnCollision2D_Enter_Component : BasicEventMono<Collision2D>
        {
            private void OnCollisionEnter2D(Collision2D other)
            {
                RunAction(other);
            }
        }
    }

    public class OnCollision2D_Enter
    {

        public static void Add(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Core.Method.Add<Component.OnCollision2D_Enter_Component, Collision2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Core.Method.Remove<Component.OnCollision2D_Enter_Component, Collision2D>(gameObject, action);
        }

    }


}
