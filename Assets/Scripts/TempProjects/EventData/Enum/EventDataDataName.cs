using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EventData
{
    //枚举 ：数据名
    public enum DataName
    {
        全局_输入_移动向量,
        全局_输入_跳跃值,
        全局_输入_冲刺值,

        运动速度向量,


        重力向量,
        地表法线,
        是否站在地面,
        地面物体,


        行走施力,
        跳跃施力,
        重力施力,
    }



    //预输入参数
    public static partial class DataNameD
    {
       
        //字典:类型判断正则表达
        public static Dictionary<System.Type, string> TypeRegexDic = new Dictionary<System.Type, string>(){
            {typeof(Vector2), @"(向量|施力|法线)$"},
            {typeof(bool), @"^是否"},
            {typeof(GameObject), @"物体$"},
            {typeof(float), @"值$"}
        };


    }





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
            bool success = DataNameD.TypeRegexDic.TryGetValue(type, out pattern);
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







        ///* <summary>获取数据名对应类型</summary>
        public static System.Type GetType(DataName dataName)
        {
            //返回值
            System.Type type = default;

            //历遍正则字典TypeRegexDic
            DataNameD.TypeRegexDic.ForEach((keyValue) =>
            {
                //创建一个正则表达式
                Regex r = new Regex(keyValue.Value);
                //匹配
                if (r.IsMatch(dataName.ToString()))
                {
                    //返回类型
                    type = keyValue.Key;
                }
            });



            return type;
        }













    }


}
