using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Timer
{
    namespace Base
    {
        public class WaitUpdate : MonoBehaviour
        {
            private Action action;

            public void Setup(Action action)
            {
                this.action += action;
            }

            private void Update()
            {
                action?.Invoke();
                this.Destroy();
            }

            //销毁自身
            private void Destroy()
            {
                MonoBehaviour[] monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
                //如果只有一个组件，且是自身
                if (monoBehaviours.Length == 1 && monoBehaviours[0] == this)
                {
                    Destroy(gameObject);
                }

            }




        }
    }

}

