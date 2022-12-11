using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicF
{

    public static Action VelocityModify(Rigidbody2D rb, float speed, float maxForce, Vector2 direction)
    {
        if (rb == null) return null;

        return () =>
        {
            Vector2 vector2 = PhysicMathF.CalcForceByVel(rb.velocity, speed * direction.normalized, maxForce, direction, (float)rb.mass);
            rb.AddForce(vector2);
        };
    }
}
