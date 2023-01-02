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

        标签,



        Unity事件_碰撞开始,
        Unity事件_碰撞结束,
        Unity事件_碰撞持续,



        行走方向,
        重力向量,
        地表法线,
        行走施力,
        站在地面,
    }

}
