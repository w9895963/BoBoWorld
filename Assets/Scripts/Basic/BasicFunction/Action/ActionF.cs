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




    /// <summary>安全执行操作，建议只在不得已的情况下考虑使用, 如界面事件， 依赖于新建物体和更新事件，效率不高</summary>
    public static void RunActionSafeAndDelay(Action action)
    {
        ActionUtility.SafeRunAction.RunAction(action);
    }



}