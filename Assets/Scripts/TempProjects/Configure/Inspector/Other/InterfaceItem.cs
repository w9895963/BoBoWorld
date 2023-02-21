using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configure.Inspector.InterfaceItems;
using UnityEngine;

namespace Configure.Inspector
{

    ///<summary>配置参数表</summary>
    [Serializable]
    public class ParamsTable
    {

        private List<ParmData> datas = new List<ParmData>();

        // [SerializeField]
        // [SerializeReference]
        // private List<InterfaceItem> interfaceItems;
        public string name;




        public ParamsTable(params ParmData[] presetData)
        {
            datas.AddRange(presetData);
            Init();
        }




        public void Init()
        {
            //根据类型创建对应的 InterfaceItem<T>
            // if (datas.Count > 0)
            //     DebugF.GetClassFieldsLog(datas[0]).Log();


        }

        public void GetData(int index)
        {

        }





    }

    public abstract class ParmData
    {
        public string showName;
        public bool useStaticData = true;
        public string dataName;
        public string help;
        public abstract System.Object data { get; set; }
    }
    public class ParmData<T> : ParmData
    {
        public T dataT;

        public ParmData(string showName, bool useStaticData, string dataName = null, T data = default, string help = null)
        {
            this.showName = showName;
            this.useStaticData = useStaticData;
            this.dataName = dataName;
            this.dataT = data;
            this.help = help;
        }


        public override object data { get => dataT; set => dataT = (T)value; }
    }





}

namespace Configure.Inspector.InterfaceItems
{
    public class InterfaceItem
    {
        [HideInInspector]
        public string name;

    }

    [Serializable]
    public class InterfaceItem<T> : InterfaceItem
    {
        
        
        public T value;

    }

    [Serializable]
    public class InterfaceItemInt : InterfaceItem<int>
    {
    }
    [Serializable]
    public class InterfaceItemListInt : InterfaceItem<List<int>>
    {
    }

    [Serializable]
    public class InterfaceItemDataNameDropList : InterfaceItem
    {
        
        
        
        public string value;

        private Type type;

        public InterfaceItemDataNameDropList(Type type)
        {
            this.type = type;
        }


        private string[] DataNameDropList()
        {
            return EventData.PresetNameF.GetAllNamesOnType(type);
        }
    }
}