using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




namespace BasicEvent
{
    namespace Component
    {
        public class OnCollision2D_Exit_Component : BasicEventMono<Collision2D>
        {
            private void OnCollisionExit2D(Collision2D other)
            {
                RunAction(other);
            }
        }
    }

    public class OnCollision2D_Exit : MonoBehaviour
    {







        public static void Add(GameObject gameObject, Action<Collision2D> action)
        {
            Method.Add<Component.OnCollision2D_Exit_Component, Collision2D>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action<Collision2D> action)
        {
            Method.Remove<Component.OnCollision2D_Exit_Component, Collision2D>(gameObject, action);
        }

    }


}
