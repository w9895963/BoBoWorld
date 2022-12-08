using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFunction : MonoBehaviour
{
    public static bool ContactTest(Collider2D collider, ContactPoint2D[] contacts, Vector2 normalTest = default, float AngleTest = 0, string tagCheck = null)
    {
        bool result = true;



        if (normalTest != Vector2.zero)
        {
            foreach (var ct in contacts)
            {
                if (ct.normal.Angle(normalTest) > AngleTest)
                {
                    result = false;
                    break;
                };
            }
        }
        if (tagCheck != null)
        {
            if (collider.gameObject.tag != tagCheck)
            {
                result = false;
            }
        }

        return result;
    }
}
