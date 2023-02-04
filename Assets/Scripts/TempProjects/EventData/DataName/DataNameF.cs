using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Configure;
using EventData;
using EventData.CustomDataNameCore;
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

            //分割并获取最后一个
            IEnumerable<string> l1 = System.Enum.GetNames(typeof(DataNamePreset));

            //获得额外的数据名
            string[] l2 = GetAllCustomDataNamesAndTypesDict().Keys.ToArray();




            //合并多个字符串数组
            string[] arr = l1.Concat(l2).ToArray();



            return arr;
        }
        ///<summary>获取所有可能的数据名列表, 全名, 不去重,分组</summary>
        public static string[] GetDataNamesListWithGroup()
        {
            string[] arr = GetDataNamesList();
            arr = DataName_AddGroup(arr);

            return arr;
        }



        ///<summary>获取所有可能的数据名列表, 全名, 不去重</summary>
        public static string[] GetDataNamesList_PresetName()
        {
            return System.Enum.GetNames(typeof(DataNamePreset));
        }




        ///<summary>获得某一类型的所有数据名</summary>
        public static string[] GetAllNamesOnType(System.Type type)
        {
            if (type == null)
                return new string[0];

            List<string> re = new List<string>();
            //所有数据名




            //~创建一个正则表达式⁡,并匹配内置数据名
            if (DataNameD.TypeDict.TryGetValue(type, out string pattern))
            {
                //创建一个正则表达式
                Regex regex = new Regex(pattern);
                //匹配
                re.AddRange(GetDataNamesList_PresetName().Where(name => regex.IsMatch(GetNameFromFullName(name))));
            }

            //~匹配自定义数据名
            re.AddRange(GetAllCustomDataNamesAndTypesDict().Where(keyValue => keyValue.Value == type).Select(keyValue => keyValue.Key));


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
        public static System.Type GetType(string dataName)
        {
            //返回值
            System.Type type = null;


            System.Type type1 = GetCustomType(dataName);
            if (type1 != null)
            {
                return type1;
            }
            System.Type type2 = GetPresetType(dataName);
            if (type2 != null)
            {
                return type2;
            }



            return type;
        }
        /// <summary>获取数据名对应类型,核心</summary>
        public static System.Type GetType(DataNamePreset dataName)
        {
            return GetType(dataName.ToString());
        }
        ///<summary>获得一个数据的类型预设, 不对数据名进行预处理</summary>
        public static System.Type GetPresetType(string dataName)
        {
            System.Type type = null;

            //历遍正则字典TypeRegexDic
            DataNameD.TypeDict.ForEach((keyValue) =>
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

        ///<summary>获得一个数据的自定义类型预设, 不对数据名进行预处理</summary>
        public static System.Type GetCustomType(string dataName)
        {
            System.Type type = null;

            GetAllCustomDataNamesAndTypesDict().ForEach((keyValue) =>
            {
                if (keyValue.Key == dataName)
                {
                    type = keyValue.Value;
                }
            });



            return type;
        }





        ///<summary>获取现有数据类型表</summary>
        public static System.Type[] GetAllTypes()
        {
            return DataNameD.TypeDict.Keys.Distinct().ToArray();
        }



        ///<summary>判断一个数据名是否为全局参数</summary>
        public static bool IsGlobal(string dataName)
        {
            return dataName.StartsWith("全局_");
        }




        ///<summary> 获得所有自定义数据名和类型的字典, 不会重复 </summary>
        public static Dictionary<string, Type> GetAllCustomDataNamesAndTypesDict()
        {
            Dictionary<string, Type> re = new Dictionary<string, Type>();
            //获得所有数据名列表
            GameObject.FindObjectOfType<CustomDataNameLoaderMono>()?.数据列表.ForEach((list) =>
            {
                //获得所有数据名
                list.数据名列表.ForEach((data) =>
                {
                    // 如果数据名不为空, 且类型不为空
                    if (data.数据全名.IsNotEmpty() && data.type != null)
                        //添加数据名和类型
                        re.TryAdd(data.数据全名, data.type);
                });
            });

            return re;
        }





        ///<summary>新建数据名</summary>
        public static void CreateDataName(DataName.IDataName dataName)
        {
            DataName.CustomDataName.AllNames.Add(dataName);
        }


        ///<summary>删除数据名</summary>
        public static void RemoveDataName(DataName.IDataName dataName)
        {
            DataName.CustomDataName.AllNames.Remove(dataName);
        }

        ///<summary>重命名数据名</summary>
        public static void Rename(string currentName, string newName)
        {
            //~检查重名, 如果重名, 重命名失败
            var allNames = DataName.CustomDataName.AllNames;
            bool isRepeat = allNames.Any((data) => data.DataName == newName);
            if (isRepeat)
            {
                Debug.Log($"重命名失败, 尝试重命名 {currentName} 为已存在的数据名: {newName}");
                return;
            }

            allNames.Where((data) => data.DataName == currentName).ForEach((data) =>
            {
                data.DataName = newName;
            });


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
    }
}
