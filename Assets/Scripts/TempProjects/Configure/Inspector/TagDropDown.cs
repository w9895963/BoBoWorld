using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;



namespace Configure.Inspector
{
    [Serializable]
    [InlineProperty]
    public class TagDropDown
    {
        [ValueDropdown(nameof(ValueDropdown))]
        [HideLabel]
        public string tag;

        public TagDropDown()
        {
        }

        public TagDropDown(string tag)
        {
            this.tag = tag;
        }

        public TagDropDown(System.Enum tag)
        {
            this.tag = tag.ToString();
        }

        private string[] ValueDropdown => UnityEditorInternal.InternalEditorUtility.tags;
    }
}