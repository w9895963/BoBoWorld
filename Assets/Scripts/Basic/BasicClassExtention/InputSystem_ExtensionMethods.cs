using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public static partial class ExtensionMethods
{



    public static float ReadValueAsFloat(this InputAction.CallbackContext date)
    {
        return date.ReadValue<float>();
    }
    public static Vector2 ReadValueAsVector2(this InputAction.CallbackContext date)
    {
        return date.ReadValue<Vector2>();
    }
    public static float ReadValueAsVector2_X(this InputAction.CallbackContext date)
    {
        return date.ReadValue<Vector2>().x;
    }
    public static float ReadValueAsVector2_Y(this InputAction.CallbackContext date)
    {
        return date.ReadValue<Vector2>().y;
    }
    public static bool ReadValueAsBool(this InputAction.CallbackContext date)
    {
        return date.ReadValue<float>() == 1;
    }
    public static Vector2? TryReadVector(this InputAction.CallbackContext date)
    {
        Vector2? re = null;
        object v = date.ReadValueAsObject();
        if (v.IsType<Vector2>())
            re = (Vector2)v;
        return re;
    }
    public static float? TryReadFloat(this InputAction.CallbackContext date)
    {
        float? re = null;
        object v = date.ReadValueAsObject();
        if (v.IsType<float>())
            re = (float)v;
        return re;
    }
    public static bool? TryReadButton(this InputAction.CallbackContext date)
    {
        bool? re = null;
        object v = date.ReadValueAsObject();
        if (v.IsType<float>())
            re = (float)v == 1;
        return re;
    }

    public static bool IsKeyOn(this InputAction.CallbackContext date)
    {
        return date.ReadValue<float>() == 1;
    }

}



