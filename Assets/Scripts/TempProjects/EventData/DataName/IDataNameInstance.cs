using System;
using System.Collections.Generic;

namespace EventData.DataName
{









    public interface IDataNameInstance
    {
        string DataName { get; set; }
        Type DataType { get; }




        IEnumerable<DataName.IDataNameInstance> DataNameInstances => ToDataNameInfo().DataNameInstances;


        public IDataNameInfo ToDataNameInfo()
        {
            return new DataNameF.NameInfoInstance(this);
        }


         
    }


}
