using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Bolt;

namespace BoltExt
{

    public class Input_EventToBoltTransfer_CMP : MonoBehaviour
    {
        public InputActionAsset asset;


        private void Reset()
        {
            asset = GetComponent<PlayerInput>().actions;
        }

        private void OnEnable()
        {

            asset.actionMaps.ForEach((map) =>
            {
                map.actions.ForEach((act) =>
                {
                    act.performed += Action;
                });
            });
        }
        private void OnDisable()
        {
            asset.actionMaps.ForEach((map) =>
             {
                 map.actions.ForEach((act) =>
                 {
                     act.performed -= Action;
                 });
             });

        }
        private void Action(InputAction.CallbackContext dat)
        {
            CustomEvent.Trigger(gameObject, dat.action.name, dat);
        }



    }
}
