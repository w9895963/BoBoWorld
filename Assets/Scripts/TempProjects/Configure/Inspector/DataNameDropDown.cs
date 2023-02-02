using System.Linq;
using EventData;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Configure.Inspector
{
    [System.Serializable]
    [InlineProperty]
    public class DataNameDropDown<T>
    {
        
        
        [HideLabel]
        [ValueDropdown(nameof(UpdateDropdownNames))]
        public string dataName;

        public string[] UpdateDropdownNames()
        {
            return EventData.DataNameF.GetAllNamesOnType(typeof(T)).ToArray();
        }






        public DataNameDropDown(System.Enum dataNamePreset)
        {
            Construct(dataNamePreset.ToString());
        }


        public DataNameDropDown(string dataNamePreset)
        {

            Construct(dataNamePreset);
        }
        public void Construct(string dataNamePreset)
        {
            dataName = dataNamePreset;
        }



        public EventDataHandler<T> GetEventDataHandler(GameObject gameObject)
        {
            return EventDataF.GetData<T>(dataName, gameObject);
        }

    }









}
