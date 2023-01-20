using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Interface;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure.ConfigureItem
{




    [System.Serializable]
    public class ConfigureItem_ApplyForce : ConfigureItemBase
    {

        [Tooltip("将所选向量数据作为力应用到刚体上")]
        public List<DataHolder_NameDropDown<Vector2>> 施力数据列表 = new List<DataHolder_NameDropDown<Vector2>>(){
                new  (EventData.DataName.行走施力),
                new  (EventData.DataName.跳跃施力),
                new  (EventData.DataName.重力施力),
            };







        //脚本说明
        public Interface.ShowOnlyText 说明 = new Interface.ShowOnlyText("将所选向量数据作为力应用到刚体上");







        public ConfigureItem_ApplyForce()
        {
            createRunner = GetCreateRunner;
        }

        private ConfigureRunner GetCreateRunner(GameObject gameObject)
        {
            Runner r = new Runner(gameObject, this);
            return new ConfigureRunner(r.initialize, r.enable, r.disable, r.destroy);
        }



        private class Runner
        {
            private GameObject gameObject;
            private Rigidbody2D rigidbody2D;
            private ConfigureItem_ApplyForce cf;

            public Runner(GameObject gameObject, ConfigureItem_ApplyForce cf)
            {
                this.gameObject = gameObject;
                rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                this.cf = cf;
            }
            private List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>();
            private List<EventDataHandler<Vector2>> forceDList;
            private List<Vector2> forceList = new List<Vector2>() { Vector2.zero };

            public void initialize()
            {

                //获取施力数据列表
                forceDList = cf.施力数据列表.Select(x => EventDataF.GetData<Vector2>(x.dataName, gameObject)).Where(x => x != null).ToList();




                forceDList.ForEach((forceD, i) =>
                {

                    (Action Enable, Action Disable) value = EventDataF.CreateConditionEnabler(() =>
                    {
                        // Debug.Log("施力数据更新");
                        //获取施力数据

                        forceList.AddToIndex(i, forceD.Data);
                    }, null, forceD.OnUpdateCondition);

                    enablerList.Add(value);

                    enablerList.ForEach(x => x.Enable());
                });




            }

            public void destroy()
            {
                enablerList.ForEach(x => x.Disable());
            }

            public void enable()
            {

                BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);

            }

            public void disable()
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

