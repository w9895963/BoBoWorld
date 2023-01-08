using System;
using EventData.Core;
using UnityEngine;

namespace EventData
{
    //*公用方法需要的类:EventDataHandler
    public class EventDataHandler
    {

        private Core.EventData eventData;

        public EventDataHandler(Core.EventData eventData)
        {
            this.eventData = eventData;
        }

        //属性：数据
        public System.Object Data
        {
            get => eventData.GetData();
        }





        //属性：获得数据判断方法，数据更新
        public (Core.EventData data, Func<bool> check) OnUpdateCondition => (eventData, null);

        //属性：获得数据判断方法，数据为真
        public (Core.EventData data, Func<bool> check) OnTrueCondition => (eventData, () => { return Data.Equals(true); }
        );
        //属性：获得数据判断方法，数据为假
        public (Core.EventData data, Func<bool> check) OnFalseCondition => (eventData, () => { return Data.Equals(false); }
        );
        //方法：获得数据判断方法，自定义判断
        public (Core.EventData data, Func<bool> check) OnCustomCondition(Func<bool> check)
        {
            return (eventData, check);
        }

    }

    //类型:数据操作器
    public class EventDataHandler<T> : EventDataHandler
    {
        //字段：事件数据
        private EventData<T> eventDataT;

        public EventDataHandler(EventData<T> eventDataT) : base(eventDataT)
        {
            this.eventDataT = eventDataT;

        }



        //属性：数据
        public new T Data
        {
            get => eventDataT.GetData();
            set => eventDataT.SetIfNotEqual(value);
        }





        public void BindDataTo(Action<T> setAction, MonoBehaviour enableWithBehaviour = null)
        {
            setAction?.Invoke(eventDataT.GetData());
            (Core.EventData data, Func<bool> check)[] conditions = { (eventDataT, null) };
            EventDataF.CreateConditionEnabler(() => setAction?.Invoke(eventDataT.GetData()), null, conditions, enableWithBehaviour).Enable();
        }





    }

}