using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventData.DataName;
using UnityEngine;

namespace Configure.Inspector
{
    public class CustomData
    {


    }
    public class CustomData<T> : CustomData, EventData.DataName.IDataName
    {
        private string dataName;
        public string DataName { get => dataName; set => dataName = value; }
        public Type DataType => typeof(T);

        public CustomData(string dataName = "")
        {
            DataName = dataName;
        }



        public void Rename(string newName)
        {
            EventData.DataNameF.Rename(dataName, newName);
        }


        private bool enabled = false;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (value == enabled)
                {
                    return;
                }
                enabled = value;
                if (enabled)
                {
                    EventData.DataNameF.CreateDataName(this);
                }
                else
                {
                    EventData.DataNameF.RemoveDataName(this);
                }
            }
        }



    }


    [Serializable]
    public class OutDataInspector<T> : CustomData<T>
    {
        public string currentName;
        public string type;

        public string rename = "";
        public bool doRename = false;
        public string renameCheck;



        public OutDataInspector(string dataName = "") : base(dataName)
        { }

        public void OnValidate()
        {
            UpdateInspectorShowStrings();

            if (doRename)
            {
                Rename(rename);
                doRename = false;
                UpdateInspectorShowStrings();
            }
        }

        private void UpdateInspectorShowStrings()
        {
            if (currentName != DataName)
            {
                currentName = DataName;
            }

            if (type != DataType.Name)
            {
                type = DataType.Name;
            }



            //~检查重命名是否合规
            //检查是否为全局属性
            string globalStr = "";
            if (rename.StartsWith("Global") | rename.StartsWith("全局"))
            {
                globalStr = "Global date; ";
            }


            //检查是否重复
            string repeatStr = "";
            if (EventData.DataNameF.GetDataNamesList().Contains(rename))
            {
                repeatStr = "Repeat name; ";
            }

            renameCheck = globalStr + repeatStr;
            if (renameCheck == "")
            {
                renameCheck = "OK";
            }
        }
    }







}