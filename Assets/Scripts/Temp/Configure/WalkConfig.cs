using System;
using System.Collections.Generic;
using EventDataS;
using UnityEngine;


//命名空间：配置
namespace ConfigureS
{
    //配置:計算行走施力
    [CreateAssetMenu(fileName = "計算行走施力", menuName = "动态配置/計算行走施力", order = 1)]
    public class WalkConfig : ConfigureBase
    {
        //导入数据
        public List<ImportData> 导入参数 = new List<ImportData>();
        //导出
        [Space]
        public List<ExportData> 导出参数 = new List<ExportData>();







        //覆盖方法:创建启用器
        public override (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
        {
            //创建启用器
            (Action Enable, Action Disable) enabler = (null, null);

            //如果导入参数<2则返回
            if (导入参数.Count < 2)
            {
                return enabler;
            }

            //获取数据行走输入
            EventDataHandler<Vector2> moveInput = EventDataF.GetData<Vector2>(gameObject, 导入参数[0].DataName);
            //获取数据地表法线
            EventDataHandler<Vector2> groundNormal = EventDataF.GetData<Vector2>(gameObject, 导入参数[1].DataName);
            Func<float> speedAc = 导入参数[2].GetDataAccessor<float>();
            Func<float> maxForceAc = 导入参数[3].GetDataAccessor<float>();





            //获取数据行走施力
            EventDataHandler<Vector2> moveForce = EventDataF.GetData<Vector2>(gameObject, 导出参数[0].DataName);
            //获取刚体
            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();


            enabler = EventDataF.OnDataCondition(CalculateMoveForce, OnFail, moveInput.OnCustom(() => moveInput.Data.x != 0), groundNormal.OnUpdate);





            //计算移动施力
            void CalculateMoveForce()
            {
                Vector2 currentVelocity = rigidbody2D.velocity;
                float mass = rigidbody2D.mass;
                Vector2 groundNormalV = groundNormal.Data.magnitude > 0 ? groundNormal.Data.normalized : Vector2.up;
                float moveInputV = moveInput.Data.x;

                float speed = speedAc();
                float maxForce = maxForceAc();
                Func<Vector2> targetVelocity = () =>
                {
                    Vector2 v = default;

                    //行走方向向量
                    Vector2 direction = groundNormalV.y >= 0 ? groundNormalV.Rotate(90) : groundNormalV.Rotate(-90);
                    direction = direction.normalized;

                    //计算期望速度
                    v = direction * speed * moveInputV;

                    return v;
                };
                Vector2 projectVector = groundNormalV.Rotate(90);


                //施力赋值
                Vector2 vector2 = PhysicMathF.CalcForceByVel(currentVelocity, targetVelocity(), maxForce, projectVector, mass);
                moveForce.Data = vector2;

            }

            void OnFail()
            {
                moveForce.Data = Vector2.zero;
            }




            return enabler;
        }



        //基本方法:重设
        private void Reset()
        {
            //重设导入参数
            导入参数 = new List<ImportData>()
            {
                new ImportData(ConfigureS.DataName.输入指令_移动 ,DataType.浮点数),
                new ImportData(ConfigureS.DataName.地表法线 ,DataType.向量),
                new ImportData(10f, DataName.行走速度),
                new ImportData(10f,  DataName.最大加速度),
            };
            //重设导出参数
            导出参数 = new List<ExportData>()
            {
                new ExportData(ConfigureS.DataName.行走施力),
            };
        }


        public enum DataName
        {
            输入指令_移动,
            地表法线,
            行走速度,
            最大加速度,
        }






    }

}

