using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Bolt;

namespace BoltExt
{
    public class BoltF : MonoBehaviour
    {

        public static Component GetOrAddComponent(GameObject gameObject, Type componentType)
        {
            Component component = gameObject.GetComponent(componentType);
            if (component == null)
            {
                component = gameObject.AddComponent(componentType);
            }
            return component;
        }


        public static bool IfContaint(string[] items, string item)
        {
            foreach (string t in items)
            {
                if (t == item)
                    return true;
            }
            return false;
        }


        public static void GetValue(VariableDeclaration[] vars, string name)
        {
            vars.First((i) => i.name == name);
        }




    }
}

