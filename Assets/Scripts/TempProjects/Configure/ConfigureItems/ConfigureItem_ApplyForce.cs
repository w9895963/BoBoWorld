using System;
using System.Collections.Generic;
using System.Linq;
using Configure;
using Configure.Inspector;
using EventData;
using UnityEditor;
using UnityEngine;



//命名空间：用来存放所有具体定义的配置项
namespace Configure.ConfigureItems
{




    [System.Serializable]
    public class ConfigureItem_ApplyForce : ConfigureItem
    {

        [Tooltip("将所选向量数据作为力应用到刚体上")]
        public List<DataNameDropDown<Vector2>> 施力数据列表;
        public List<string> forceNameList
        {
            get
            {
                return 施力数据列表.Select(x => x.dataName).ToList();
            }
            set
            {
                施力数据列表 = value.Select(x => new DataNameDropDown<Vector2>(x)).ToList();
            }
        }







        //脚本说明
        public Inspector.HelpText 说明 = new Inspector.HelpText("将所选向量数据作为力应用到刚体上");



        public void PresetDataContentOnCreate()
        {
            forceNameList = DataNameF.GetAllNamesOnType(typeof(Vector2)).Where(x => x.Contains("施力")).ToList();
        }



        public ConfigureItem_ApplyForce()
        {
            createRunnerFunc = GetCreateRunner;
            onAfterCreate += PresetDataContentOnCreate;
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
                forceDList = cf.forceNameList.Select(x => EventDataF.GetData<Vector2>(x, gameObject)).Where(x => x != null).ToList();




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

