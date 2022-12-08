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




        }
    }

}

