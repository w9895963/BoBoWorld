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
    public class ConfigureItem_GravityFore : ConfigureItemBase
    {




        #region //&界面部分





        [Header("动态参数")]

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力向量", tooltip = "获得重力向量")]
        public Configure.Interface.DataHolder_NameDropDown<Vector2> p重力向量 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.重力向量);
        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得地表法线")]
        public Configure.Interface.DataHolder_NameDropDown<Vector2> p地表法线 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);









        [Header("输出参数")]

        [Tooltip("")]
        [StackableField]
        [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力施力", tooltip = "根据输入计算出重力施力")]
        public Configure.Interface.DataHolder_NameDropDown<Vector2> p重力施力 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.重力施力);




        //脚本说明
        public Interface.ShowOnlyText 说明 = new Interface.ShowOnlyText("计算重力");



        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        //构造函数
        public ConfigureItem_GravityFore()
        {
            CreateRunnerFunc<Runner, ConfigureItem_GravityFore>();
        }









        //类:计算核心
        private class Runner : ConfigureRunnerT<ConfigureItem_GravityFore>, IConfigureRunnerBuilder
        {



            //地面法线
            private string groundNormalName => config.p地表法线.dataName;
            private EventDataHandler<Vector2> groundNormalD;
            private Vector2 groundNormal => groundNormalD.Data;



            //重力向量
            private string gravityVectorName => config.p重力向量.dataName;
            private EventDataHandler<Vector2> gravityVectorD;
            private Vector2 gravityVector => gravityVectorD.Data;

            //重力施力
            private string gravityForceName => config.p重力施力.dataName;
            private EventDataHandler<Vector2> gravityForceD;
            private Vector2 gravityForce { set => gravityForceD.Data = value; }




            //启动器
            private (Action Enable, Action Disable) enabler;








            void IConfigureRunnerBuilder.Init()
            {
                groundNormalD = EventDataF.GetData<Vector2>(groundNormalName, gameObject);
                gravityVectorD = EventDataF.GetData<Vector2>(gravityVectorName, gameObject);
                gravityForceD = EventDataF.GetData<Vector2>(gravityForceName, gameObject);


                (EventData.Core.EventData data, Func<bool> check)[] checks =
                {
                    gravityForceD.OnUpdateCondition,
                    gravityVectorD.OnUpdateCondition,
                    groundNormalD.OnUpdateCondition,
                };
                EventDataF.CreateConditionEnabler(CalcGravity, ZeroGravity, ref enabler, checks);
            }




            void IConfigureRunnerBuilder.Enable()
            {
                enabler.Enable?.Invoke();
            }
            void IConfigureRunnerBuilder.Disable()
            {
                enabler.Disable?.Invoke();
            }

            void IConfigureRunnerBuilder.Destroy()
            {

            }


            private void CalcGravity()
            {
                //如果地面法线为0则重力为默认重力
                if (groundNormal == Vector2.zero)
                {
                    gravityForce = gravityVector;
                    return;
                }
                //否则重力将朝向地面
                else
                {
                    gravityForce = gravityVector.RotateTo(-groundNormal);
                }

            }

            //零重力
            private void ZeroGravity()
            {
                gravityForce = Vector2.zero;
            }


        }











    }







}