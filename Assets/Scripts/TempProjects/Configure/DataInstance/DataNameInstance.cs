using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EventData.PresetNameF;

namespace Configure.DataInstance
{





    //*静态
    public partial class DataNameInstance
    {
        private static List<DataNameInstance> allNameInstance = new List<DataNameInstance>();
        public static List<DataNameInstance> AllNameInstance => allNameInstance;


        public static DataNameInstance AddName(Func<string> nameGetter, Action<string> nameSetter, Type type,
            Func<bool> isAliveChecker = null, Object identifier = null)
        {
            DataNameInstance name = new()
            {
                nameGetter = nameGetter,
                nameSetter = nameSetter,
                typeGetter = () => type,
                aliveChecker = isAliveChecker ?? (() => true),
                identifier = identifier,
            };
            allNameInstance.AddNotNull(name);
            return name;
        }
        public static DataNameInstance AddName(DataNameInstanceConfig config)
        {
            DataNameInstance name = new()
            {
                nameGetter = config.nameGetter,
                nameSetter = config.nameSetter,
                typeGetter = config.typeGetter,
                aliveChecker = config.aliveChecker,
                identifier = config.identifier,
            };
            allNameInstance.AddNotNull(name);
            return name;
        }
        public class DataNameInstanceConfig
        {
            public Func<string> nameGetter;
            public Action<string> nameSetter;
            public Func<Type> typeGetter;

            public Func<bool> aliveChecker;
            public Object identifier;
        }

        /// <summary>移除不活动数据</summary>
        public static void RemoveDeadData()
        {
            allNameInstance.RemoveAll((data) => !data.aliveChecker());
        }

        /// <summary>移除标记物的数据</summary>
        public static void RemoveData(Object identifier)
        {
            allNameInstance.RemoveAll((data) => data.identifier == identifier);
        }


    }

    //*接口定义
    public partial class DataNameInstance : IDataNameInstance
    {
        public string DataName { get => nameGetter?.Invoke(); set => nameSetter?.Invoke(value); }

        public Type DataType
        {
            get
            {
                return typeGetter?.Invoke();
            }
            set
            {
                typeGetter = () => value;
            }
        }
    }

    //*构建参数
    public partial class DataNameInstance
    {
        private Func<string> nameGetter;
        private Action<string> nameSetter;
        private Func<Type> typeGetter;

        private Func<bool> aliveChecker;
        private Object identifier;
    }






}