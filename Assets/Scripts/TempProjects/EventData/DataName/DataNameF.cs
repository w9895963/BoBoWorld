using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Configure;
using EventData;
using EventData.DataName;
using UnityEngine;

namespace EventData
{
    public static partial class DataNameF
    {
        ///<summary>从完整数据名中获得单纯数据名</summary>

        public static string GetNameFromFullName(string fullDataName)
        {
            return fullDataName.Split("_").Last();
        }

        ///<summary>获取所有可能的数据名列表,全名, 不去重</summary>
        public static string[] GetDataNamesList()
        {
            IEnumerable<string> re = new string[0];
            //分割并获取最后一个
            IEnumerable<string> l1 = System.Enum.GetNames(typeof(DataName.Preset.PresetName));





            //合并多个字符串数组
            re = re.Concat(l1);



            return re.ToArray();
        }
        ///<summary>获取数据名列表, 预设, 全名, 不去重</summary>
        public static string[] GetDataNamesList_PresetName()
        {
            return System.Enum.GetNames(typeof(DataName.Preset.PresetName));
        }






        ///<summary>获取所有可能的数据名列表, 全名, 不去重,分组</summary>
        public static string[] GetDataNamesListWithGroup()
        {
            string[] arr = GetDataNamesList();
            arr = DataName_AddGroup(arr);

            return arr;
        }








        ///<summary>获得某一类型的所有数据名</summary>
        public static string[] GetAllNamesOnType(System.Type type)
        {
            if (type == null)
                return new string[0];

            List<string> re = new List<string>();
            //所有数据名




            //~创建一个正则表达式⁡,并匹配内置数据名
            if (DataName.Preset.TypeDict.TryGetValue(type, out string pattern))
            {
                //创建一个正则表达式
                Regex regex = new Regex(pattern);
                //匹配
                re.AddRange(GetDataNamesList_PresetName().Where(name => regex.IsMatch(GetNameFromFullName(name))));
            }




            return re.ToArray();
        }
        ///<summary>获得某一类型的所有数据名</summary>
        public static string[] GetAllNamesOnTypeWithGroup(System.Type type)
        {
            string[] arr = GetAllNamesOnType(type);
            arr = DataName_AddGroup(arr);


            return arr;
        }







        ///<summary>获取数据名对应类型,核心</summary>
        public static System.Type GetDataType(string dataName)
        {
            //返回值
            System.Type type = null;


            type = GetPresetType(dataName) ?? type;



            return type;
        }

        ///<summary>获得一个数据的类型预设, 不对数据名进行预处理</summary>
        public static System.Type GetPresetType(string dataName)
        {
            System.Type type = null;

            //历遍正则字典TypeRegexDic
            DataName.Preset.TypeDict.ForEach((keyValue) =>
             {
                 //创建一个正则表达式
                 Regex r = new Regex(keyValue.Value);
                 //匹配
                 if (r.IsMatch(dataName))
                 {
                     //返回类型
                     type = keyValue.Key;
                 }
             });

            return type;
        }

      





        ///<summary>获取现有数据类型表</summary>
        public static System.Type[] GetAllTypes()
        {
            return DataName.Preset.TypeDict.Keys.Distinct().ToArray();
        }



        ///<summary>判断一个数据名是否为全局参数</summary>
        public static bool IsGlobal(string dataName)
        {
            return dataName.StartsWith("全局_");
        }










    }





    ///<summary>类定义</summary>
    public static partial class DataNameF
    {




        public struct NameInfoPreSet : IDataNameInfo
        {
            public string dataName;
            public string DataName => dataName;

            public Type DataType => GetDataType(dataName);
            public string DataGroup => GetGroup(dataName);
            public string DataNameWithGroup => String.Join('/', DataGroup, dataName);
            public IEnumerable<DataName.IDataNameInstance> DataNameInstances => GetDataNameInstances();
            public int InstanceCount => DataNameInstances.Count();

            public NameInfoPreSet(string dataName)
            {
                this.dataName = dataName;
            }

            private IEnumerable<DataName.IDataNameInstance> GetDataNameInstances()
            {
                foreach (var item in DataNameD.AllNameInstance)
                {
                    if (item.DataName == dataName)
                    {
                        yield return item;
                    }
                }
            }


        }


        public struct NameInfoInstance : IDataNameInfo
        {
            private IDataNameInstance instance;
            public string DataName => instance.DataName;

            public Type DataType => instance.DataType;
            public string DataGroup => GetGroup(instance.DataName);
            public string DataNameWithGroup => String.Join('/', DataGroup, DataName);
            public IEnumerable<DataName.IDataNameInstance> DataNameInstances => GetDataNameInstances();
            public int InstanceCount => DataNameInstances.Count();

            public NameInfoInstance(DataName.DataNameInstance nameInstance)
            {
                instance = nameInstance;
            }
            public NameInfoInstance(IDataNameInstance nameInstance)
            {
                instance = nameInstance;
            }


            private IEnumerable<DataName.IDataNameInstance> GetDataNameInstances()
            {
                foreach (var item in EventData.DataName.DataNameInstance.AllNameInstance)
                {
                    if (item.DataName == instance.DataName)
                    {
                        yield return item;
                    }
                }
            }






        }




    }








    ///<summary>内部方法</summary>
    public static partial class DataNameF
    {
        ///<summary>将数据名列表加上分组</summary>
        private static string[] DataName_AddGroup(string[] arr)
        {
            arr = arr.Select(name =>
            {
                string group = null;
                string fullName = name;
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

                if (group != null)
                    fullName = fullName.Insert(0, group + "/");
                return fullName;
            }).ToArray();
            return arr;
        }




        ///<summary>从数据名获得分组字符串</summary>
        private static string GetGroup(string dataName)
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





    }

}


