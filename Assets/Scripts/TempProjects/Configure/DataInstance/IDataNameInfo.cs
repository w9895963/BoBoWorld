using System;
using System.Collections.Generic;

namespace Configure.DataInstance
{
    public interface IDataNameInfo
    {
        string DataName { get; }
        Type DataType { get; }
        /// <summary> 用"/"组合起来的分组名 </summary>
        string DataGroup { get; }
        /// <summary> 包含分组的名称, 适合做标题,格式为"分组/名" </summary>
        string DataNameWithGroup { get; }
        /// <summary> 所有同名的实例 </summary>
        IEnumerable<IDataNameInstance> DataNameInstances { get; }
        int InstanceCount { get; }





    }






}