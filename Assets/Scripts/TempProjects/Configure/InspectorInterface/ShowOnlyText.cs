using System;
using UnityEngine;



namespace Configure.InspectorInterface
{

    [Serializable]
    public class ShowOnlyText
    {
        [NaughtyAttributes.ResizableTextArea]
        [NaughtyAttributes.ReadOnly]
        public string 脚本说明;


        public ShowOnlyText(params string[] texts)
        {
            this.脚本说明 = string.Join("\n", texts);
        }


    }




}

