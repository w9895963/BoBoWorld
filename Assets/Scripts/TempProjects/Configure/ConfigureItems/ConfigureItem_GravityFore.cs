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



        [Header("动态参数")]

        [Tooltip("默认重力:当没有其他任何影响, 物体的重力为此值, 若不成功则为零")]
        public Inspector.InputDataOrValue 默认重力 = new(typeof(Vector2), EventData.DataName.Preset.PresetName.重力向量.ToString(), new Vector2(0, -9.8f));




        public Inspector.ConditionTriggerList 触发条件 = new Inspector.ConditionTriggerList(){labelName = "触发条件"};





        [Header("输出参数")]

        [Tooltip("")]
        public Configure.Inspector.DataNameDropDown<Vector2> 重力施力 = new Configure.Inspector.DataNameDropDown<Vector2>(EventData.DataName.Preset.PresetName.重力施力);




        //脚本说明
        public Inspector.HelpText 说明 = new Inspector.HelpText("计算物体此时所受重力");



        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        public override string MenuName => "物理/重力施力";

        public override Type[] RequireComponentsOnGameObject => null;
        public override ItemRunnerBase CreateRunnerOver(GameObject gameObject)
        {
            return new Runner() { gameObject = gameObject, config = this };
        }

















        public class Runner : ItemRunnerBase<ConfigureItem_GravityFore>
        {

            // 预设重力
            private Func<Vector2> getPresetGravity;
            private Vector2 presetGravity => getPresetGravity();

            // 重力设置
            private Action<Vector2> setPresetGravity;
            private Vector2 gravityForce { set => setPresetGravity(value); }
            private (Action Enable, Action Disable) enabler = (null, null);

            public override void Init()
            {
                var outDataHolder = config.默认重力.CreateDataHandlerInstance<Vector2>(gameObject);
                getPresetGravity = () => outDataHolder.data;
                EventDataF.CreateConditionEnabler(CalcGravity, GravityDisable, ref enabler);
            }








            private void CalcGravity()
            {
                gravityForce = presetGravity;
            }
            private void GravityDisable()
            {
                gravityForce = Vector2.zero;
            }





            public override void Destroy()
            {
            }

            public override void Disable()
            {
                enabler.Disable();
            }

            public override void Enable()
            {
                enabler.Enable();
            }


        }





    }

}