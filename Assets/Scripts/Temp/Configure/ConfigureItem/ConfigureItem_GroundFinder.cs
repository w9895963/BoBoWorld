using System;
using System.Collections.Generic;
using System.Linq;
using EventData;
using StackableDecorator;
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














            public bool test;
            [Header("固定参数")]
            //地表最大角度
            [Tooltip("大于此角度则不视为地面")]
            [NaughtyAttributes.ShowIf("test")]
            public float 地表最大角度 = 10;
            [OneLine.OneLine]
            public DataAccess<float> maxAngle = new DataAccess<float>();


            [SerializeReference, SubclassSelector]
            public Configure.DataNameDropdownHelper dataNameHelper;






            [Tooltip("此标签外的物体不被视为地面")]
            [OneLine.OneLine]
            [OneLine.HideLabel]
            [NaughtyAttributes.Label("地表碰撞体标签"), NaughtyAttributes.Tag]
            public List<string> collisionTags = new List<string>();




            //脚本说明
            [NaughtyAttributes.Label("说明")]
            public ShowOnlyText 脚本说明_ = new ShowOnlyText("输入: 无", "输出: 地面法线, 站立地面");

            //必要组件
            public override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };





            //类:数据访问
            [System.Serializable]
            public class DataAccess<T>
            {
                [Tooltip("使用共享数据")]
                public bool import;
                [NaughtyAttributes.AllowNesting, NaughtyAttributes.DisableIf("import"), NaughtyAttributes.Label("")]
                [OneLine.OneLine, OneLine.HideLabel]
                public T data;

                [NaughtyAttributes.AllowNesting, NaughtyAttributes.EnableIf("import")]
                [OneLine.OneLine, OneLine.HideLabel]
                public Configure.DataNameDropdown<T> dataName;
            }











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




        }

        [System.Serializable]
        [AddTypeMenu("物理/地面检测器")]
        public partial class ConfigureItem_GroundFinder_ : ConfigureBase_
        {







            [Header("导入参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "最大角度", tooltip = "大于此角度则不视为地面")]
            public Configure.Interface.DataGetterHold<float> maxAngleGetIn = new Configure.Interface.DataGetterHold<float>(10);

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "重力向量", tooltip = "角色的重力向量")]
            public Configure.Interface.DataGetterHold<Vector2> gravityIn = new Configure.Interface.DataGetterHold<Vector2>(Vector2.down, DataName.重力向量, true);

            [Tooltip("此标签外的物体不被视为地面")]
            [TagPopup]
            public List<string> 地表碰撞体标签 = new List<string>() { "地表碰撞体" };
            [Space(10)]
            [Header("导出参数")]

            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面法线", tooltip = "获取此刻地面的法线")]
            public Configure.Interface.DataSetterHolder<Vector2> groundNormalIn = new Configure.Interface.DataSetterHolder<Vector2>(DataName.地表法线);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "站在地面", tooltip = "此刻是否正站在地面上")]
            public Configure.Interface.DataSetterHolder<bool> standOnGroundIn = new Configure.Interface.DataSetterHolder<bool>(DataName.站在地面);
            [Tooltip("")]
            [StackableField]
            [HorizontalGroup("info2", true, "", 0, prefix = true, title = "地面物体", tooltip = "获得脚下的地面物体")]
            public Configure.Interface.DataSetterHolder<GameObject> groundObjectIn = new Configure.Interface.DataSetterHolder<GameObject>(DataName.地面物体);





            //脚本说明
            [NaughtyAttributes.AllowNesting, NaughtyAttributes.Label("说明")]
            public ShowOnlyText 脚本说明_ = new ShowOnlyText("判断主体是否站立在地面物体上, 并输出一系列数据");

            //必要组件
            public override List<Type> requiredTypes => new List<Type>() { typeof(Rigidbody2D), typeof(Collider2D) };





            //类:数据访问











            //覆盖方法:创建启用器
            public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
            {
                //创建启用器
                (Action Enable, Action Disable) enabler = (null, null);

                //碰撞标签

                List<(GameObject, ContactPoint2D[])> groundObjects = new List<(GameObject, ContactPoint2D[])>();

                float maxAngle = 0;
                Vector2 gravity = Vector2.down;
                Vector2 groundNormal = Vector2.zero;



                (Action Enable, Action Disable) maxAngleSetEnabler = maxAngleGetIn.SetDataOnChanged((x) => maxAngle = x, gameObject);

                //重力方向
                var gravityD = EventDataF.GetData<Vector2>(DataName.重力向量, gameObject);
                (Action Enable, Action Disable) gravityEn = gravityIn.SetDataOnChanged((x) => gravity = x, gameObject);


                //地面法线
                var groundNormalD = groundNormalIn.GetEventDataHandler(gameObject);
                //站立地面
                var standGroundD = standOnGroundIn.GetEventDataHandler(gameObject);

                //地面物体
                var groundObjectD = groundObjectIn.GetEventDataHandler(gameObject);







                enabler.Enable = () =>
                {
                    groundObjects = new List<(GameObject, ContactPoint2D[])>();
                    maxAngleSetEnabler.Enable();
                    gravityEn.Enable();

                    BasicEvent.OnCollision2D_Enter.Add(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Add(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Add(gameObject, OnCollisionExit2D);

                };

                enabler.Disable = () =>
                {
                    maxAngleSetEnabler.Disable();
                    gravityEn.Disable();
                    BasicEvent.OnCollision2D_Enter.Remove(gameObject, OnCollisionEnter2D);
                    BasicEvent.OnCollision2D_Stay.Remove(gameObject, OnCollisionStay2D);
                    BasicEvent.OnCollision2D_Exit.Remove(gameObject, OnCollisionExit2D);
                };










                void OnCollisionEnter2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!地表碰撞体标签.Contains(obj.gameObject.tag))
                        return;
                    groundObjects.Add((obj.gameObject, obj.contacts));
                    CalcAndSetGroundNormal(obj.contacts);
                    SetStandGround();
                    SetGroundObject();

                }
                void OnCollisionStay2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!地表碰撞体标签.Contains(obj.gameObject.tag))
                        return;
                    int v = groundObjects.FindIndex(x => x.Item1 == obj.gameObject);
                    if (v != -1)
                    {
                        groundObjects[v] = (obj.gameObject, obj.contacts);
                    }

                    CalcAndSetGroundNormal(obj.contacts);
                    SetStandGround();
                }

                void OnCollisionExit2D(Collision2D obj)
                {
                    //如果碰撞体标签不包含碰撞体标签则退出
                    if (!地表碰撞体标签.Contains(obj.gameObject.tag))
                        return;
                    groundObjects.RemoveAll(x => x.Item1 == obj.gameObject);
                    ContactPoint2D[] contactPoint2Ds = groundObjects.SelectMany(x => x.Item2).ToArray();
                    CalcAndSetGroundNormal(contactPoint2Ds);
                    SetStandGround();
                    SetGroundObject();

                }





                Vector2 CalcAndSetGroundNormal(ContactPoint2D[] contacts)
                {


                    //如果重力为0
                    if (gravity == Vector2.zero)
                        gravity = Vector2.down;

                    foreach (var x in contacts)
                    {
                        var normal = x.normal;
                        //如果法线与重力反方向的夹角小于最大角度

                        if (Vector2.Angle(-gravity, normal) < maxAngle)
                        {
                            groundNormal = normal;
                            break;
                        }
                    }

                    //如果地面法线不为0
                    //设置地面法线
                    groundNormalD.Data = groundNormal;

                    return groundNormal;


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
                        groundObjectD.Data = groundObjects.LastOrDefault().Item1;
                    }
                    else
                    {
                        groundObjectD.Data = null;
                    }

                }








                return enabler;
            }









        }








    }

}

