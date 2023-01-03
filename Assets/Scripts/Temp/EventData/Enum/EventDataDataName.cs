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


        重力向量,
        地表法线,
        站在地面,


        行走施力,
        跳跃施力,
    }






    namespace DataNameAddition
    {
        //数据名对应类型
        public static class DataNameType
        {
            //类型字典
            private static Dictionary<DataName, System.Type> typeDic = new Dictionary<DataName, System.Type>(){
            {DataName.输入指令_移动, typeof(Vector2)},
            {DataName.输入指令_跳跃, typeof(bool)},
            {DataName.输入指令_冲刺, typeof(bool)},

            {DataName.重力向量, typeof(Vector2)},
            {DataName.地表法线, typeof(Vector2)},
            {DataName.站在地面, typeof(bool)},

            
            {DataName.行走施力, typeof(Vector2)},
            {DataName.跳跃施力, typeof(Vector2)},
        };


            public static System.Type Get(DataName dataName)
            {
                if (typeDic.ContainsKey(dataName))
                {
                    return typeDic[dataName];
                }
                else
                {
                    Debug.LogError($"数据名[{dataName}]没有对应的类型");
                    return null;
                }
            }
        }
    }

}
