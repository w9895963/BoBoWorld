using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EventData.DataName
{
    public partial class DataNameInstance : EventData.DataName.IDataNameInstance
    {



        public string DataName { get => nameGetter?.Invoke(); set => nameSetter?.Invoke(value); }

        public Type DataType => typeGetter?.Invoke();
    }


    public partial class DataNameInstance
    {
        private static List<DataNameInstance> allNameInstance = new List<DataNameInstance>();


        private Func<string> nameGetter;
        private Action<string> nameSetter;
        private Func<Type> typeGetter;

        private Func<bool> isAliveChecker;


    }


    public partial class DataNameInstance
    {

        public static List<DataNameInstance> AllNameInstance
        {
            get
            {
                allNameInstance.RemoveAll(n => !n.isAliveChecker?.Invoke() ?? false);

                return allNameInstance;
            }
        }




        public static DataNameInstance AddName(BuildNameInfo nameInfo)
        {
            DataNameInstance name = new()
            {
                nameGetter = nameInfo.nameGetter,
                nameSetter = nameInfo.nameSetter,
                typeGetter = nameInfo.typeGetter,
                isAliveChecker = nameInfo.isAliveChecker,
            };
            allNameInstance.AddNotNull(name);
            return name;
        }
        public class BuildNameInfo
        {
            public Func<string> nameGetter;
            public Action<string> nameSetter;
            public Func<Type> typeGetter;

            public Func<bool> isAliveChecker;
        }



    }
}