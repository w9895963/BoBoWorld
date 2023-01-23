using System;
using System.Collections.Generic;

using EventData;
using StackableDecorator;
using UnityEditor;

using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItem
{


    public static partial class ConfigureItemBaseEnablerGroup
    {



        [System.Serializable]
        public class ConfigureItem_GroundNormalDetect : ConfigureItemBaseEnabler
        {




            #region //&界面部分


            [Header("固定参数")]

            //原点偏移

            public Vector2 offset = Vector2.zero;
            [Tooltip("计算出的行走力大小依据这个加速度计算")]
            public Vector2 最大加速度 = Vector2.zero;
            [Tooltip("生成一个和地面法线相反的里，将物体固定在地上")]
            public float 向地施力大小 = 50;

            [Header("动态参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, title = "移动指令", tooltip = "获得移动指令")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<float> 移动指令 = new Configure.InspectorInterface.DataHolder_NameDropDown<float>(DataName.全局_输入_移动横向值);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得脚下的地面法线")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> 地表法线 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, title = "运动速度", tooltip = "获得物体的运动速度")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> 运动速度 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.运动速度向量);











            [Header("输出参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, tooltip = "根据输入计算出行走施力")]
            public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> 行走施力 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.行走施力);




            //脚本说明
            public InspectorInterface.ShowOnlyText 说明 = new InspectorInterface.ShowOnlyText("根据一系列参数计算出施加于物体上的用于行走的力", "输入: 输入指令_移动, 地表法线, 运动速度", "输出: 行走施力");



            #endregion
            //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


            //构造函数
            public ConfigureItem_GroundNormalDetect()
            {
            }

            public override string MenuName => "物理/地面法线检测";

            public override Type[] RequireComponents => null;

            public override void Init(GameObject gameObject)
            {
                throw new NotImplementedException();
            }
            public override void Destroy(GameObject gameObject)
            {
                throw new NotImplementedException();
            }

            public override void Disable(GameObject gameObject)
            {
                throw new NotImplementedException();
            }

            public override void Enable(GameObject gameObject)
            {
                throw new NotImplementedException();
            }


        }



    }



}