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
            public override List<Type> requiredTypes => new List<Type>() { typeof(ConstantForce2D), typeof(Rigidbody2D) };













            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);


                //获取施力数据列表
                List<EventDataHandler<Vector2>> forceList = 施力数据列表.Select(x => EventDataF.GetData<Vector2>(x, gameObject)).ToList();


                //获取施力组件
                ConstantForce2D constantForce2D = gameObject.GetComponent<ConstantForce2D>();

                enabler = EventDataF.OnDataCondition(CalculateMoveForce, OnFail, forceList.Select(x => x.OnUpdate).ToArray());





                //计算移动施力
                void CalculateMoveForce()
                {
                    //将施力数据相加
                    Vector2 force = forceList.Select(x => x.Data).Aggregate((x, y) => x + y);
                    constantForce2D.force = force;

                }

                void OnFail()
                {
                    constantForce2D.force = Vector2.zero;
                }




                return enabler;
            }


        }
    }


}

