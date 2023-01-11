using System.Collections.Generic;
using EventData;
using NaughtyAttributes;
using UnityEngine;



//命名空间：配置
namespace Configure
{


    [System.Serializable]
    public class DataNameDropdown<T> : DataNameDropdownHelper
    {
        [AllowNesting]
        [NaughtyAttributes.Label("")]
        [Dropdown("UpdateDropdownNames")]
        [StackableDecorator.Label(0)]
        [StackableDecorator.StackableField]
        public string dataName;

        public DataNameDropdown(System.Enum name = null)
        {
            this.dataName = name.ToString();
        }



        public List<string> UpdateDropdownNames => DataNameF.GetNamesOnType(typeof(T));


    }


    [System.Serializable]
    public class DataNameDropdownHelper
    {

    }


    [System.Serializable]
    public class DataNameDropdownVec : DataNameDropdownHelper
    {
        [NaughtyAttributes.Label("")]
        [Dropdown("UpdateDropdownNames")]
        [StackableDecorator.StackableField]
        public string dataName;




        public List<string> UpdateDropdownNames => DataNameF.GetNamesOnType(typeof(Vector2));


    }

    [System.Serializable]
    public class DataNameDropdownVector : DataNameDropdownHelper
    {
        [NaughtyAttributes.Label("")]
        [Dropdown("UpdateDropdownNames")]
        [StackableDecorator.StackableField]
        public string dataName;
        [StackableDecorator.Label(0)]
        [StackableDecorator.StackableField]
        public string dataName2;




        public List<string> UpdateDropdownNames => DataNameF.GetNamesOnType(typeof(Vector2));


    }











}

