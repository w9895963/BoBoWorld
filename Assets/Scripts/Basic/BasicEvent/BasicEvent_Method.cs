using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasicEvent
{
    public static class Method
    {



        public static void Add<C>(GameObject gameObject, Action action, bool preventMultiple = false) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.IsDestroyed() == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            if (preventMultiple) c.RemoveAction(action);
            c.AddAction(action);
        }
        public static void Add<C, T>(GameObject gameObject, Action<T> action, bool preventMultiple = false) where C : Component.BasicEventMono<T>
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.IsDestroyed() == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            if (preventMultiple) c.RemoveAction(action);
            c.AddAction(action);
        }


        
        public static void Remove<C>(GameObject gameObject, Action action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.IsDestroyed() == false);
            if (c == null)
            {
                return;
            }

            c.RemoveAction(action);
            if (c.IsActionEmpty())
            {
                c.Destroy();
            }
        }
        public static void Remove<C, T>(GameObject gameObject, Action<T> action) where C : Component.BasicEventMono<T>
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.IsDestroyed() == false);
            if (c == null)
            {
                return;
            }
            c.RemoveAction(action);
            if (c.IsActionEmpty())
            {
                c.Destroy();
            }
        }



    }
}

