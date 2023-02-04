using System;
using System.Collections.Generic;
using System.Linq;


namespace EventData.DataName
{
    public static class CustomDataName
    {
        public static List<IDataName> AllNames = new();


    }



    public interface IDataName
    {
        string DataName { get; set; }
        Type DataType { get; }
    }


}
