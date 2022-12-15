using System;
using System.Collections.Generic;
using System.Linq;

namespace EventDataS
{
    namespace EventDataCore
    {
        //条件操作
        public class ConditionAction
        {
            public System.Action action;
            public System.Action actionOnFail;
            public List<Func<bool>> conditionList = new List<Func<bool>>();


            //方法: 检测并运行
            public void CheckAndRun()
            {
                var isConditionMet = conditionList.All(condition => condition());



                if (isConditionMet)
                {
                    ActionF.QueueAction(action);
                }
                else
                {

                    ActionF.QueueAction(actionOnFail);
                }

            }

        }



    }
}
