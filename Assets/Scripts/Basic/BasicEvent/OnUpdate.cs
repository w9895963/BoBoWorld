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
        public static void Add(GameObject gameObject, Action action, int index = 0)
        {
            BasicEvent.Core.Method.Add<Component.OnUpdateComponent>(gameObject, action, index);
        }



        public static void Remove(GameObject gameObject, Action action, int index = 0)
        {
            BasicEvent.Core.Method.Remove<Component.OnUpdateComponent>(gameObject, action);
        }





    }





}
