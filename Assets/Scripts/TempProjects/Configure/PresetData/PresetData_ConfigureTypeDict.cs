//命名空间：配置
using System;
using System.Collections.Generic;
using Configure.ConfigureItem;

namespace Configure
{
    //预设数据
    public static partial class Data
    {
        //配置类型与文字字典
        public static Dictionary<string, Type> ConfigureTypeDict
        {
            get
            {
                string v = nameof(Data.ConfigureTypeDict).Log();
                //获得所有 ConfigureItemHolder 内部的类
                typeof(ConfigureItemHolder).GetNestedTypes().ForEach(t =>
                {
                    if (t.IsSubclassOf(typeof(ConfigureItemBaseEnabler)))
                    {
                        ConfigureItemBaseEnabler ins = Activator.CreateInstance(t) as ConfigureItemBaseEnabler;
                        configureTypeDict.TryAdd(ins.MenuName, t);

                    }
                });


                return configureTypeDict;
            }
        }
        //写在外部的配置类型字典
        private static Dictionary<string, Type> configureTypeDict = new Dictionary<string, Type>() {
            { "选择配置类型",null},

            { "数据操作/共享数据", typeof(ConfigureItem_SetDatas)},

            { "物理/力量施加器", typeof(ConfigureItem_ApplyForce)},
            { "物理/获取物理量",typeof(ConfigureItem_GetPhysicData)},
            { "物理/地面检测器",typeof(ConfigureItem_GroundFinder)},
            { "物理/站立检测器",typeof(ConfigureItem_StandDetect)},
            { "物理/计算行走施力",typeof(ConfigureItem_WalkFore)},

            { "摄像机/位置同步",typeof(ConfigureItem_PositionLocked)},
        };

    }





}