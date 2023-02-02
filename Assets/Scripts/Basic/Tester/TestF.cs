using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class TestF
{
    ///<summary> 对比两组代码的事件花费 </summary>
    public static void CompareCodeTime(Func<(Action beforeRun, Action run, Action afterRun)> action1, Func<(Action beforeRun, Action run, Action afterRun)> action2, int repeat = 1000, int localRepeat = 1)
    {
        List<long> time1_t = new List<long>();
        List<long> time2_t = new List<long>();
        List<long> time1_ms = new List<long>();
        List<long> time2_ms = new List<long>();

        (Action beforeRun, Action run, Action afterRun) actionGroup1 = action1();
        (Action beforeRun, Action run, Action afterRun) actionGroup2 = action2();


        Stopwatch stopwatch = new Stopwatch();
        for (int ia = 0; ia < repeat; ia++)
        {
            stopwatch.Reset();
            for (int i = 0; i < localRepeat; i++)
            {
                actionGroup1.beforeRun?.Invoke();
                stopwatch.Start();
                actionGroup1.run?.Invoke();
                stopwatch.Stop();
                actionGroup1.afterRun?.Invoke();

            }
            time1_t.Add(stopwatch.ElapsedTicks);
            time1_ms.Add(stopwatch.ElapsedMilliseconds);


            stopwatch.Reset();
            for (int i = 0; i < localRepeat; i++)
            {
                actionGroup2.beforeRun?.Invoke();
                stopwatch.Start();
                actionGroup2.run?.Invoke();
                stopwatch.Stop();
                actionGroup2.afterRun?.Invoke();
            }

            time2_t.Add(stopwatch.ElapsedTicks);
            time2_ms.Add(stopwatch.ElapsedMilliseconds);
        }
        string str = "";



        //~对比两组代码的事件花费

        long totalTime1 = time1_t.Sum();
        long totalTime1_ms = time1_ms.Sum();
        long totalTime2 = time2_t.Sum();
        long totalTime2_ms = time2_ms.Sum();


        if (totalTime1 > totalTime2)
        {
            str += $"对比总耗时:方法1:{totalTime1}t,方法2:{totalTime2}t,方法2比方法1快{(float)(totalTime1 - totalTime2) / (float)totalTime1 * 100}%";
        }
        else
        {
            str += $"对比总耗时:方法1:{totalTime1}t,方法2:{totalTime2}t, 方法1比方法2快{(float)(totalTime2 - totalTime1) / (float)totalTime2 * 100}%";
        }

        //如果时间为0，说明方法执行时间太短，无法计算
        if (totalTime1_ms == 0 || totalTime2_ms == 0)
        {

        }
        else if (totalTime1_ms > totalTime2_ms)
        {
            str += Environment.NewLine;
            str += $"对比总耗时:方法1:{totalTime1_ms}ms,方法2:{totalTime2_ms}ms,方法2比方法1快{(float)(totalTime1_ms - totalTime2_ms) / (float)totalTime1_ms * 100}%";

        }
        else
        {
            str += Environment.NewLine;
            str += $"对比总耗时:方法1:{totalTime1_ms}ms,方法2:{totalTime2_ms}ms, 方法1比方法2快{(float)(totalTime2_ms - totalTime1_ms) / (float)totalTime2_ms * 100}%";

        }


        //~对比中位数
        time1_t.Sort();
        time2_t.Sort();
        time1_ms.Sort();
        time2_ms.Sort();
        long medianTime1 = time1_t[time1_t.Count / 2];
        long medianTime2 = time2_t[time2_t.Count / 2];
        long medianTime1_ms = time1_ms[time1_ms.Count / 2];
        long medianTime2_ms = time2_ms[time2_ms.Count / 2];


        if (medianTime1 > medianTime2)
        {
            str += Environment.NewLine;
            str += $"对比总时长:方法1:{medianTime1}t,方法2:{medianTime2}t,方法2比方法1快{(float)(medianTime1 - medianTime2) / (float)medianTime1 * 100}%";
        }
        else
        {
            str += Environment.NewLine;
            str += $"对比总时长:方法1:{medianTime1}t,方法2:{medianTime2}t, 方法1比方法2快{(float)(medianTime2 - medianTime1) / (float)medianTime2 * 100}%";
        }

        //如果时间为0，说明方法执行时间太短，无法计算
        if (medianTime1_ms == 0 || medianTime2_ms == 0)
        {

        }
        else if (medianTime1_ms > medianTime2_ms)
        {
            str += Environment.NewLine;
            str += $"对比中位数:方法1:{medianTime1_ms}ms,方法2:{medianTime2_ms}ms,方法2比方法1快{(float)(medianTime1_ms - medianTime2_ms) / (float)medianTime1_ms * 100}%";

        }
        else
        {
            str += Environment.NewLine;
            str += $"对比中位数:方法1:{medianTime1_ms}ms,方法2:{medianTime2_ms}ms, 方法1比方法2快{(float)(medianTime2_ms - medianTime1_ms) / (float)medianTime2_ms * 100}%";
        }


        Debug.Log(str);
    }

}
