using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EventData
{
    //数据名方法
    public static class DataNameF
    {
        ///* <summary>获取所有可能的数据名列表,全名</summary>
        public static string[] GetDataNamesList()
        {

            //分割并获取最后一个
            IEnumerable<string> l1 = System.Enum.GetNames(typeof(DataName));


            //合并多个字符串数组
            string[] arr = l1.ToArray();

            return arr;
        }





        ///* <summary>获得某一类型的所有数据名</summary>
        public static string[] GetAllNamesOnTypeRegex(System.Type type)
        {
            string[] re = default;




            //~创建一个正则表达式⁡
            string pattern = null;
            //获得和类型对应的正则表达式
            bool success = DataNameD.TypeDict.TryGetValue(type, out pattern);
            if (!success)
            {
                Debug.LogError($"类型[{type}]没有预设对应的正则表达式");
                return re;
            }
            //创建一个正则表达式
            Regex regex = new Regex(pattern);



            //~多行匹配
            re = GetDataNamesList().Where(name => regex.IsMatch(name.Split("_").Last())).ToArray();

            return re;
        }







        ///* <summary>获取数据名对应类型,核心</summary>
        public static System.Type GetType(string dataName)
        {
            //返回值
            System.Type type = default;

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
        public static System.Type GetType(DataName dataName)
        {
            return GetType(dataName.ToString());
        }



        ///* <summary>判断一个数据名是否为全局参数</summary>
        public static bool IsGlobal(string dataName)
        {
            return dataName.StartsWith("全局_");
        }













    }


}
