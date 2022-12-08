using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicMathF
{
    public static Vector2 VelocityChange_getForce(Vector2 currentVelocity, Vector2 targetVelocity,
     float maxForce, Vector2 demensionVector = default, float mass = 1, float? deltaTime = null)
    {
        Vector2 force = default;
        Vector2 curV = currentVelocity;
        Vector2 tarV = targetVelocity;
        float delT = deltaTime == null ? Time.fixedDeltaTime : (float)deltaTime;

        if (demensionVector != Vector2.zero)
        {
            curV = curV.Project(demensionVector);
            tarV = tarV.Project(demensionVector);
        }
        Vector2 difV = tarV - curV;
        force = (difV / delT * mass).ClampDistanceMax(maxForce);



        return force;
    }

}
