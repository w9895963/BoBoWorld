using System.Collections.Generic;
using System.Linq;

namespace EventData
{
    namespace Core
    {
        ///<summary> 静态类:数据事件分离执行队列 </summary>
        public static class SeparatedExecutionQueue
        {
            //数据修改操作队列
            private static List<System.Action> DataModifyQueue = new List<System.Action>();
            //事件执行操作队列
            private static List<ConditionAction> EventExecuteQueue = new List<ConditionAction>();

            //方法: 添加到数据修改操作
            public static void AddDataAction(System.Action dataModifyAction)
            {
                DataModifyQueue.Add(dataModifyAction);
                //如果只有一个数据修改操作则运行
                StartExecuteIfPossible();

            }
            //方法: 添加到事件执行操作
            public static void AddCondition(ConditionAction conditionAction)
            {
                EventExecuteQueue.Add(conditionAction);
                StartExecuteIfPossible();
            }

            //方法: 开始执行
            private static void StartExecuteIfPossible()
            {

                if (DataModifyQueue.Count + EventExecuteQueue.Count == 1)
                {
                    //轮流执行两个队列直到两者都为空
                    while (DataModifyQueue.Count != 0 || EventExecuteQueue.Count != 0)
                    {
                        //如果数据修改队列不为空则运行数据修改队列
                        if (DataModifyQueue.Count != 0)
                        {
                            DataModifyQueue.ForEach(action => action());
                            DataModifyQueue.Clear();
                        }
                        //如果事件执行队列不为空则运行事件执行队列
                        if (EventExecuteQueue.Count != 0)
                        {
                            //反向去重
                            EventExecuteQueue.Distinct().ForEach(conditionAction => conditionAction.CheckAndRun());
                            EventExecuteQueue.Clear();
                        }
                    }
                }

            }

        }



    }
}
