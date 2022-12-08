using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.Linq;

namespace BoltExt
{
    public class ComponentCleaner : MonoBehaviour
    {

        private static Dictionary<StateMachine, List<Component>> dict =
         new Dictionary<StateMachine, List<Component>>();



        public static void AddComponentToList(StateMachine stateMachine, Component component)
        {
            List<Component> components = dict.GetOrCreate(stateMachine);
            components.AddNotHas(component);
        }
        public static void AddComponentToList(GameObject gameObject, string stateMachineName, Component component)
        {
            StateMachine stateMachine = gameObject.GetComponents<StateMachine>().First((st) => st.name == stateMachineName);
            AddComponentToList(stateMachine, component);
        }

        public static void RemoveComponentsBut(StateMachine stateMachine, params Component[] components)
        {
            List<Component> list = dict.GetOrCreate(stateMachine);
            list.Except(components).ForEach((i) =>
            {
                list.Remove(i);
                i.Destroy();
            });

        }
    }
}

