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
        [CreateAssetMenu(fileName = "地表检测", menuName = "动态配置/地表检测", order = 1)]
        public partial class ConfigureItem_GroundFinder : ConfigureBase
        {
            [Header("固定参数")]
            //地表最大角度
            [Tooltip("大于此角度则不视为地面")]
            public float 地表最大角度 = 10;

            [Tooltip("此标签外的物体不被视为地面")]
            [Label("地表碰撞体标签")]
            [Tag]
            public List<string> collisionTags = new List<string>();

            //脚本说明
            [Label("说明")]
            public ShowOnlyText 脚本说明_ = new ShowOnlyText("输入: 无", "输出: 地面法线, 站立地面");

            //必要组件
            public override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };










            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);

                //碰撞标签

                List<(GameObject, ContactPoint2D[])> groundObjects = new List<(GameObject, ContactPoint2D[])>();



                //重力方向
                var gravityD = EventDataF.GetData<Vector2>(DataName.重力向量, gameObject);
                //地面法线
                var groundNormalD = EventDataF.GetData<Vector2>(DataName.地表法线, gameObject);
                //站立地面
                var standGroundD = EventDataF.GetData<bool>(DataName.站在地面, gameObject);



                enabler.Enable = () =>
                {
                    groundObjects = new List<(GameObject, ContactPoint2D[])>();

                    BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Add(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2D);

                };

                enabler.Disable = () =>
                {
                    BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Remove(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2D);
                };




                void OnCollisionEnter2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!collisionTags.Contains(obj.gameObject.tag))
                        return;
                    groundObjects.Add((obj.gameObject, obj.contacts));
                    CalcGroundNormal(obj.contacts);

                }
                void OnCollisionStay2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!collisionTags.Contains(obj.gameObject.tag))
                        return;
                    int v = groundObjects.FindIndex(x => x.Item1 == obj.gameObject);
                    if (v != -1)
                    {
                        groundObjects[v] = (obj.gameObject, obj.contacts);
                    }

                    CalcGroundNormal(obj.contacts);
                }

                void OnCollisionExit2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!collisionTags.Contains(obj.gameObject.tag))
                        return;
                    groundObjects.RemoveAll(x => x.Item1 == obj.gameObject);
                    ContactPoint2D[] contactPoint2Ds = groundObjects.SelectMany(x => x.Item2).ToArray();
                    CalcGroundNormal(contactPoint2Ds);

                }





                Vector2 CalcGroundNormal(ContactPoint2D[] contacts)
                {
                    Vector2 groundNormal = Vector2.zero;
                    Vector2 gravity = gravityD.Data;
                    //如果重力为0
                    if (gravity == Vector2.zero)
                        gravity = Vector2.down;

                    foreach (var x in contacts)
                    {
                        var normal = x.normal;
                        //如果法线与重力反方向的夹角小于最大角度
                        if (Vector2.Angle(-gravity, normal) < 地表最大角度)
                        {
                            groundNormal = normal;
                            break;
                        }
                    }

                    //如果地面法线不为0
                    //设置地面法线
                    groundNormalD.Data = groundNormal;
                    if (groundNormal != Vector2.zero)
                    {
                        //设置站立地面为true
                        standGroundD.Data = true;
                    }
                    else
                    {
                        //设置站立地面为false
                        standGroundD.Data = false;

                    }



                    return groundNormal;
                }








                return enabler;
            }


            [System.Serializable]
            public class Tag
            {
                [AllowNesting]
                [Tag]
                public string tag;

            }


        }


    }

}

