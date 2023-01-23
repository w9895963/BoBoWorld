using System;
using System.Collections.Generic;
using System.Linq;
using EventData.CustomDataNameCore;
using UnityEngine;

namespace EventData.CustomDataNameCore
{
    //可编辑脚本对象: 数据名列表
    [CreateAssetMenu(fileName = "数据名列表", menuName = "配置/数据名列表", order = 0)]
    public partial class CustomDataName : ScriptableObject
    {
        public List<DataNameSetup> 数据名列表 = new List<DataNameSetup>();

        ///<summary> 获得所有自定义数据名和类型的列表, 允许重复, 允许空类型</summary>
        public static List<(string, Type)> GetAllAdditionDataNamesAndTypesList()
        {
            List<(string, Type)> re = new List<(string, Type)>();
            //获得所有数据名列表
            GameObject.FindObjectOfType<CustomDataNameLoaderMono>()?.数据列表.ForEach((list) =>
            {
                //获得所有数据名
                list.数据名列表.ForEach((data) =>
                {
                    // 如果数据名不为空, 且类型不为空
                    if (data.DataNameFull.IsNotEmpty())
                        //添加数据名和类型
                        re.Add((data.DataNameFull, data.type));
                });
            });

            return re;
        }





        [Serializable]
        public class DataNameSetup
        {
            #region //&界面


            [SerializeField]
            [HideInInspector]
            private string labelName;



            [SerializeField]
            [NaughtyAttributes.AllowNesting]
            [NaughtyAttributes.Label("数据名")]
            private string dataName = "数据名";


            [SerializeField]
            [NaughtyAttributes.Label("数据类型")]
            [NaughtyAttributes.Dropdown(nameof(AllTypes))]
            [NaughtyAttributes.AllowNesting]
            [NaughtyAttributes.DisableIf(nameof(autoType))]
            private string typeName = "选择类型名";

            [SerializeField]
            [NaughtyAttributes.AllowNesting]
            [NaughtyAttributes.Label("自动猜测类型")]
            private bool autoType = false;

            [SerializeField]
            [NaughtyAttributes.AllowNesting]
            [NaughtyAttributes.Label("全局属性")]
            private bool IsGlobal;

            [SerializeField]
            [NaughtyAttributes.AllowNesting]
            [NaughtyAttributes.ReadOnly]
            private string 数据重复检测;

            #endregion
            //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑







            public Type type => EventData.DataNameF.GetAllTypes().Where((t) => t.ToString() == typeName).FirstOrDefault();




            public string DataNameFull
            {
                get
                {
                    string pre = IsGlobal ? "全局_" : "";
                    string n = dataName;
                    return pre + n;
                }
            }

            public void OnValidate()
            {
                UpdateLabelName();
                checkDuplicate();
                CheckType();
            }

            private void UpdateLabelName()
            {
                labelName = $"数据名: {DataNameFull}; 数据类型: {typeName}";
            }
            //检查数据名是否重复
            private void checkDuplicate()
            {
                string[] Prenames = DataNameF.GetDataNamesList_PresetName();
                IEnumerable<string> customNames = GetAllAdditionDataNamesAndTypesList().Select((x) => x.Item1);
                string[] names = Prenames.Concat(customNames).ToArray();
                //如果数据在所有数据名中
                if (names.Count((n) => n == DataNameFull) > 1)
                {
                    数据重复检测 = "数据名重复";
                }
                else
                {
                    数据重复检测 = "可用";
                }

            }

            private void CheckType()
            {
                if (autoType)
                {
                    string v = EventData.DataNameF.GetPresetType(DataNameFull)?.ToString();
                    if (v.IsEmpty())
                    {
                        v = AllTypes[0];
                    }
                    typeName = v;
                }
            }


            private string[] AllTypes => EventData.DataNameF.GetAllTypes().Select((t) => t.ToString()).Prepend("未定义类型").ToArray();
        }



        //unity事件:更新时
        private void OnValidate()
        {
            foreach (var item in 数据名列表)
            {
                item.OnValidate();
            }
        }
    }
}



