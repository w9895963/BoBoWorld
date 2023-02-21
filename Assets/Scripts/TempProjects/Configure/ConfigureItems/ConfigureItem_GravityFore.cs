using System;
using System.Collections.Generic;
using EventData;
using UnityEditor;
using UnityEngine;
using Configure.Inspector;
using static EventData.DataName.Preset;



//命名空间：配置
namespace Configure.ConfigureItems
{



    [System.Serializable]
    public class ConfigureItem_GravityFore : ConfigureItem, IConfigItemInfo, IConfigureItem
    {




        #region //&界面部分



        [Header("动态参数")]

        [Tooltip("默认重力:当没有其他任何影响, 物体的重力为此值, 若不成功则为零")]
        public Inspector.InputDataOrValue 默认重力 = new(typeof(Vector2), EventData.DataName.Preset.PresetName.重力向量.ToString(), new Vector2(0, -9.8f));
        public DataReferOrValue<Vector2> 默认重力_ = new(cfg =>
        {
            cfg.defaultDataName =  PresetName.重力向量.ToString();
            cfg.defaultValue = new Vector2(0, -9.8f);
            cfg.dataType = typeof(Vector2);
            
            return cfg;
        });




        public Inspector.ConditionTriggerList 触发条件 = new Inspector.ConditionTriggerList() { labelName = "触发条件" };





        [Header("输出参数")]

        [Tooltip("")]
        public Configure.Inspector.DataNameDropDown<Vector2> 重力施力 = new Configure.Inspector.DataNameDropDown<Vector2>(EventData.DataName.Preset.PresetName.重力施力);




        //脚本说明
        public Inspector.HelpText 说明 = new Inspector.HelpText("计算物体此时所受重力");



        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




        string IConfigItemInfo.MenuName => "物理/重力施力";

        IConfigItemInfo.ConfigItemInfo IConfigItemInfo.OptionalInfo => null;

        CoreClass.InitedEnabler ICreate<MonoBehaviour, CoreClass.InitedEnabler>.Create(MonoBehaviour parm)
        {
            Runner runner = new Runner()
            {
                gameObject = parm.gameObject,
                config = this
            };

            return new CoreClass.InitedEnabler(runner);
        }



        public class Runner : IConfigureItemRunner
        {
            public GameObject gameObject;
            public ConfigureItem_GravityFore config;

            // 预设重力
            private IDataGetter<Vector2> presetGravityInspData=> config.默认重力_;
            private Func<Vector2> presetGravityGetter;
            private Vector2 presetGravity => (presetGravityGetter ??= presetGravityInspData.CreateGetter(gameObject))();

            // 重力设置
            private Action<Vector2> setPresetGravity;
            private Vector2 gravityForce { set => setPresetGravity(value); }



            // 启动器
            private (Action Enable, Action Disable) enabler = (null, null);

            public void OnInit()
            {
                var outDataHolder = config.默认重力.CreateDataHandlerInstance<Vector2>(gameObject);
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





            public void OnUnInit()
            {
            }

            public void OnDisable()
            {
                enabler.Disable();
            }

            public void OnEnable()
            {
                enabler.Enable();
            }


        }





    }

}