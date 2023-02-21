using System;
using System.Collections.Generic;
using System.Linq;

using Configure.Inspector;

using EventData;


using UnityEditor;

using UnityEngine;

//命名空间：配置
namespace Configure
{



    namespace ConfigureItems
    {


        [System.Serializable]
        public partial class ConfigureItem_GroundFinder : ConfigureItem
        {


            #region //&界面部分            

            [Header("参数")]


            [Tooltip("地面向量与重力的夹角大于此角度则不视为地面")]
            public float 地面最大夹角 = 10;

            [Tooltip("此标签外的物体不被视为地面")]
            public List<Inspector.TagDropDown> 地面标签 = new List<Inspector.TagDropDown>() { new Inspector.TagDropDown(BaseData.UnityTag.地表碰撞体) };


            [Header("动态参数")]
            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<Vector2> 重力 = new Configure.Inspector.DataNameDropDown<Vector2>(EventData.DataName.Preset.PresetName.重力向量);







            [Header("输出参数")]
            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<Vector2> 地表法线 = new Configure.Inspector.DataNameDropDown<Vector2>(EventData.DataName.Preset.PresetName.地表法线);
            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<bool> 是否与地面物体物理接触 = new Configure.Inspector.DataNameDropDown<bool>(EventData.DataName.Preset.PresetName.是否与地面物体物理接触);
            [Tooltip("")]
            public Configure.Inspector.DataNameDropDown<GameObject> 地面物体 = new Configure.Inspector.DataNameDropDown<GameObject>(EventData.DataName.Preset.PresetName.地面物体);





            [Space(10)]
            public Inspector.HelpText 说明 = new Inspector.HelpText("检测地面, 并获得一系列地面信息");

            #endregion 
            //&↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




















            //构建函数
            public ConfigureItem_GroundFinder()
            {
                requiredTypes = new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };
                CreateRunnerFunc<Runner, ConfigureItem_GroundFinder>();

            }





            private class Runner : ConfigureRunnerT<ConfigureItem_GroundFinder>, IConfigureItemRunner
            {

                private float maxAngle;
                private Vector2 gravity;
                private Vector2 groundNormal;
                private List<string> tags;
                private EventDataHandler<Vector2> groundNormalD;
                private EventDataHandler<bool> standGroundD;
                private EventDataHandler<GameObject> groundObjectD;
                private (Action enable, Action disable) enabler;
                private List<(Collider2D, ContactPoint2D[])> groundCollider = new List<(Collider2D, ContactPoint2D[])>();




                //界面






               public void OnInit()
                {
                    groundCollider.Clear();
                    enabler = default;

                    maxAngle = config.地面最大夹角;
                    gravity = Vector2.down;
                    groundNormal = Vector2.zero;
                    tags = config.地面标签.Select(t => t.tag).WhereNotNull().Distinct().ToList();




                    //当引用改变时自动更新重力方向的值
                    EventDataF.GetData<Vector2>(config.重力.dataName, gameObject).OnUpdateDo_AddEnabler((d) =>
                    {
                        gravity = d;
                    }, ref enabler);


                    //地面法线
                    groundNormalD = config.地表法线.GetEventDataHandler(gameObject);
                    //站立地面
                    standGroundD = config.是否与地面物体物理接触.GetEventDataHandler(gameObject);

                    //地面物体
                    groundObjectD = config.地面物体.GetEventDataHandler(gameObject);
                }
               public void OnEnable()
                {
                    enabler.enable?.Invoke();
                    BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Add(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2D);
                }

               public void OnDisable()
                {
                    enabler.disable?.Invoke();
                    BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Remove(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2D);
                }


               public void OnUnInit()
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
                    SetContactGround();
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
                    SetContactGround();
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
                    SetContactGround();
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

                private void SetContactGround()
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

