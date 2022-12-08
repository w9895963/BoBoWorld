using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{
    namespace Component
    {
        public class OnParticleTriggerEventComponent : BasicEventMono
        {

            private void OnParticleTrigger()
            {
                RunAction();
            }
        }
    }



    public class OnParticleTriggerEvent
    {





        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.Add<Component.OnParticleTriggerEventComponent>(gameObject, action);
        }
        public static void Remove(GameObject gameObject, Action action)
        {
            Method.Remove<Component.OnParticleTriggerEventComponent>(gameObject, action);
        }


    }





}
