using System;
using EventDataS.EventDataCore;

namespace EventDataS
{
    //*公用方法需要的类:EventDataHandler
    public class EventDataHandler
    {

        public EventData eventData;

        //属性：数据
        public System.Object Data
        {
            get => eventData.GetData();
        }




        //属性：获得数据判断方法，数据更新
        public (EventDataCore.EventData data, Func<bool> check) OnUpdate => (eventData, null);

        //属性：获得数据判断方法，数据为真
        public (EventDataCore.EventData data, Func<bool> check) OnTrue => (eventData, () => { return Data.Equals(true); }
        );
        //属性：获得数据判断方法，数据为假
        public (EventDataCore.EventData data, Func<bool> check) OnFalse => (eventData, () => { return Data.Equals(false); }
        );
        //方法：获得数据判断方法，自定义判断
        public (EventDataCore.EventData data, Func<bool> check) OnCustom(Func<bool> check)
        {
            return (eventData, check);
        }

    }

    //类型:数据操作器
    public class EventDataHandler<T> : EventDataHandler
    {
        //字段：事件数据
        private EventData<T> eventDataT;

        public EventDataHandler(EventData<T> eventDataT)
        {
            this.eventDataT = eventDataT;
            this.eventData = eventDataT;
        }



        //属性：数据
        public new T Data
        {
            get => eventDataT.GetData();
            set => eventDataT.SetData(value);
        }

        //方法：同步数据
        public (Action Enable, Action Disable) SetDataTo(Action<T> act)
        {
            ConditionAction conditionAction = new ConditionAction();
            conditionAction.action = () => { act(Data); };
            Action enable = () => eventDataT.conditionActionList.Add(conditionAction);
            Action disable = () => { eventDataT.conditionActionList.Remove(conditionAction); };

            return (enable, disable);
        }
        public void SetDataTo(Action<T> act, ref (Action Enable, Action Disable) enabler)
        {
            (Action Enable, Action Disable) enableAction = SetDataTo(act);
            enabler.Enable += enableAction.Enable;
            enabler.Disable += enableAction.Disable;
        }







    }

}