using System.Collections.Generic;
using System;
using System.Linq;

public static class OrderRun
{
    private static List<Action> list = new List<Action>();

    public static void Run(Action action)
    {

        list.Add(action);
        if (list.Count() == 1)
        {
            while (list.Count() > 0)
            {
                Action aa = list[0];
                list.RemoveAt(0);
                aa?.Invoke();
            }
        }


    }


}