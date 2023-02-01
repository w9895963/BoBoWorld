using System;
using System.Collections.Generic;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItems
{



    [System.Serializable]
    public class ConfigureItem_GravityFore : ConfigureItemBase
    {




        #region //&界面部分
        [Header("静态参数")]

        [Tooltip("默认重力")]
        public Vector2 p默认重力 = new Vector2(0, -10);



        [Header("动态参数")]

        [Tooltip("")]
        public Configure.Inspector.DataNameDropDown<Vector2> p重力向量 = new Configure.Inspector.DataNameDropDown<Vector2>(DataName.重力向量);
        [Tooltip("")]
        public Configure.Inspector.DataNameDropDown<Vector2> p地表法线 = new Configure.Inspector.DataNameDropDown<Vector2>(DataName.地表法线);









        [Header("输出参数")]

        [Tooltip("")]
        public Configure.Inspector.DataNameDropDown<Vector2> p重力施力 = new Configure.Inspector.DataNameDropDown<Vector2>(DataName.重力施力);




        //脚本说明
        public Inspector.HelpText 说明 = new Inspector.HelpText("计算重力");



        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        public override string MenuName => "物理/重力施力";

        public override Type[] RequireComponents => null;








        public string gravityVectorName => p重力向量.dataName;
        public string groundNormalName => p地表法线.dataName;
        public string gravityForceName => p重力施力.dataName;








        public class Runner : ItemRunnerBase<ConfigureItem_GravityFore>
        {


            private EventDataHandler<Vector2> gravityVectorD;
            private Vector2 gravityVector => gravityVectorD.Data;
            private EventDataHandler<Vector2> groundNormalD;
            private Vector2 groundNormal => groundNormalD.Data;



            private EventDataHandler<Vector2> gravityForceD;
            private Vector2 gravityForce { set => gravityForceD.Data = value; }






            private (Action Enable, Action Disable) enabler;

            public override void Init()
            {
                base.config.Log("Init");
                gravityVectorD = EventDataF.GetData<Vector2>(config.gravityVectorName, gameObject);
                groundNormalD = EventDataF.GetData<Vector2>(config.groundNormalName, gameObject);
                gravityForceD = EventDataF.GetData<Vector2>(config.gravityForceName, gameObject);

                EventDataF.CreateConditionEnabler(CalcGravity, null, gravityForceD.OnUpdateCondition, groundNormalD.OnUpdateCondition, gravityVectorD.OnUpdateCondition);
            }

            private void CalcGravity()
            {
                Vector2 gv = Vector2.zero;
                if (groundNormal.y < 0)
                {
                    gv = gravityVector * groundNormal.y;
                }





                gravityForce = gv;
            }

            public override void Destroy()
            {
            }

            public override void Disable()
            {
            }

            public override void Enable()
            {
            }


        }





    }

}