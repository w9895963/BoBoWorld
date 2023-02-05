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
    public class OutDataInspector<T>
    {
        #region //&界面



        [ValueDropdown(nameof(dataNamesDropDownList))]
        [SuffixLabel("$" + nameof(type), true)]
        [HorizontalGroup("A")]
        [ShowIf(nameof(uiTab), 0)]
        [HideLabel]
        public string currentName;
        [HideInInspector]
        public string type;
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
        [HorizontalGroup("A", MaxWidth = 18)]
        private void helpIcon() { }



        private int uiTab = 0;




        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



        public OutDataInspector(string dataName = "", Func<bool> aliveChecker = null)
        {
            currentName = dataName;
            type = typeof(T).Name;
            this.aliveChecker = aliveChecker;
        }

        public void OnValidate()
        {
            //~初始化
            Init();

            //~固定名称
            type = typeof(T).Name;


            //~重命名
            if (doRename)
            {
                doRename = false;
                Rename(rename);
            }

            //~数据名改变
            if (currentName != customData.DataName)
            {
                customData.DataName = currentName;
            }

            //~获得帮助信息
            //引用次数
            var count = EventData.DataNameD.AllNameInstance.Where((data) => data.DataName == currentName).Count();
            var refCount = $"引用次数:{count}";
            var help = "";


            string[] helpInfoList = { refCount, help };
            helpInfo = string.Join(Environment.NewLine, helpInfoList);


        }

        public Action<T> CreateDataSetter(GameObject gameObject)
        {
            return (data) => EventData.EventDataF.GetData<T>(currentName, gameObject).Data = data;
        }




        private Func<bool> aliveChecker;
        private bool isInitial => customData != null;
        private CustomData customData;
        private IEnumerable dataNamesDropDownList => GetDataNamesDropDownList();



        private void Init()
        {
            if (!isInitial)
            {
                customData = new CustomData<T>(currentName, aliveChecker);
                customData.onDataNameChangeAction = (newName) =>
                {
                    currentName = newName;
                };
            }
        }
        private static IEnumerable GetDataNamesDropDownList()
        {
            ValueDropdownList<string> valueDropdownList = new ValueDropdownList<string>();
            EventData.DataNameF.GetDataNameInfoList().Where((data) => data.DataType == typeof(T)).ForEach((data) =>
            {
                string countStr = data.InstanceCount == 0 ? "" : $"({data.InstanceCount})";
                valueDropdownList.Add($"{data.DataGroup}/{countStr}{data.DataName}", data.DataName);
            });

            return valueDropdownList;
        }



        /// <summary>重命名</summary>
        private void Rename(string newName)
        {
            customData.Rename(newName);
        }


    }







}