using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventData
{
    //枚举 ：数据名
    public enum DataName
    {
        输入指令_移动,
        输入指令_跳跃,
        输入指令_冲刺,

        运动速度,


        重力向量,
        地表法线,
        站在地面,
        地面物体,


        行走施力,
        跳跃施力,
        重力施力,
    }

    //数据名对应类型
    namespace DataNameAddition
    {
        //数据名对应类型
        public static class DataNameType
        {
            //类型字典
            public static Dictionary<DataName, System.Type> typeDic = new Dictionary<DataName, System.Type>(){
            {DataName.输入指令_移动, typeof(Vector2)},
            {DataName.输入指令_跳跃, typeof(bool)},
            {DataName.输入指令_冲刺, typeof(bool)},

            {DataName.运动速度, typeof(Vector2)},

            {DataName.重力向量, typeof(Vector2)},
            {DataName.地表法线, typeof(Vector2)},
            {DataName.站在地面, typeof(bool)},
            {DataName.地面物体, typeof(GameObject)},


            {DataName.行走施力, typeof(Vector2)},
            {DataName.跳跃施力, typeof(Vector2)},
            {DataName.重力施力, typeof(Vector2)},
            };




        }
    }






    //数据名方法
    public static class DataNameF
    {
        /// <summary>获取数据名对应类型</summary>
        public static System.Type GetType(DataName dataName)
        {
            Dictionary<DataName, System.Type> typeDic = DataNameAddition.DataNameType.typeDic;
            if (typeDic.ContainsKey(dataName))
            {
                return typeDic[dataName];
            }
            else
            {
                Debug.LogError($"数据名[{dataName}]没有预设对应的类型");
                return null;
            }
        }


        /// <summary>获得某一类型的所有数据名</summary>
        public static List<string> GetNamesOnType(System.Type type)
        {
            List<string> list = new List<string>();
            foreach (DataName dataName in System.Enum.GetValues(typeof(DataName)))
            {
                if (GetType(dataName) == type)
                {
                    list.Add(dataName.ToString());
                }
            }
            if (list.Count == 0)
            {
                list.Add("");
            }
            return list;
        }
    }
}
