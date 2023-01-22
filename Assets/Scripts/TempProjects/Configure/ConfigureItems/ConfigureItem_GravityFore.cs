using System;
using System.Collections.Generic;

using EventData;
using StackableDecorator;
using UnityEditor;

using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItem
{




    [System.Serializable]
    public class ConfigureItem_GravityFore : ConfigureItemBase, IConfigureItemEnabler
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







       

        private string gravityVectorName => p重力向量.dataName;
        private string groundNormalName => p地表法线.dataName;


        private EventDataHandler<Vector2> gravityVectorD;
        private Vector2 gravityVector => gravityVectorD.Data;
        private EventDataHandler<Vector2> groundNormalD;
        private Vector2 groundNormal => groundNormalD.Data;


        private string gravityForceName => p重力施力.dataName;
        private EventDataHandler<Vector2> gravityForceD;
        private Vector2 gravityForce { set => gravityForceD.Data = value; }

        private (Action Enable, Action Disable) enabler;

        void IConfigureItemEnabler.Init(GameObject gameObject)
        {
            gravityVectorD = EventDataF.GetData<Vector2>(gravityVectorName, gameObject);
            groundNormalD = EventDataF.GetData<Vector2>(groundNormalName, gameObject);
            gravityForceD = EventDataF.GetData<Vector2>(gravityForceName, gameObject);

            (EventData.Core.EventData data, Func<bool> check)[] checks = {
                gravityVectorD.OnUpdateCondition,
                groundNormalD.OnUpdateCondition,
            };
            EventDataF.CreateConditionEnabler(OnDataChanged, OnConditionFail, ref enabler, checks);
            OnDataChanged();


            void OnDataChanged()
            {
                gravityForce = gravityVector;
            }

            void OnConditionFail()
            {
                gravityForce = gravityVector;
            }
        }
        void IConfigureItemEnabler.Destroy(GameObject gameObject)
        {
        }
        void IConfigureItemEnabler.Enable(GameObject gameObject)
        {
            enabler.Enable();
        }
        void IConfigureItemEnabler.Disable(GameObject gameObject)
        {
            enabler.Disable();
        }





























    }







}