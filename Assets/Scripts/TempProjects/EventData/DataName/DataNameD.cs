using System.Collections.Generic;
using System.Linq;
using EventData.DataName;

namespace EventData
{


    public static partial class DataNameD
    {
        public static IEnumerable<DataName.IDataNameInstance> AllNameInstance => EventData.DataName.DataNameInstance.AllNameInstance;

        public static IEnumerable<string> AllNamePreset => System.Enum.GetNames(typeof(DataName.Preset.PresetName));
        public static IEnumerable<DataName.IDataNameInfo> AllNamePresetInfo => DataNameD.AllNamePreset.SelectNotNull((name) => new DataNameF.NameInfoPreSet(name) as IDataNameInfo);


        public static IEnumerable<DataName.IDataNameInfo> AllDataNameInfo
        {
            get
            {
                IEnumerable<IDataNameInfo> re = new IDataNameInfo[0];
                re = AllNamePresetInfo;
                IEnumerable<IDataNameInfo> enumerable1 = DataNameInstance.AllNameInstance.Cast<IDataNameInstance>().Select((name) => name.ToDataNameInfo());
                re = re.Concat(enumerable1);

                return re;
            }
        }
    }


}
