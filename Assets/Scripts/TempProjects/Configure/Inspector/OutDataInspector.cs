using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventData.DataName;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;

namespace Configure.Inspector
{

    //*界面组成
    [Serializable]
    [InlineProperty]
    public partial class OutDataInspector
    {
        [ValueDropdown(nameof(currentName_NamesDropDownList))]
        [SuffixLabel("$" + nameof(currentName_typeName), true)]
        [HorizontalGroup("A")]
        [ShowIf(nameof(uiTab), 0)]
        [HideLabel]
        public string currentName;
        private string currentName_typeName => dataType.Name;
        private IEnumerable currentName_NamesDropDownList => GetDataNamesDropDownList();

        [HorizontalGroup("A")]
        [ShowIf(nameof(uiTab), 1)]
        [HideLabel]
        public string rename = "";

        [HorizontalGroup("A", MinWidth = 25)]
        [PropertyTooltip("重命名, 填写完名字后, 再次点击确认改名")]
        [Button("改")]
        private void RenameButton()
        {
            if (uiTab == 0)
            {
            }
            else
            {
                Rename(rename);
            }


            if (uiTab == 0)
            {
                uiTab++;
            }
            else
            {
                uiTab = 0;
            }
        }
        private void Rename(string newName)
        {
            //~重命名所有或自身
            if (currentName.IsEmpty())
            {
                UseName.DataName = newName;
            }
            else
            {
                EventData.DataNameD.AllNameInstance.Where((data) => data.DataName == currentName).ForEach((data) => data.DataName = newName);
            }

        }



        [Button("?")]
        [PropertyTooltip("$" + nameof(helpInfo))]
        [HorizontalGroup("A", MaxWidth = 20)]
        private void helpIcon() { }
        private string helpInfo
        {
            get
            {
                //~获得帮助信息
                var count = EventData.DataNameD.AllNameInstance.Where((data) => data.DataName == currentName).Count();
                var refCount = $"引用次数:{count}";
                var help = "如果不设置名字将不会被输出";


                string[] helpInfoList = { refCount, help };
                return string.Join(Environment.NewLine, helpInfoList);
            }
        }


        private int uiTab = 0;

        private IEnumerable GetDataNamesDropDownList()
        {
            ValueDropdownList<string> valueDropdownList = new ValueDropdownList<string>();
            EventData.DataNameD.AllDataNameInfo
            .Where((data) => data.DataType == dataType)
            .Where((data) => data.DataName.IsNotEmpty())
            .ForEach((data) =>
            {
                string countStr = data.InstanceCount == 0 ? "" : $"({data.InstanceCount})";
                valueDropdownList.Add($"{data.DataGroup}/{countStr}{data.DataName}", data.DataName);
            });

            valueDropdownList.Insert(0, new ValueDropdownItem<string>("未命名", ""));

            return valueDropdownList;
        }




    }




    //*公用方法
    public partial class OutDataInspector: IDataSetter
    {
        public void OnEnable()
        {
            if (enabled)
            {
                return;
            }
            UseName = EventData.DataName.DataNameInstance.AddName(() => currentName, (name) => currentName = name, dataType, null, this);
            enabled = true;
        }
        public void OnDisable()
        {
            if (!enabled)
            {
                return;
            }
            UseName = null;
            EventData.DataName.DataNameInstance.RemoveData(this);
            enabled = false;
        }


        public string DataName { get => currentName; set => currentName = value; }







        public Action<T> CreateDataSetter<T>(GameObject gameObject)
        {
            //~名字为空时, 返回空方法
            if (currentName == null)
            {
                return (data) => { };
            }
            return (data) => EventData.EventDataF.GetData<T>(currentName, gameObject).Data = data;
        }





        private bool enabled = false;
        private EventData.DataName.IDataNameInstance UseName;

    }





    //*构造
    public partial class OutDataInspector
    {

        public OutDataInspector(Type dataType, string dataName = null)
        {
            this.currentName = dataName;
            this.dataType = dataType;
            OnEnable();
        }



        private Type dataType;
    }











}
