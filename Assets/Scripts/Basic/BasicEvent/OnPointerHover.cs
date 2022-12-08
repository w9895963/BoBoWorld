using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BasicEvent
{


    public class OnPointerHover
    {






        public static void Add(GameObject gameObject, Action<BaseEventData> onPointerEnter, Action<BaseEventData> onPointerExit)
        {
            Collider2D com = gameObject.GetComponent<Collider2D>();
            if (com == null) { gameObject.AddComponent<PolygonCollider2D>(); }
            EventTrigger et = gameObject.GetComponent<EventTrigger>(true);



            EventTrigger.Entry enterTrigger = new EventTrigger.Entry();
            enterTrigger.eventID = EventTriggerType.PointerEnter;
            enterTrigger.callback.AddListener((d) =>
            {
                onPointerEnter?.Invoke(d);

            });
            et.triggers.Add(enterTrigger);



            EventTrigger.Entry exitTrigger = new EventTrigger.Entry();
            exitTrigger.eventID = EventTriggerType.PointerExit;
            exitTrigger.callback.AddListener((d) =>
            {
                onPointerExit?.Invoke(d);

            });
            et.triggers.Add(exitTrigger);


        }

        public static void Remove(GameObject gameObject)
        {

        }

    }


}
