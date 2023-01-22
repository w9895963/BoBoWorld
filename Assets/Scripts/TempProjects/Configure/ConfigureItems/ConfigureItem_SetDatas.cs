using System;
using System.Collections.Generic;
using System.Linq;

using Configure.InspectorInterface;

using EventData;

using StackableDecorator;

using UnityEditor;

using UnityEngine;

//命名空间：配置
namespace Configure
{



    namespace ConfigureItem
    {


        [System.Serializable]
        public partial class ConfigureItem_SetDatas : ConfigureItemBase
        {


            #region //&界面部分            

            [Header("参数")]


            [Tooltip("地面向量与重力的夹角大于此角度则不视为地面")]
            public float 地面最大夹角 = 10;

            [Tooltip("此标签外的物体不被视为地面")]
            [TagPopup]
            [Label(0)]
            public List<string> 地面标签 = new List<string>() { "地表碰撞体" };




            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力向量", tooltip = "获得重力向量")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> 重力 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.重力向量);


            public List<SetDataHolder> setDataHolders = new List<SetDataHolder>();


            [Serializable]
            public class SetDataHolder
            {
                [NaughtyAttributes.AllowNesting]
                [NaughtyAttributes.OnValueChanged(nameof(OnDataNameChanged))]
                [NaughtyAttributes.Label("数据名")]
                [NaughtyAttributes.Dropdown(nameof(dataNames))]
                public string dataName;

                [NaughtyAttributes.AllowNesting]
                [NaughtyAttributes.OnValueChanged(nameof(OnDataValueChanged))]
                public string dataValueStr;

                [HideInInspector]
                public System.Object dataValue => ConvertData();

                private string[] dataNames => DataNameF.GetDataNamesList();
                private void OnDataNameChanged()
                {
                    // 赋默认值
                    //数据类型
                    var type = DataNameF.GetType(dataName);
                    //找到默认值
                    if (defaultValueDict.TryGetValue(type, out string defaultValue))
                    {
                        dataValueStr = defaultValue;
                    }
                }
                private void OnDataValueChanged()
                {
                    //非空则检查类型
                    object v = ConvertData();
                    if (v != null)
                    {
                        Debug.Log("输入类型正确");
                    }
                    else
                    {
                        Debug.Log($"输入 {dataValueStr} 无法被视为数据 {dataName} 的类型 {DataNameF.GetType(dataName)}");
                    }
                }


                private System.Object ConvertData()
                {
                    object re = null;
                    //数据类型
                    var type = DataNameF.GetType(dataName);
                    //检测是否成功

                    //用通用转换检测
                    try
                    {
                        re = Convert.ChangeType(dataValueStr, type);
                    }
                    catch (Exception)
                    {
                    }


                  


                    return re;
                }




                //默认值字典
                private Dictionary<Type, string> defaultValueDict = new Dictionary<Type, string>()
                {
                    {typeof(int),"0"},
                    {typeof(float),"0"},
                    {typeof(bool),"false"},
                    {typeof(Vector2),"0,0"},
                    {typeof(Vector3),"0,0,0"},
                    {typeof(Vector4),"0,0,0,0"},
                    {typeof(Quaternion),"0,0,0,0"},
                    {typeof(Color),"0,0,0,0"},
                    {typeof(string),""},
                };
            }







            [Header("输出参数")]
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得脚下的地面法线")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> 地表法线 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, tooltip = "此刻是否与地面物体物理接触")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<bool> 是否与地面物体物理接触 = new Configure.InspectorInterface.DataHolder_NameDropDown<bool>(DataName.是否与地面物体物理接触);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面物体", tooltip = "获得脚下的地面物体")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<GameObject> 地面物体 = new Configure.InspectorInterface.DataHolder_NameDropDown<GameObject>(DataName.地面物体);





            [Space(10)]
            public InspectorInterface.ShowOnlyText 说明 = new InspectorInterface.ShowOnlyText("检测地面, 并获得一系列地面信息");

            #endregion 
            //&↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




















            //构建函数
            public ConfigureItem_SetDatas()
            {
                requiredTypes = null;
                CreateRunnerFunc<Runner, ConfigureItem_SetDatas>();

            }





            private class Runner : ConfigureRunnerT<ConfigureItem_SetDatas>, IConfigureRunnerBuilder
            {

                private (Action enable, Action disable) enabler;




                //界面






                void IConfigureRunnerBuilder.Init()
                {
                }
                void IConfigureRunnerBuilder.Enable()
                {
                    enabler.enable?.Invoke();
                }

                void IConfigureRunnerBuilder.Disable()
                {
                    enabler.disable?.Invoke();
                }


                void IConfigureRunnerBuilder.Destroy()
                {

                }








            }





















        }








    }

}

