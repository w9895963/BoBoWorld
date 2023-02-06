using System;
using System.Collections.Generic;


namespace EventData.DataName
{
    public interface IDataNameInfo
    {
        string DataName { get; }
        Type DataType { get; }
        string DataGroup { get; }
        string DataNameWithGroup { get; }
        IEnumerable<DataName.IDataNameInstance> DataNameInstances { get; }
        int InstanceCount { get; }





    }


}