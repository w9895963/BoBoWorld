using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{
    namespace Component
    {
        public class OnLateUpdateComponent : Component.BasicEventMono
        {

            private void LateUpdate()
            {
                RunAction();
            }


        }
    }



    public class OnLateUpdate
    {
        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.Add<Component.OnLateUpdateComponent>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action action)
        {
            Method.Remove<Component.OnLateUpdateComponent>(gameObject, action);
        }
    }





}
