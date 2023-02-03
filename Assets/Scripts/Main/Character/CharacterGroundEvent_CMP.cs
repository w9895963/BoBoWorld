using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterGroundEvent_CMP : MonoBehaviour
{
    public Vector2 UpDirection = new Vector2(0, 1);
    public float allowAngle = 10;
    public string groundTabName = "GroundBlock";


    public GameObject groundObject;
    public bool isOnGround;


    public Events events = new Events();
    public class Events
    {
        public UnityEvent GroundStepOn = new UnityEvent();
        public UnityEvent GroundStepOut = new UnityEvent();
    }



    private List<(GameObject, List<ContactPoint2D>)> groundObjects = new List<(GameObject, List<ContactPoint2D>)>();
    private OtState onGround;

    private void Awake()
    {
        onGround = new OtState(gameObject, Conf.CharacterActionName.在地上);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        bool onAir = groundObjects.Count == 0;
        other.contacts.ForEach((ct) =>
        {
            if (ct.normal.Angle(UpDirection) <= allowAngle)
            {
                (GameObject obj, List<ContactPoint2D> ps) newM = (ct.collider.gameObject, new List<ContactPoint2D>());
                var ms = groundObjects.FindOrAdd(newM, (o) => o.Item1 == ct.collider.gameObject);
                ms.Item2.Add(ct);
            }
        });
        bool landing = onAir & groundObjects.Count > 0;
        if (landing)
        {
            GroundStepOn();
        }


        groundObject = GetGround();

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        bool onGround = groundObjects.Count > 0;
        GameObject groundObj = other.gameObject;
        groundObjects.RemoveAll((i) => i.Item1 == groundObj);

        bool leave = onGround & groundObjects.Count == 0;
        if (leave)
        {
            GroundStepOut();
        }

        groundObject = GetGround();
    }




    private void GroundStepOut()
    {
        isOnGround = false;
        onGround.Enabled = false;
        events.GroundStepOut.Invoke();
    }

    private void GroundStepOn()
    {
        isOnGround = true;
        onGround.Enabled = true;
        events.GroundStepOn.Invoke();
    }







    public GameObject GetGround()
    {
        GameObject re = null;
        var g = groundObjects.LastOrDefault();
        if (g != default)
        {
            re = g.Item1;
        }

        return re;
    }

    public Vector2 GetGroundVelocity()
    {
        Vector2 re = Vector2.zero;
        if (isOnGround)
        {
            Rigidbody2D r = gameObject.GetRigidbody2D();
            if (r != null)
            {
                re = r.velocity;
            }
        }
        return re;
    }

}
