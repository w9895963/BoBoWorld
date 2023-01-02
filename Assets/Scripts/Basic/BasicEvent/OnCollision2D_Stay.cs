using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace BasicEvent
{
    namespace Component
    {
        public class OnCollision2D_Stay_Component : BasicEventMono<Collision2D>
        {
            private void OnCollisionStay2D(Collision2D other)
            {
                RunAction(other);
            }
        }
    }

    public class OnCollision2D_Stay
    {

        public static void Add(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.Add<Component.OnCollision2D_Stay_Component, Collision2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collision2D> action)
        {
            BasicEvent.Method.Remove<Component.OnCollision2D_Stay_Component, Collision2D>(gameObject, action);
        }

    }


}
