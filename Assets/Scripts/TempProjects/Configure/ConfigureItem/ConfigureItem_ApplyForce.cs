using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Interface;
using EventData;
using NaughtyAttributes;
using StackableDecorator;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {




        [System.Serializable]
        [AddTypeMenu("物理/力量施加器")]
        public class ConfigureItem_ApplyForce_ : ConfigureBase_
        {

            [HorizontalGroup("施力数据列表", true, "", 0)]
            [StackableField]
            public List<DataHolder_NameDropDown<Vector2>> 施力数据列表 = new List<DataHolder_NameDropDown<Vector2>>(){
                new  (EventData.DataName.行走施力),
                new  (EventData.DataName.跳跃施力),
                new  (EventData.DataName.重力施力),
            };







            //脚本说明
            [NaughtyAttributes.Label("说明")]
            public ShowOnlyText info = new ShowOnlyText("将所选向量数据作为力应用到刚体上");



            public ConfigureItem_ApplyForce_()
            {
                Construct();
            }











            private GameObject gameObject;
            private Rigidbody2D rigidbody2D;
            private List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>();
            private List<EventDataHandler<Vector2>> forceDList;
            private List<Vector2> forceList = new List<Vector2>() { Vector2.zero };

            private void Construct()
            {
                createRunner = (obj) =>
                {
                    gameObject = obj;
                    rigidbody2D = gameObject.GetComponent<Rigidbody2D>();


                    return new ConfigureRunner(initialize, enable, disable, destroy);
                };
            }



            private void initialize()
            {

                //获取施力数据列表
                forceDList = 施力数据列表.Select(x => EventDataF.GetData<Vector2>(x.dataName, gameObject)).Where(x => x != null).ToList();





                //递增历遍施力数据列表
                for (int i = 0; i < forceDList.Count; i++)
                {
                    //获取施力数据
                    EventDataHandler<Vector2> forceD = forceDList[i];

                    (Action Enable, Action Disable) value = EventDataF.CreateConditionEnabler(() =>
                    {
                        // Debug.Log("施力数据更新");
                        //获取施力数据
                        forceList.AddToIndex(i, forceD.Data);
                    }, null, forceD.OnUpdateCondition);

                    enablerList.Add(value);
                    
                    enablerList.ForEach(x => x.Enable());
                }



            }

            private void destroy()
            {
                enablerList.ForEach(x => x.Disable());
            }

            private void enable()
            {

                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);

            }

            private void disable()
            {

                BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdate);
            }






            private void FixedUpdate()
            {
                Vector2 force = forceList.Aggregate((x, y) => x + y);
                //等于0时不计算
                if (force.magnitude == 0)
                {
                    return;
                }
                rigidbody2D.AddForce(force);
            }
        }

    }


}

