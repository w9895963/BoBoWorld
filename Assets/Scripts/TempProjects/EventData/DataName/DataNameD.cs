using System.Collections.Generic;
using System.Linq;
using EventData.DataName;

namespace EventData
{


    public static partial class DataNameD
    {
        public static IEnumerable<DataName.IDataNameInstance> AllNameInstance => EventData.DataName.DataNameInstance.AllNameInstance;

        private static IEnumerable<string> allNamePreset;
        public static IEnumerable<string> AllNamePreset => allNamePreset ?? (allNamePreset = System.Enum.GetNames(typeof(DataName.Preset.PresetName)));
        public static IEnumerable<DataName.IDataNameInfo> AllNameInfo_Preset => DataNameD.AllNamePreset.SelectNotNull((name) => new DataNameF.NameInfoPreSet(name) as IDataNameInfo);


        public static IEnumerable<DataName.IDataNameInfo> AllDataNameInfo
        {
            get
            {
                IEnumerable<IDataNameInfo> re = AllNameInfo_Preset;
                IEnumerable<IDataNameInfo> add;
                add = DataNameInstance.AllNameInstance.Cast<IDataNameInstance>().Select((name) => name.ToDataNameInfo());
                re = re.Concat(add);

                return re;
            }
        }
    }


}
