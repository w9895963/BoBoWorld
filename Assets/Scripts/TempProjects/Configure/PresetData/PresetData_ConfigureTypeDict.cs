//命名空间：配置
using System;
using System.Collections.Generic;
using System.Linq;
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
                //如果已经初始化过了，就直接返回
                if (configureTypeDict_all != null)
                {
                    return configureTypeDict_all;
                }

                //否则，先把预设的字典复制过来, 用列表
                List<KeyValuePair<string, Type>> list = new List<KeyValuePair<string, Type>>();
                foreach (var item in configureTypeDict_preset)
                {
                    list.Add(item);
                }

                //获得所有 ConfigureItemHolder 内部的类
                typeof(ConfigureItemBaseEnablerGroup).GetNestedTypes().ForEach(t =>
                {
                    if (t.IsSubclassOf(typeof(ConfigureItemBaseEnabler)))
                    {
                        ConfigureItemBaseEnabler ins = Activator.CreateInstance(t) as ConfigureItemBaseEnabler;
                        list.Add(new KeyValuePair<string, Type>(ins.MenuName, t));

                    }
                });

                //排序
                list.Sort((a, b) => a.Key.CompareTo(b.Key));

                //将第一个移动到最前
                list.Move(list.FindIndex(a => a.Value == null), 0);

                //转换成字典
                configureTypeDict_all = new Dictionary<string, Type>();
                foreach (var item in list)
                {
                    configureTypeDict_all.TryAdd(item.Key, item.Value);
                }


                return configureTypeDict_all;
            }
        }
        //写在外部的配置类型字典
        private static Dictionary<string, Type> configureTypeDict_preset = new Dictionary<string, Type>() {
            { "选择配置类型",null},

            { "数据操作/共享数据", typeof(ConfigureItem_SetDatas)},

            { "物理/力量施加器", typeof(ConfigureItem_ApplyForce)},
            { "物理/获取物理量",typeof(ConfigureItem_GetPhysicData)},
            { "物理/地面检测器",typeof(ConfigureItem_GroundFinder)},
            { "物理/站立检测器",typeof(ConfigureItem_StandDetect)},
            { "物理/计算行走施力",typeof(ConfigureItem_WalkFore)},

            { "摄像机/位置同步",typeof(ConfigureItem_PositionLocked)},
        };

        private static Dictionary<string, Type> configureTypeDict_all = null;

    }





}