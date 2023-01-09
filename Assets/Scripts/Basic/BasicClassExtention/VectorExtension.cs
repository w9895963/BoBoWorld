using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{



    public static Vector2 AddEach(this Vector2 vector, Vector2 add)
    {
        return new Vector2(vector.x + add.x, vector.y + add.y);
    }
    public static float Angle(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to);
    }

    public static Vector2 ProjectOnPlane(this Vector2 vector, Vector2 normal)
    {
        return (Vector2)Vector3.ProjectOnPlane(vector, normal);
    }
    public static Vector2 Project(this Vector2 vector, Vector2 direction)
    {
        return (Vector2)Vector3.Project(vector, direction);
    }
    public static float ProjectToFloat(this Vector2 vector, Vector2 direction)
    {
        Vector2 vct = (Vector2)Vector3.Project(vector, direction);
        float result = vct.magnitude;
        if ((vct.normalized + direction.normalized).magnitude < 1)
            result *= -1;
        return result;
    }



    public static Vector2 ScreenToWold(this Vector2 vector)
    {
        return Camera.main.ScreenToWorldPoint(vector);
    }
    public static Vector2 WoldToScreen(this Vector2 vector)
    {
        return Camera.main.WorldToScreenPoint(vector);
    }



    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        
        return Quaternion.AngleAxis(angle, Vector3.back) * vector;
    }


    public static Vector2 RotateTo(this Vector2 vector, Vector2 to)
    {
        return to.normalized * vector.magnitude;
    }
    public static Vector2 RotateTo(this Vector2 vector, Vector2 to, float angle)
    {
        float v = Vector2.SignedAngle(vector, to);
        angle = angle.Abs() * v.Sign();
        return vector.Rotate(angle);
    }

    public static float SignedAngle(this Vector2 vector, Vector2 to)
    {
        return Vector2.SignedAngle(vector, to);
    }



    public static Vector2 MultiplyEach(this Vector2 v, Vector2 n)
    {
        return new Vector2(v.x * n.x, v.y * n.y);
    }
    public static Vector3 Scale(this Vector3 v, float scale)
    {
        return new Vector3(v.x * scale, v.y * scale, v.z * scale);
    }
    public static Vector3 Divide(this Vector3 v, Vector3 s, float preventZero = 0.00001f)
    {
        return v.Each((x, i) => x / (s[i] == 0 ? preventZero : s[i]));
    }
    public static Vector2 Divide(this Vector2 v, Vector2 s, float preventZero = 0.00001f)
    {
        return v.ToVector3().Divide(s, preventZero);
    }
    public static Vector3 Each(this Vector3 v, Func<float, int, float> func)
    {
        return new Vector3(func(v.x, 0), func(v.y, 1), func(v.z, 2));
    }

    public static Vector2 SetOneAxis(this Vector2 v, Vector2 direction, float value)
    {
        Vector2 oneV = v.Project(direction);
        Vector2 otherV = v - oneV;
        return otherV + oneV.normalized * value;

    }









    public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(v.x.Clamp(min.x, max.x), v.y.Clamp(min.y, max.y));
    }
    public static Vector2 ClampDirection(this Vector2 v, Vector2 direction, float? min = null, float? max = null)
    {
        Vector2 v1 = v.Project(direction);
        float v1F = v1.ProjectToFloat(direction);
        if (min != null)
        {
            if (min > v1F)
            {
                v1F = (float)min;
            }
        }
        if (max != null)
        {
            if (max < v1F)
            {
                v1F = (float)max;
            }
        }
        v1 = v1.normalized * v1F;
        Vector2 v2 = v - v1;

        return v1 + v2;
    }

    public static Vector2 ClampDistanceMax(this Vector2 vector, float max)
    {
        if (vector.magnitude >= max)
        {
            return vector.normalized * max;
        }
        else
        {
            return vector;
        }
    }

    public static float GetAngleSigned(this Vector2 v)
    {
        return Vector2.SignedAngle(Vector2.right, v);
    }

    public static bool IsSameSide(this Vector2 vector, Vector2 to)
    {
        return Vector2.Angle(vector, to) < 90;
    }
    public static bool IsNotZero(this Vector2 vector)
    {
        return vector.magnitude != 0;
    }




    public static Vector2 SetVector(this Vector2 v, float? magnitude = null, float? angle = null)
    {
        Vector2 re = v;

        if (re.magnitude == 0)
        {
            if (magnitude != null & angle != null)
            {
                re = Vector2.right;
                re = re.normalized * magnitude.Value;
                re = re.Rotate(angle.Value);
            }
        }
        else
        {
            if (magnitude != null)
            {
                re = re.normalized * magnitude.Value;
            }
            if (angle != null)
            {
                re = re.Rotate(angle.Value);
            }
        }

        return re;
    }


    public static Vector3 ToVector3(this Vector2 vector, float z = 0)
    {
        return new Vector3(vector.x, vector.y, z);
    }
    public static string[] ToStringArray(this Vector2 vector)
    {
        return new string[2] { vector.x.ToString(), vector.y.ToString() };
    }









}
