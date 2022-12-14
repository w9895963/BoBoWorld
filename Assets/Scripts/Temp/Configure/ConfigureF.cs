using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDataS;
using System.Linq;

namespace ConfigureS
{
    //静态类:方法
    public static class ConfigureF
    {
        //方法:将执行事件添加到数据条件
        public static (Action Enable, Action Disable) OnDataCondition(GameObject gameObject,Action action, Action actionOnFail, List<ImportData> importDatas)
        {
            //创建启用器
            (Action Enable, Action Disable) enabler = (null, null);
            //获得导入参数内的所有事件数据
            List<EventDataHandler> eventDatas = importDatas.Select(x => x.GetEventDataHandler(gameObject)).ToList();



            return enabler;
        }
    }
}
