using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BasicEvent
{
    namespace Component
    {
        public class OnUpdateComponent : Component.BasicEventMono
        {
            private void Update()
            {
                RunAction();
            }


        }
    }



    public class OnUpdate
    {
        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.Add<Component.OnFixedUpdateComponent>(gameObject, action);
        }


        public static void Remove(GameObject gameObject, Action action)
        {
            Method.Remove<Component.OnFixedUpdateComponent>(gameObject, action);
        }


    }





}
