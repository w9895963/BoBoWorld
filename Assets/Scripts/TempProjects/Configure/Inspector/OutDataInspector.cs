using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configure.Inspector
{
    [Serializable]
    [InlineProperty]
    public abstract partial class OutDataInspector : IDataSetter
    {
        #region //&界面



        [ValueDropdown(nameof(currentName_NamesDropDownList))]
        [SuffixLabel("$" + nameof(typeName), true)]
        [HorizontalGroup("A")]
        [ShowIf(nameof(uiTab), 0)]
        [HideLabel]
        public string currentName;
        private string currentName_beforeEdit;


        [HideInInspector]
        public string typeName;

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

        [HideInInspector]
        public bool doRename = false;

        [HideInInspector]
        public string helpInfo;

        [Button("?")]
        [PropertyTooltip("$" + nameof(helpInfo))]
        [HorizontalGroup("A", MaxWidth = 20)]
        private void helpIcon() { }



        private int uiTab = 0;
        private IEnumerable currentName_NamesDropDownList => GetDataNamesDropDownList();
        private IEnumerable GetDataNamesDropDownList()
        {
            ValueDropdownList<string> valueDropdownList = new ValueDropdownList<string>();
            EventData.DataNameD.AllDataNameInfo
            .Where((data) => data.DataType == DataType)
            .Where((data) => data.DataName.IsNotEmpty())
            .ForEach((data) =>
            {
                string countStr = data.InstanceCount == 0 ? "" : $"({data.InstanceCount})";
                valueDropdownList.Add($"{data.DataGroup}/{countStr}{data.DataName}", data.DataName);
            });

            valueDropdownList.Insert(0, new ValueDropdownItem<string>("未命名", ""));

            return valueDropdownList;
        }


        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        public Action<T> CreateDataSetter<T>(GameObject gameObject)
        {
            //~名字为空时, 返回空方法
            OutDataInspector<T> thisT = (OutDataInspector<T>)this;
            if (thisT.currentName == null)
            {
                return (data) => { };
            }
            return (data) => EventData.EventDataF.GetData<T>(thisT.currentName, gameObject).Data = data;
        }



        public void OnValidate()
        {
            //~初始化
            EventData.DataName.IDataNameInstance useName = UseName;


            //~固定名称
            typeName = DataType.Name;


            //~重命名
            if (doRename)
            {
                doRename = false;
                Rename(rename);
            }

            //~数据名改变
            if (currentName != currentName_beforeEdit)
            {
                currentName_beforeEdit = currentName;
                UseName.DataName = currentName;
            }


            //~获得帮助信息
            BuildHelpInfo();






        }





        protected Func<bool> aliveChecker;
        private EventData.DataName.IDataNameInstance useName_data;
        private EventData.DataName.IDataNameInstance UseName
        {
            get
            {
                EventData.DataName.IDataNameInstance dataNameInstance = useName_data ?? (useName_data = EventData.DataName.DataNameInstance.AddName(new()
                {
                    nameGetter = () => currentName,
                    nameSetter = (newName) => currentName = newName,
                    typeGetter = () => DataType,
                    isAliveChecker = () => aliveChecker?.Invoke() ?? false,
                }));
                return dataNameInstance;

            }
        }

        private void BuildHelpInfo()
        {
            var count = EventData.DataNameD.AllNameInstance.Where((data) => data.DataName == currentName).Count();
            var refCount = $"引用次数:{count}";
            var help = "如果不设置名字将不会被输出";


            string[] helpInfoList = { refCount, help };
            helpInfo = string.Join(Environment.NewLine, helpInfoList);
        }

        /// <summary>重命名</summary>
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





        protected abstract Type DataType { get; }


    }












    [Serializable]
    [InlineProperty]
    public class OutDataInspector<T> : OutDataInspector
    {
        protected override Type DataType => typeof(T);





        public OutDataInspector(string dataName = "", Func<bool> aliveChecker = null)
        {
            currentName = dataName;
            typeName = typeof(T).Name;
            this.aliveChecker = aliveChecker;
        }


    }


    public class OutDataInspectorVector2 : OutDataInspector<Vector2>
    {
       
    }


}



