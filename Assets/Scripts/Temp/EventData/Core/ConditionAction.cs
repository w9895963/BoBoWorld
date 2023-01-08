using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventData
{
    namespace Core
    {
        //条件操作
        public class ConditionAction
        {
            public System.Action action;
            public System.Action actionOnFail;
            public List<Func<bool>> conditionList = new List<Func<bool>>();

            //条件执行队列
            public static List<System.Action> ConditionActionQueue = new List<System.Action>();


            ///<summary> 方法: 检测并运行,基础执行版本 </summary>
            public void CheckAndRun()
            {
                var isConditionMet = conditionList.All(condition => condition());

                //基础执行
                if (isConditionMet)
                {
                    action();
                }
                else
                {
                    actionOnFail();
                }



            }


            // ///<summary> 方法: 检测并运行,顺序执行版本 </summary>
            // public void CheckAndRunQueue()
            // {
            //     var isConditionMet = conditionList.All(condition => condition());

            //     //顺序执行
            //     if (isConditionMet)
            //     {
            //         ActionF.QueueAction(action);
            //     }
            //     else
            //     {

            //         ActionF.QueueAction(actionOnFail);
            //     }
            // }

        }



    }
}
