using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace ActionUtility
{
    public class SafeRunAction : MonoBehaviour
    {
        private static List<Action> actions = new List<Action>();




        //更新
        private void Update()
        {
            if (actions.Count > 0)
            {
                actions.ForEach(a => a?.Invoke());
                actions.Clear();
            }




            GameObject.Destroy(gameObject);
        }



        public static void RunAction(Action action)
        {
            if (action == null) return;
            actions.Add(action);
            GameObject p = Resources.Load<GameObject>("Prefab/SafeRunAction");
            GameObject obj = Instantiate(p);
        }


    }
}
