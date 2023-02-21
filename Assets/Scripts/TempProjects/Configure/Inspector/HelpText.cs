using System;
using Sirenix.OdinInspector;
using UnityEngine;



namespace Configure.Inspector
{

    [Serializable]
    [BoxGroup("ShowOnlyText",false)]
    // [HideLabel]
    public class HelpText
    {

        [InfoBox("$" + nameof(脚本说明))]
        [HideLabel]
        [SerializeField]
        private PlaceHolder placeHolder;
        [HideInInspector]
        public string 脚本说明;

        public HelpText(params string[] texts)
        {

            this.脚本说明 = string.Join("\n", texts);
        }
        [Serializable]
        private class PlaceHolder
        {

        }


    }




}

