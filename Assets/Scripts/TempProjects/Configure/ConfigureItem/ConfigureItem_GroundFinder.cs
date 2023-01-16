using System;
using System.Collections.Generic;
using System.Linq;
using Configure.Interface;
using EventData;
using StackableDecorator;
using UnityEditor;
using UnityEngine;



//命名空间：配置
namespace Configure
{



    namespace ConfigureItem
    {


        [System.Serializable]
        public partial class ConfigureItem_GroundFinder : ConfigureBase_
        {
            [Header("固定参数")]
            [Tooltip("地面向量与重力的夹角大于此角度则不视为地面")]
            public float 地面最大夹角 = 10;

            [Tooltip("此标签外的物体不被视为地面")]
            [StackableDecorator.TagPopup]
            public List<string> 地面标签 = new List<string>() { "地表碰撞体" };


            [Header("导入参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "重力向量", tooltip = "角色的重力向量")]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> gravityIn = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.重力向量);







            [Space(10)]
            [Header("导出参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面法线", tooltip = "获取此刻地面的法线")]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> groundNormalIn = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "站在地面", tooltip = "此刻是否正站在地面上")]
            public Configure.Interface.DataHolder_NameDropDown<bool> standOnGroundIn = new Configure.Interface.DataHolder_NameDropDown<bool>(DataName.是否站在地面);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面物体", tooltip = "获得脚下的地面物体")]
            public Configure.Interface.DataHolder_NameDropDown<GameObject> groundObjectIn = new Configure.Interface.DataHolder_NameDropDown<GameObject>(DataName.地面物体);





            [Space(10)]

            //脚本说明
            public ShowOnlyText 说明 = new ShowOnlyText("判断主体是否站立在地面物体上, 并输出一系列数据");

            //必要组件
            protected override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };





            //构建函数
            public ConfigureItem_GroundFinder()
            {
                createRunner = CreateRunner;
                displaceTypeName = "物理/地面检测器";
            }








            //覆盖方法:创建运行器
            private ConfigureRunner CreateRunner(GameObject gameObject)
            {


                List<(Collider2D, ContactPoint2D[])> groundCollider = new();



                float maxAngle = 地面最大夹角;
                Vector2 gravity = Vector2.down;
                Vector2 groundNormal = Vector2.zero;
                List<string> tags = this.地面标签.Distinct().ToList();
                bool enabled = false;




                //重力方向
                gravityIn.GetEventDataHandler(gameObject).BindDataTo((d) =>
                {
                    if (d == Vector2.zero)
                        return;
                    gravity = d;
                });
                //地面法线
                var groundNormalD = groundNormalIn.GetEventDataHandler(gameObject);
                //站立地面
                var standGroundD = standOnGroundIn.GetEventDataHandler(gameObject);

                //地面物体
                var groundObjectD = groundObjectIn.GetEventDataHandler(gameObject);









                void initialize()
                {
                    BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Add(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2D);
                }

                void enable()
                {

                    enabled = true;
                    groundCollider.Clear();


                }

                void disable()
                {
                    enabled = false;


                    groundObjectD.Data = null;
                    standGroundD.Data = false;
                    groundNormalD.Data = Vector2.zero;
                }


                void destroy()
                {
                    BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Remove(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2D);
                }




                void OnCollisionEnter2D(Collision2D obj)
                {
                    if (enabled == false)
                        return;


                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!tags.Contains(obj.gameObject.tag))
                        return;


                    groundCollider.Add((obj.collider, obj.contacts));

                    CalcGroundNormal(obj.contacts);
                    SetGroundNormal();
                    SetStandGround();
                    SetGroundObject();

                }
                void OnCollisionStay2D(Collision2D obj)
                {
                    if (enabled == false)
                        return;
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!tags.Contains(obj.gameObject.tag))
                        return;


                    int v2 = groundCollider.FindIndex(x => x.Item1 == obj.collider);
                    if (v2 != -1)
                    {
                        groundCollider.RemoveAt(v2);
                        groundCollider.Add((obj.collider, obj.contacts));
                    }

                    CalcGroundNormal(obj.contacts);
                    SetGroundNormal();
                    SetStandGround();
                }

                void OnCollisionExit2D(Collision2D obj)
                {
                    if (enabled == false)
                        return;
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!tags.Contains(obj.gameObject.tag))
                        return;

                    ContactPoint2D[] contactPoint2Ds;


                    groundCollider.RemoveAll(x => x.Item1 == obj.collider);
                    contactPoint2Ds = groundCollider.SelectMany(x => x.Item2).ToArray();

                    CalcGroundNormal(contactPoint2Ds);
                    SetGroundNormal();
                    SetStandGround();
                    SetGroundObject();

                }





                Vector2 CalcGroundNormal(ContactPoint2D[] contacts)
                {
                    var gn = Vector2.zero;

                    //如果重力为0
                    if (gravity == Vector2.zero)
                        gravity = Vector2.down;

                    foreach (var x in contacts)
                    {
                        var normal = x.normal;
                        //如果法线与重力反方向的夹角小于最大角度

                        if (Vector2.Angle(-gravity, normal) < maxAngle)
                        {
                            gn = normal;
                            break;
                        }
                    }

                    groundNormal = gn;
                    return gn;


                }



                void SetGroundNormal()
                {
                    if (groundNormal != Vector2.zero)
                    {
                        //设置地面法线
                        groundNormalD.Data = groundNormal;
                    }
                    else
                    {
                        //设置地面法线为0
                        groundNormalD.Data = Vector2.zero;
                    }
                }

                void SetStandGround()
                {
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
                }

                void SetGroundObject()
                {
                    if (groundNormal != Vector2.zero)
                    {
                        if (groundCollider.Count == 0)
                        {
                            groundObjectD.Data = null;
                        }
                        else
                        {
                            groundObjectD.Data = groundCollider.LastOrDefault().Item1.gameObject;
                        }
                    }
                    else
                    {
                        groundObjectD.Data = null;
                    }

                }



                ConfigureRunner runner = new ConfigureRunner(initialize, enable, disable, destroy);
                return runner;
            }







        }








    }

}

