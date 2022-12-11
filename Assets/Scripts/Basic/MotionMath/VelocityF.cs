using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicMathF
{
    public static Vector2 CalcForceByVel(Vector2 currentVelocity, Vector2 targetVelocity, float maxForce, Vector2 projectVector = default, float mass = 1, float? deltaTime = null)
    {
        
        Vector2 force = default;
        Vector2 curV = currentVelocity;
        Vector2 tarV = targetVelocity;
        float delT = deltaTime == null ? Time.fixedDeltaTime : (float)deltaTime;

        if (projectVector != Vector2.zero)
        {
            curV = curV.Project(projectVector);
            tarV = tarV.Project(projectVector);
        }
        Vector2 difV = tarV - curV;
        force = (difV / delT * mass).ClampDistanceMax(maxForce);



        return force;
    }

}
