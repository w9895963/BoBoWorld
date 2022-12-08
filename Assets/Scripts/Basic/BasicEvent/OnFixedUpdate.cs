using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BasicEvent
{
    namespace Component
    {
        public class OnFixedUpdateComponent : Component.BasicEventMono
        {
            private void FixedUpdate()
            {
                RunAction();
            }


        }
    }



    public class OnFixedUpdate
    {
        public static void Add(GameObject gameObject, Action action)
        {
            BasicEvent.Method.Add<Component.OnFixedUpdateComponent>(gameObject, action);
        }
        public static void Add(GameObject gameObject, Action action, ref Action AddRemoveAction)
        {
            Add(gameObject, action);
            AddRemoveAction += () => Remove(gameObject, action);
        }

        public static void Remove(GameObject gameObject, Action action)
        {
            Method.Remove<Component.OnFixedUpdateComponent>(gameObject, action);
        }
        
        public static void Turn(GameObject gameObject, Action action, bool state)
        {
            if (state)
            {
                Add(gameObject, action);
            }
            else
            {
                Remove(gameObject, action);
            }
        }
    }





}
