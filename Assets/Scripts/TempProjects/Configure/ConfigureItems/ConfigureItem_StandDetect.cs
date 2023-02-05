using System;
using System.Collections.Generic;
using System.Linq;

using Configure.Inspector;

using EventData;


using UnityEditor;

using UnityEngine;

using UnityTimer;

//命名空间：配置
namespace Configure
{



    namespace ConfigureItems
    {


        [System.Serializable]
        public partial class ConfigureItem_StandDetect : ConfigureItem
        {


            #region //&界面部分            

            [Header("参数")]


            [Tooltip("")]
            public float p延迟判断时间 = 0.1f;



            //^
            [Header("动态参数")]


            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<bool> p是否与地面物体物理接触 = new Configure.Inspector.DataNameDropDown<bool>(EventData.DataName.Preset.PresetName.是否与地面物体物理接触);





            //^
            [Header("输出参数")]


            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<bool> p是否站立在地面 = new Configure.Inspector.DataNameDropDown<bool>(EventData.DataName.Preset.PresetName.是否站在地面);





            [Space(10)]
            public Inspector.HelpText 说明 = new Inspector.HelpText("当物理脱离地面超过此时间则视为不再站立在地面上", "为了避免一些极短时间的物理脱离地面导致的误判，此处设置了一个延迟判断时间");

            #endregion 
            //&↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




















            //构建函数
            public ConfigureItem_StandDetect()
            {
                CreateRunnerFunc<Runner, ConfigureItem_StandDetect>();

            }





            private class Runner : ConfigureRunnerT<ConfigureItem_StandDetect>, IConfigureItemRunner
            {

                private float time;
                private EventDataHandler<bool> standOn;
                private EventDataHandler<bool> contactGround;
                private (Action Enable, Action Disable) enabler;
                private Timer timer;










                void IConfigureItemRunner.Init()
                {
                    contactGround = EventDataF.GetData<bool>(config.p是否与地面物体物理接触.dataName, gameObject);
                    time = config.p延迟判断时间;
                    standOn = EventDataF.GetData<bool>(config.p是否站立在地面.dataName, gameObject);
                    contactGround.OnUpdateDo_AddEnabler(OnContactGroundUpdate, ref enabler);

                }
                void IConfigureItemRunner.Enable()
                {
                    enabler.Enable?.Invoke();
                }

                void IConfigureItemRunner.Disable()
                {
                    enabler.Disable?.Invoke();
                    timer.Cancel();
                }


                void IConfigureItemRunner.Destroy()
                {

                }


                private void OnContactGroundUpdate(bool contact)
                {
                    //当脱离地面时
                    if (contact == false)
                    {
                        timer = Timer.Register(time, () => { standOn.Data = false; });
                    }
                    //当接触地面时
                    else
                    {
                        timer.Cancel();
                        standOn.Data = true;
                    }
                }

            }





















        }








    }

}

