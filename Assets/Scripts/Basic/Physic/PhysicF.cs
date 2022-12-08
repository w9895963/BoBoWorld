using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicF
{
    public static Action VelocityModify(GameObject gameObject, float speed, float maxForce, Vector2 direction, bool state = true)
    {
        Rigidbody2D rb = gameObject.GetRigidbody2D();
        if (rb == null) return null;

        return () =>
        {
            Vector2 vector2 = PhysicMathF.VelocityChange_getForce(rb.velocity, speed * direction.normalized, maxForce, direction, (float)rb.mass);
            rb.AddForce(vector2);
        };
    }
}
