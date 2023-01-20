//命名空间：配置
using System;
using System.Collections.Generic;
using Configure.ConfigureItem;

namespace Configure
{
    //预设数据
    public static partial class PresetData
    {
        //配置类型与文字字典
        public static Dictionary<string, Type> ConfigureTypeDict = new Dictionary<string, Type>() {
            { "选择配置类型",null},
            { "物理/力量施加器", typeof(ConfigureItem_ApplyForce)},
            { "物理/获取物理量",typeof(ConfigureItem_GetPhysicData)},
            { "物理/地面检测器",typeof(ConfigureItem_GroundFinder)},
            { "物理/计算行走施力",typeof(ConfigureItem_WalkFore)},
            { "位置/同步位置",typeof(ConfigureItem_PositionLocked)},
        };
    }





}