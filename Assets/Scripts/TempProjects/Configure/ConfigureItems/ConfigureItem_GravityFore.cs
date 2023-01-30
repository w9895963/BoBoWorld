using System;
using System.Collections.Generic;

using EventData;
using StackableDecorator;
using UnityEditor;

using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItems
{



    [System.Serializable]
    public class ConfigureItem_GravityFore : ConfigureItemBase
    {




        #region //&界面部分





        [Header("动态参数")]

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力向量", tooltip = "获得重力向量")]
        public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> p重力向量 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.重力向量);
        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得地表法线")]
        public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> p地表法线 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);









        [Header("输出参数")]

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力施力", tooltip = "根据输入计算出重力施力")]
        public Configure.InspectorInterface.DataHolder_NameDropDown<Vector2> p重力施力 = new Configure.InspectorInterface.DataHolder_NameDropDown<Vector2>(DataName.重力施力);




        //脚本说明
        public InspectorInterface.ShowOnlyText 说明 = new InspectorInterface.ShowOnlyText("计算重力");



        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        public override string MenuName => "物理/重力施力";

        public override Type[] RequireComponents => null;








        public string gravityVectorName => p重力向量.dataName;
        public string groundNormalName => p地表法线.dataName;
        public string gravityForceName => p重力施力.dataName;








        public class Runner : ItemRunnerBase
        {

            private ConfigureItem_GravityFore Config => config as ConfigureItem_GravityFore;

            private EventDataHandler<Vector2> gravityVectorD;
            private Vector2 gravityVector => gravityVectorD.Data;
            private EventDataHandler<Vector2> groundNormalD;
            private Vector2 groundNormal => groundNormalD.Data;



            private EventDataHandler<Vector2> gravityForceD;
            private Vector2 gravityForce { set => gravityForceD.Data = value; }






            private (Action Enable, Action Disable) enabler;

            public override void Init()
            {
                gravityVectorD = EventDataF.GetData<Vector2>(Config.gravityVectorName, gameObject);
                groundNormalD = EventDataF.GetData<Vector2>(Config.groundNormalName, gameObject);
                gravityForceD = EventDataF.GetData<Vector2>(Config.gravityForceName, gameObject);
            }


            public override void Destroy()
            {
                throw new NotImplementedException();
            }

            public override void Disable()
            {
                throw new NotImplementedException();
            }

            public override void Enable()
            {
                throw new NotImplementedException();
            }


        }





    }

}