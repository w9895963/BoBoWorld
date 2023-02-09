using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configure
{
    public static partial class ConfigureCoreF
    {
        ///<summary> 获得所有配置组件的(名字,类型)字典, 结果已排序 </summary>
        public static Dictionary<string, Type> NameTypeDict
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
                foreach (var item in Data.NameTypeDict_preset)
                {
                    list.Add(item);
                }



                //获得命名空间为 ConfigureItem 的所有类
                typeof(ConfigureItemBase).GetSubTypes().ForEach(t =>
                {
                    if (t.IsSubclassOf(typeof(ConfigureItemBase)))
                    {
                        ConfigureItemBase ins = Activator.CreateInstance(t) as ConfigureItemBase;
                        //如果没有菜单名，就不添加
                        if (ins.MenuName.IsNotEmpty())
                        {
                            list.Add(new KeyValuePair<string, Type>(ins.MenuName, t));
                        }

                    }
                });


                //获得接口为 IConfigItemInfo 的所有类
                typeof(ConfigureItem).GetSubTypes().ForEach(t =>
                {
                    //如果继承了接口 IConfigItemInfo
                    if (t.GetInterfaces().Contains(typeof(IConfigItemInfo)))
                    {
                        IConfigItemInfo ins = Activator.CreateInstance(t) as IConfigItemInfo;
                        //如果没有菜单名，就不添加
                        if (ins.MenuName.IsNotEmpty())
                        {
                            list.Add(new KeyValuePair<string, Type>(ins.MenuName, t));
                        }

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
        private static Dictionary<string, Type> configureTypeDict_all = null;


        
    }
}