using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventData.PresetNameF;



namespace Configure.DataInstance
{
    using static InnerF;
    public static class CoreF
    {
        public static IEnumerable<IDataNameInstance> AllNameInstance => DataNameInstance.AllNameInstance;

        private static IEnumerable<string> allNamePreset;
        public static IEnumerable<string> AllNamePreset => allNamePreset ?? (allNamePreset = System.Enum.GetNames(typeof(EventData.DataName.Preset.PresetName)));
        public static IEnumerable<IDataNameInfo> AllNameInfo_Preset => AllNamePreset.SelectNotNull((name) => new NameInfoPreSet(name) as IDataNameInfo);


        public static IEnumerable<IDataNameInfo> AllDataNameInfo
        {
            get
            {
                IEnumerable<IDataNameInfo> re = AllNameInfo_Preset;
                IEnumerable<IDataNameInfo> add;
                add = DataNameInstance.AllNameInstance.SelectNotNull((name) => (name as IDataNameInstance)?.ToDataNameInfo());
                re = re.Concat(add);

                return re;
            }
        }














    }
}