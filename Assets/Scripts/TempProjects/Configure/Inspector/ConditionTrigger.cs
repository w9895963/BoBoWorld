using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;





namespace Configure.Inspector
{

    [Serializable]
    [InlineProperty]
    [HideLabel]
    public class ConditionTriggerList
    {
        [SerializeField]
        [ListDrawerSettings(Expanded = false)]
        [LabelText("$"+nameof(labelName))]
        private List<ConditionTrigger> conditionList = new List<ConditionTrigger>();

        [HideInInspector]
        public string labelName = "触发条件列表";



        public (EventData.Core.EventData, Func<bool>)[] GetConditions(GameObject gameObject)
        {
            return conditionList.Select(c => c.GetCondition(gameObject)).ToArray();
        }
    }



    [Serializable]
    public partial class ConditionTrigger
    {
        [SerializeField]
        [ValueDropdown(nameof(GetDataList))]
        [OnValueChanged(nameof(OnDataNameChanged))]
        [LabelText("数据名")]
        private string dataNameWithGroup;
        private string dataName => dataNameWithGroup.Split('/').Last();



        [SerializeField]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [HideLabel]
        [HideIf(nameof(hideConditionBase))]
        private ConditionCore conditionBase;
        private bool hideConditionBase => conditionBase == null;

        private void OnDataNameChanged()
        {
            Type type = typeof(ConditionTrigger).GetNestedTypes()
            .Where(t => t.BaseType == typeof(ConditionCore))
            // .Log()//Test
            .FirstOrDefault(t => t.GetProperty(nameof(ConditionCore.dataType)).GetValue(Activator.CreateInstance(t)) as Type == EventData.DataNameF.GetType(dataName));

            if (type == null)
                conditionBase = new ConditionOnUpdate();
            else
                conditionBase = (ConditionCore)Activator.CreateInstance(type);
        }

        private string[] GetDataList()
        {
            return EventData.DataNameF.GetDataNamesListWithGroup();
        }



        public (EventData.Core.EventData, Func<bool>) GetCondition(GameObject gameObject)
        {
            return conditionBase.GetCondition(gameObject, dataName);
        }







    }





    ///<summary> 细分类型 </summary>
    public partial class ConditionTrigger
    {
        [Serializable]
        public abstract class ConditionCore
        {
            public abstract Type dataType { get; }
            public abstract (EventData.Core.EventData, Func<bool>) GetCondition(GameObject gameObject, string dataName);
        }

        [Serializable]
        public class ConditionBool : ConditionCore
        {
            public override Type dataType => typeof(bool);
            public UpdateType 条件;


            public enum UpdateType
            {
                数据更新,
                为真,
                为假,


            }

            public override (EventData.Core.EventData, Func<bool>) GetCondition(GameObject gameObject, string dataName)
            {
                (EventData.Core.EventData, Func<bool>) re = default;
                EventData.EventDataHandler eventDataHandler = EventData.EventDataF.GetData(dataName, gameObject);
                if (条件 == UpdateType.数据更新)
                    re = eventDataHandler.OnUpdateCondition;
                else if (条件 == UpdateType.为真)
                    re = eventDataHandler.OnTrueCondition;
                else if (条件 == UpdateType.为假)
                    re = eventDataHandler.OnFalseCondition;

                return re;
            }
        }

        [Serializable]
        public class ConditionOnUpdate : ConditionCore
        {
            public override Type dataType => null;
            public UpdateType 条件;


            public enum UpdateType
            {
                数据更新,
            }

            public override (EventData.Core.EventData, Func<bool>) GetCondition(GameObject gameObject, string dataName)
            {
                (EventData.Core.EventData, Func<bool>) re = default;
                EventData.EventDataHandler eventDataHandler = EventData.EventDataF.GetData(dataName, gameObject);
                if (条件 == UpdateType.数据更新)
                    re = eventDataHandler.OnUpdateCondition;

                return re;
            }
        }
    }
}