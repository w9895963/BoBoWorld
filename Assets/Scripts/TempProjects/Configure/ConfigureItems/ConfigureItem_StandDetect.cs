using System;
using System.Collections.Generic;
using System.Linq;

using Configure.Interface;

using EventData;

using StackableDecorator;

using UnityEditor;

using UnityEngine;

using UnityTimer;

//命名空间：配置
namespace Configure
{



    namespace ConfigureItem
    {


        [System.Serializable]
        public partial class ConfigureItem_StandDetect : ConfigureItemBase
        {


            #region //&界面部分            

            [Header("参数")]


            [Tooltip("")]
            [StackableField]
            [Label(title = "延迟判断时间", tooltip = "当物理脱离地面超过此时间则视为不再站立在地面上")]
            public float p延迟判断时间 = 0.1f;



            //^
            [Header("动态参数")]


            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "是否与地面物体物理接触", tooltip = "此刻是否与地面物体物理接触")]
            public Configure.Interface.DataHolder_NameDropDown<bool> p是否与地面物体物理接触 = new Configure.Interface.DataHolder_NameDropDown<bool>(DataName.是否与地面物体物理接触);





            //^
            [Header("输出参数")]


            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "是否站立在地面", tooltip = "判断是否站立在地面")]
            public Configure.Interface.DataHolder_NameDropDown<bool> p是否站立在地面 = new Configure.Interface.DataHolder_NameDropDown<bool>(DataName.是否站在地面);





            [Space(10)]
            public Interface.ShowOnlyText 说明 = new Interface.ShowOnlyText("当物理脱离地面超过此时间则视为不再站立在地面上", "为了避免一些极短时间的物理脱离地面导致的误判，此处设置了一个延迟判断时间");

            #endregion 
            //&↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




















            //构建函数
            public ConfigureItem_StandDetect()
            {
                CreateRunnerFunc<Runner, ConfigureItem_StandDetect>();

            }





            private class Runner : ConfigureRunnerT<ConfigureItem_StandDetect>, IConfigureRunnerBuilder
            {

                private float time;
                private EventDataHandler<bool> standOn;
                private EventDataHandler<bool> contactGround;
                private (Action Enable, Action Disable) enabler;
                private Timer timer;










                void IConfigureRunnerBuilder.Init()
                {
                    contactGround = EventDataF.GetData<bool>(config.p是否与地面物体物理接触.dataName, gameObject);
                    time = config.p延迟判断时间;
                    standOn = EventDataF.GetData<bool>(config.p是否站立在地面.dataName, gameObject);
                    contactGround.OnUpdateDo_AddEnabler(OnContactGroundUpdate, ref enabler);

                }
                void IConfigureRunnerBuilder.Enable()
                {
                    enabler.Enable?.Invoke();
                }

                void IConfigureRunnerBuilder.Disable()
                {
                    enabler.Disable?.Invoke();
                    timer.Cancel();
                }


                void IConfigureRunnerBuilder.Destroy()
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

