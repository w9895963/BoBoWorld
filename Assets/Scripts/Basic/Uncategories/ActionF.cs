using System;
using System.Collections.Generic;
using System.Linq;





public static class ActionF
{
    private static List<Action> QueueActionList = new List<Action>();

    public static void QueueAction(Action action)
    {
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