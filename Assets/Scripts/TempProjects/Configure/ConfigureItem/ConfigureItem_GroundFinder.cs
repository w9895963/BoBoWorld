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
        public partial class ConfigureItem_GroundFinder : ConfigureBase
        {
            #region //&界面部分            

            [Header("参数")]


            [Tooltip("地面向量与重力的夹角大于此角度则不视为地面")]
            public float 地面最大夹角 = 10;

            [Tooltip("此标签外的物体不被视为地面")]
            [TagPopup]
            [Label(0)]
            public List<string> 地面标签 = new List<string>() { "地表碰撞体" };


            [Header("动态参数")]
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info1", true, "", 0, prefix = true, title = "重力向量", tooltip = "获得重力向量")]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> 重力 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.重力向量);







            [Header("输出参数")]
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地表法线", tooltip = "获得脚下的地面法线")]
            public Configure.Interface.DataHolder_NameDropDown<Vector2> 地表法线 = new Configure.Interface.DataHolder_NameDropDown<Vector2>(DataName.地表法线);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "站在地面", tooltip = "此刻是否正站在地面上")]
            public Configure.Interface.DataHolder_NameDropDown<bool> 是否站在地面 = new Configure.Interface.DataHolder_NameDropDown<bool>(DataName.是否站在地面);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面物体", tooltip = "获得脚下的地面物体")]
            public Configure.Interface.DataHolder_NameDropDown<GameObject> 地面物体 = new Configure.Interface.DataHolder_NameDropDown<GameObject>(DataName.地面物体);





            [Space(10)]
            public ShowOnlyText 说明 = new ShowOnlyText("检测地面");

            #endregion 
            //&↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑






            //必要组件
            protected override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };







            //构建函数
            public ConfigureItem_GroundFinder()
            {
                createRunner = CreateRunner_;
            }

            private ConfigureRunner CreateRunner_(GameObject gameObject)
            {
                GroundFinderMain gr = new GroundFinderMain(this, gameObject);
                return new ConfigureRunner(gr.initialize, gr.enable, gr.disable, gr.destroy);
            }




            private class GroundFinderMain
            {

                public GameObject gameObject;
                private float maxAngle;
                private Vector2 gravity;
                private Vector2 groundNormal;
                private List<string> tags;
                private (Action enable, Action disable) enabler;
                private List<(Collider2D, ContactPoint2D[])> groundCollider = new List<(Collider2D, ContactPoint2D[])>();
                private EventDataHandler<Vector2> groundNormalD;
                private EventDataHandler<bool> standGroundD;
                private EventDataHandler<GameObject> groundObjectD;
                private ConfigureItem_GroundFinder ins;

                public GroundFinderMain(ConfigureItem_GroundFinder ins, GameObject gameObject)
                {
                    this.ins = ins;
                    this.gameObject = gameObject;
                }

                public void initialize()
                {
                    groundCollider.Clear();
                    enabler = default;

                    maxAngle = ins.地面最大夹角;
                    gravity = Vector2.down;
                    groundNormal = Vector2.zero;
                    tags = ins.地面标签.Distinct().ToList();




                    //当引用改变时自动更新重力方向的值
                    EventDataF.GetData<Vector2>(ins.重力.dataName).OnUpdateDo_AddEnabler((d) =>
                    {
                        gravity = d;
                    }, ref enabler);


                    //地面法线
                    groundNormalD = ins.地表法线.GetEventDataHandler(gameObject);
                    //站立地面
                    standGroundD = ins.是否站在地面.GetEventDataHandler(gameObject);

                    //地面物体
                    groundObjectD = ins.地面物体.GetEventDataHandler(gameObject);

                }

                public void enable()
                {
                    enabler.enable?.Invoke();
                    BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Add(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2D);
                }

                public void disable()
                {
                    enabler.disable?.Invoke();
                    BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Remove(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2D);
                }

                public void destroy()
                {
                }



                private void OnCollisionEnter2D(Collision2D obj)
                {


                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!tags.Contains(obj.gameObject.tag))
                        return;


                    groundCollider.Add((obj.collider, obj.contacts));

                    CalcGroundNormal(obj.contacts);
                    SetGroundNormal();
                    SetStandGround();
                    SetGroundObject();

                }
                private void OnCollisionStay2D(Collision2D obj)
                {
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

                private void OnCollisionExit2D(Collision2D obj)
                {
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





                private Vector2 CalcGroundNormal(ContactPoint2D[] contacts)
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



                private void SetGroundNormal()
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

                private void SetStandGround()
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

                private void SetGroundObject()
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



            }








         







        }








    }

}

