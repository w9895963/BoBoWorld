//命名空间：配置
using System;
using System.Collections.Generic;
using System.Linq;
using Configure.ConfigureItems;

namespace Configure
{
    //预设数据
    public static partial class Data
    {
        ///<summary> 配置组件的预设(名字,类型)字典 </summary>
        public static Dictionary<string, Type> NameTypeDict_preset = new Dictionary<string, Type>() {
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