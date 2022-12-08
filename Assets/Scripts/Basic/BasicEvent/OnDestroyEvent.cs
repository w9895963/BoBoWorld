using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{
    namespace Component
    {
        public class OnDestroyEvent : BasicEventMono
        {
            private void OnDestroy()
            {
                RunAction();
            }
        }
    }



    public class OnDestroyEvent
    {


        public static void Add(GameObject gameObject, Action action)
        {
            Method.Add<Component.OnDestroyEvent>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action action)
        {
            Method.Remove<Component.OnFixedUpdateComponent>(gameObject, action);
        }


    }





}
