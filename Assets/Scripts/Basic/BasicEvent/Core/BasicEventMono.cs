using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BasicEvent
{
    namespace Component
    {
        public class BasicEventMono : MonoBehaviour
        {
            private bool destroyed = false;
            private Action action = null;

            //方法：运行action
            public void RunAction()
            {
                action?.Invoke();
            }
            public void AddAction(Action action)
            {
                this.action += action;

            }
            //方法：移除action
            public void RemoveAction(Action action)
            {
                this.action -= action;
            }
            //方法：操作为空
            public bool IsActionEmpty()
            {
                return action == null;
            }
            //方法：删除
            public void Destroy()
            {
                Destroy(this);
                destroyed = true;
            }
            //方法：是否删除
            public bool IsDestroyed()
            {
                return destroyed;
            }



        }


        public class BasicEventMono<T> : BasicEventMono
        {
            private Action<T> action = null;
            //方法：运行action,覆盖RunAction
            public void RunAction(T date)
            {
                action?.Invoke(date);
            }


            //方法：添加action
            public void AddAction(Action<T> action)
            {
                this.action += action;
            }
            //方法：移除action
            public void RemoveAction(Action<T> action)
            {
                this.action -= action;
            }
            //方法：操作为空
            public new bool IsActionEmpty()
            {
                return action == null;
            }
        }
    }


}
