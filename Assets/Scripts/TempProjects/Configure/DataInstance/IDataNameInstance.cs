using System;
using System.Collections.Generic;
using System.Linq;


namespace Configure.DataInstance
{
    using static InnerF;

    public interface IDataNameInstance
    {
        string DataName { get; set; }
        Type DataType { get; }






        IDataNameInfo ToDataNameInfo()
        {
            return new NameInfoInstance(this);
        }


        public struct NameInfoInstance : IDataNameInfo
        {
            private IDataNameInstance instance;
            public string DataName => instance.DataName;

            public Type DataType => instance.DataType;
            public string DataGroup => GetGroup(instance.DataName);
            public string DataNameWithGroup => String.Join('/', DataGroup, DataName);
            public IEnumerable<IDataNameInstance> DataNameInstances => GetDataNameInstances();
            public int InstanceCount => DataNameInstances.Count();

            public NameInfoInstance(DataNameInstance nameInstance)
            {
                instance = nameInstance;
            }
            public NameInfoInstance(IDataNameInstance nameInstance)
            {
                instance = nameInstance;
            }


            private IEnumerable<IDataNameInstance> GetDataNameInstances()
            {
                foreach (var item in DataNameInstance.AllNameInstance)
                {
                    if (item.DataName == instance.DataName)
                    {
                        yield return item;
                    }
                }
            }






        }
    }






}