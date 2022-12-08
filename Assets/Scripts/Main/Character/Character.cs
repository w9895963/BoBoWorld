using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static CharacterUtility.StaticFunction;

public class Character
{

    public abstract class EventDef
    {
        public abstract void AddListener(EventName e, Action act);
        public void AddListener(EventName e, Action act, ref Action disableAction)
        {
            AddListener(e, act);
            disableAction += () => RemoveListener(e, act);
        }
        public abstract void RemoveListener(EventName e, Action act);
        public abstract void Invoke(EventName e);
    }
    public enum EventName
    {
        JumpEnd,
        GroundStepOn,
        GroundStepOut

    }

    public Character(GameObject gameObject) => Construct(this, gameObject);




    public EventDef Event;
    public GameObject gameObject;



    public Vector2? RelateVelocity { get => GetRelateVelocity(this); set => SetRelateVelocity(this, value); }




}








namespace CharacterUtility
{
    public class Event : Character.EventDef
    {
        private Dictionary<Character.EventName, Action> actionDIc = new Dictionary<Character.EventName, Action>();
        public override void AddListener(Character.EventName e, Action act)
        {
            bool hasKey = actionDIc.ContainsKey(e);
            if (!hasKey) actionDIc[e] = null;
            actionDIc[e] += act;
        }
        public override void RemoveListener(Character.EventName e, Action act)
        {
            bool hasKey = actionDIc.ContainsKey(e);
            if (hasKey) actionDIc[e] -= act;
        }

        public override void Invoke(Character.EventName e)
        {
            actionDIc.TryGetValue(e)?.Invoke();
        }


    }


    public static class StaticFunction
    {
        public static Vector2? GetRelateVelocity(Character chr)
        {
            Vector2? vel = null;

            GameObject gameObject = chr.gameObject;
            Vector2? v1 = gameObject.GetRigidbody2D()?.velocity;
            Vector2? v2 = gameObject.GetComponent<CharacterGroundEvent_CMP>()?.GetGround().GetRigidbody2D()?.velocity;
            if (v1 != null & v2 != null)
            {
                vel = v1.Value - v2.Value;
            }
            return vel;
        }
        public static void SetRelateVelocity(Character chr, Vector2? velocity)
        {
            if (velocity == null)
                return;

            GameObject gameObject = chr.gameObject;
            Rigidbody2D rb = gameObject.GetRigidbody2D();
            Vector2? v1 = rb?.velocity;
            Vector2? v2 = gameObject.GetComponent<CharacterGroundEvent_CMP>()?.GetGround().GetRigidbody2D()?.velocity;
            if (v1 != null & v2 != null)
            {

                Vector2 vTar = v2.Value + velocity.Value;

                Vector2 vDif = vTar - v1.Value;

                Vector2 imp = vDif * rb.mass;

                rb.AddForce(imp, ForceMode2D.Impulse);

            }
        }


        public static void Construct(Character chr, GameObject gameObject)
        {
            chr.gameObject = gameObject;
            chr.Event = new Event();

            ICharacterEvent[] comps = gameObject.GetComponents<ICharacterEvent>();
            comps.ForEach((c) =>
            {
                c.characterEvents.ForEach((e) =>
                {
                    e.eventObject.AddListener(() =>
                    {
                        chr.Event.Invoke(e.eventName);
                    });

                });

            });
        }
    }
}

public interface ICharacterEvent
{
    CharacterEvent[] characterEvents { get; }
    public class CharacterEvent
    {
        public Character.EventName eventName;
        public UnityEvent eventObject;
    }
}

