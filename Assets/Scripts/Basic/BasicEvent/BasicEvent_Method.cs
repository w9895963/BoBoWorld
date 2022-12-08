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
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            if (preventMultiple) c.action -= action;
            c.action += action;
        }
        public static void Add<C, T>(GameObject gameObject, Action<T> action, bool preventMultiple = false) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            Action<T> ac = c.action as Action<T>;
            if (preventMultiple) ac -= action;
            ac += action;
            c.action_ = ac;
        }
        public static void Remove<C>(GameObject gameObject, Action action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                return;
            }

            c.action -= action;
            if (c.action == null)
            {
                c.destroyed = true;
                c.Destroy();
            }
        }
        public static void Remove<C, T>(GameObject gameObject, Action<T> action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                return;
            }
            Action<T> ac = c.action as Action<T>;
            ac -= action;
            if (ac == null)
            {
                c.destroyed = true;
                c.Destroy();
            }
        }



    }
}

