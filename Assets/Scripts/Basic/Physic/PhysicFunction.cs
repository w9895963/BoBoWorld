using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.Win32;
using UnityEngine;

namespace PhysicFunction
{



    public class Gravity
    {
        public Gravity(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }


        public GameObject GameObject { get => gameObject; }
        public bool Enabled { get => enabled; set => SetEnable(value); }
        public GravityConfig Config { get => config; set => SetConfig(value); }





        private GravityConfig config = new GravityConfig();
        private GameObject gameObject;
        private bool enabled = false;

        private void SetEnable(bool enabled)
        {
            if (this.enabled == enabled) return;


            GameObject obj = gameObject;
            Rigidbody2D rb = obj.GetRigidbody2D();
            if (enabled)
            {
                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdateAction);
            }
            else
            {
                BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdateAction);
            }


            void FixedUpdateAction()
            {
                Vector2 direction = config.direction;
                float force = config.force;
                float maxSpeed = config.maxSpeed;
                Vector2 v = gameObject.GetRigidbody2D().velocity.Project(direction);
                Vector2 vD = maxSpeed * direction.normalized - v;

                Vector2 f = rb.VelocityToForce(vD).ClampDistanceMax(maxSpeed);
                rb.AddForce(f);


            }
        }
        private void Reset()
        {
            SetEnable(false);
            SetEnable(true);
        }
        private void SetConfig(GravityConfig value)
        {
            config = value;
            Reset();
        }



    }
    [System.Serializable]
    public class GravityConfig
    {
        public Vector2 direction = Vector2.down;
        public float force = 60;
        public float maxSpeed = 10;
    }

}
