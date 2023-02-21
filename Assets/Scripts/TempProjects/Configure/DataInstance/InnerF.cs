using System;
using System.Collections.Generic;
using System.Linq;
using static EventData.PresetNameF;


namespace Configure.DataInstance
{
    using static InnerF;
    using static CoreF;
    public static class InnerF
    {
        ///<summary>从数据名获得分组字符串</summary>
        public static string GetGroup(string dataName)
        {
            string group = null;
            string name = dataName;
            var strings = name.Split("_").ToList();
            if (strings.Count > 1)
            {
                strings.RemoveLast();
                if (strings[0] == "全局")
                    strings.RemoveAt(0);


                if (strings.Count > 1)
                    group = string.Join("/", strings);
                else
                    group = strings[0];
            }

            return group;

        }




        public struct NameInfoPreSet : IDataNameInfo
        {
            public string dataName;
            public string DataName => dataName;

            public Type DataType => GetDataType(dataName);
            public string DataGroup => GetGroup(dataName);
            public string DataNameWithGroup => String.Join('/', DataGroup, dataName);
            public IEnumerable<IDataNameInstance> DataNameInstances => GetDataNameInstances();
            public int InstanceCount => DataNameInstances.Count();

            public NameInfoPreSet(string dataName)
            {
                this.dataName = dataName;
            }

            private IEnumerable<IDataNameInstance> GetDataNameInstances()
            {
                foreach (var item in AllNameInstance)
                {
                    if (item.DataName == dataName)
                    {
                        yield return item;
                    }
                }
            }


        }
    }
}