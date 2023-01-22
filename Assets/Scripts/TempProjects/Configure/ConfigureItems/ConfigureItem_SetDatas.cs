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

            [SerializeField]
            private List<InspectorInterface.InputDataAndValue> 设置数据 = new(1);
            [HideInInspector]
            public List<(string dataName, System.Object dataValue)> p设置数据 => 设置数据.Select(x => (x.dataName, x.dataValue)).ToList();











            [Space(30)]
            public InspectorInterface.ShowOnlyText 说明 = new InspectorInterface.ShowOnlyText("为数据赋予一个初始值");

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




                //数据名,数据值列表
                private List<(string dataName, System.Object dataValue)> dataList => config.p设置数据;






                void IConfigureRunnerBuilder.Init()
                {
                    dataList.ForEach(x =>
                    {
                        var name = x.dataName;
                        EventDataHandler eventDataHandler = EventDataF.GetData(name, gameObject);
                        if (eventDataHandler != null)
                        {
                            eventDataHandler.Data = x.dataValue;
                        }

                    });
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

