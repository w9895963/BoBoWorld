using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    namespace ConfigureItem
    {
        //配置:計算行走施力
        [CreateAssetMenu(fileName = "施力应用", menuName = "动态配置/施力应用", order = 1)]
        public partial class ConfigureItem_ApplyForce : ConfigureBase
        {

            public List<EventData.DataName> 施力数据列表 = new List<EventData.DataName>() {
                EventData.DataName.行走施力,
                EventData.DataName.跳跃施力};


            //脚本说明
            [Label("其他信息")]
            public ShowOnlyText info = new ShowOnlyText("将所选向量数据作为力应用到刚体上");

            //必要组件
            public override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D) };













            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);


                //获取施力数据列表
                List<EventDataHandler<Vector2>> forceDList = 施力数据列表.Select(x => EventDataF.GetData<Vector2>(x, gameObject)).Where(x => x != null).ToList();


                //获取物理组件
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();






                //启动器表
                List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>(forceDList.Count);




                //力列表
                List<Vector2> forceList = new List<Vector2>(){Vector2.zero};
               
                //递增历遍施力数据列表
                for (int i = 0; i < forceDList.Count; i++)
                {
                    //获取施力数据
                    EventDataHandler<Vector2> forceD = forceDList[i];

                    (Action Enable, Action Disable) value = EventDataF.OnDataCondition(() =>
                    {
                        //获取施力数据
                        forceList.AddToIndex(i, forceD.Data);
                    }, null, forceD.OnUpdate);

                    enablerList.Add(value);

                }





                enabler.Enable = () =>
                {
                    enablerList.ForEach(x => x.Enable());
                    BasicEvent.OnFixedUpdate.Add(gameObject, FixedUpdate);
                };
                enabler.Disable = () =>
                {
                    enablerList.ForEach(x => x.Disable());
                    BasicEvent.OnFixedUpdate.Remove(gameObject, FixedUpdate);
                };




                void FixedUpdate()
                {
                    Vector2 force = forceList.Aggregate((x, y) => x + y);
                    //等于0时不计算
                    if (force.magnitude == 0)
                    {
                        return;
                    }
                    rigidbody2D.AddForce(force);
                }




                return enabler;
            }


        }
    }


}

