using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace EventData.DataName
{




    //*静态
    public partial class DataNameInstance
    {
        private static List<DataNameInstance> allNameInstance = new List<DataNameInstance>();
        public static List<DataNameInstance> AllNameInstance => allNameInstance;


        public static DataNameInstance AddName(Func<string> nameGetter, Action<string> nameSetter, Type type,
            Func<bool> isAliveChecker = null,Object identifier = null)
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
    public partial class DataNameInstance : EventData.DataName.IDataNameInstance
    {
        public string DataName { get => nameGetter?.Invoke(); set => nameSetter?.Invoke(value); }

        public Type DataType => typeGetter?.Invoke();
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