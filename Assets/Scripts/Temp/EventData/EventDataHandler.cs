using System;
using EventDataS.Core;

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
        public (Core.EventData data, Func<bool> check) OnUpdate => (eventData, null);

        //属性：获得数据判断方法，数据为真
        public (Core.EventData data, Func<bool> check) OnTrue => (eventData, () => { return Data.Equals(true); }
        );
        //属性：获得数据判断方法，数据为假
        public (Core.EventData data, Func<bool> check) OnFalse => (eventData, () => { return Data.Equals(false); }
        );
        //方法：获得数据判断方法，自定义判断
        public (Core.EventData data, Func<bool> check) OnCustom(Func<bool> check)
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

  
  







    }

}