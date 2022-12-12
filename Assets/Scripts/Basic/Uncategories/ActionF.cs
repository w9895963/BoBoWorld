using System;
using System.Collections.Generic;
using System.Linq;





public static class ActionF
{
    private static List<Action> QueueActionList = new List<Action>();

    /// <summary>按照顺序执行,可处理空参数</summary>
    public static void QueueAction(Action action)
    {
        //如果空则退出
        if (action == null) return;
        
        QueueActionList.Add(action);
        if (QueueActionList.Count() == 1)
        {
            while (QueueActionList.Count() > 0)
            {
                Action aa = QueueActionList[0];
                QueueActionList.RemoveAt(0);
                aa?.Invoke();
            }
        }
    }



}