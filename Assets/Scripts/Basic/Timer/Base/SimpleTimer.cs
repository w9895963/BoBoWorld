using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Timer
{
    namespace Base
    {
        public class SimpleTimer : MonoBehaviour
        {
            private float timeStart;
            public float CurrentTime => Time.time - timeStart;

            public void Wait(float time, Action action)
            {
                IEnumerator routine = TimerAction(time, action);
                StartCoroutine(routine);
                timeStart = Time.time;
            }

            IEnumerator TimerAction(float wait, Action action)
            {
                yield return new WaitForSeconds(wait);
                action?.Invoke();
                this.Destroy();
            }




        }
    }

}

