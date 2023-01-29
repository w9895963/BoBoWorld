using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasicEvent
{





    namespace Core
    {
        public static class Method
        {



            public static void Add<C>(GameObject gameObject, Action action, int index = 0) where C : Component.BasicEventMono
            {
                C c = gameObject.GetComponent<C>();
                if (c == null)
                {
                    c = gameObject.AddComponent<C>();
                }
                c.AddActionAndSort(action, index);
            }
            public static void Add<C, T>(GameObject gameObject, Action<T> action, int index = 0) where C : Component.BasicEventMono<T>
            {
                C c = gameObject.GetComponent<C>();
                if (c == null)
                {
                    c = gameObject.AddComponent<C>();
                }
                c.AddActionAndSort(action, index);
            }





            public static void Remove<C>(GameObject gameObject, Action action) where C : Component.BasicEventMono
            {
                C c = gameObject.GetComponent<C>();
                if (c == null)
                {
                    return;
                }

                c.RemoveAction(action);



            }
            public static void Remove<C, T>(GameObject gameObject, Action<T> action) where C : Component.BasicEventMono<T>
            {
                C c = gameObject.GetComponent<C>();
                if (c == null)
                {
                    return;
                }
                c.RemoveAction(action);


            }


            public static bool Exist<C>(GameObject gameObject, Delegate action) where C : Component.BasicEventMono
            {
                C c = gameObject.GetComponent<C>();

                return c.HasAction(action); ;
            }
        }
    }
}

