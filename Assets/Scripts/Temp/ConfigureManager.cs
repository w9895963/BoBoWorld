using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using EventDataS;
using Microsoft.CSharp;
using NaughtyAttributes;
using UnityEngine;


//命名空间：配置
namespace ConfigureS
{
    //类：配置管理器
    [CreateAssetMenu(fileName = "配置管理器", menuName = "动态配置/配置管理器", order = 0)]
    //可脚本化对象
    public class ConfigureManager : ScriptableObject
    {
        [Expandable]
        //配置文件列表
        public List<ConfigureBase> ConfigObjects = new List<ConfigureBase>();




    }



    //类:配置基类
    public class ConfigureBase : ScriptableObject
    {
        //字段:启用器
        public (Action Enable, Action Disable) enabler = (null, null);



        //方法:将字段链接到事件数据
        public void LinkFieldToEventData()
        {
            //历遍字段
            foreach (var item in GetType().GetFields())
            {
                //如果字段类型是LinkData
                if (item.FieldType == typeof(InData<>))
                {

                }
            }
        }

        public virtual (Action Enable, Action Disable) CreateEnabler(GameObject gameObject)
        {
            Debug.Log("创建启用器");
            return (null, null);
        }

        public List<EventDataHandler> GetEventDataHandlers()
        {
            return null;
        }



    }


    //配置:計算行走施力
    [CreateAssetMenu(fileName = "計算行走施力", menuName = "动态配置/計算行走施力", order = 1)]
    public class WalkConfig : ConfigureBase
    {
        //本地参数
        [Header("本地参数")]
        //行走速度
        public float 行走速度 = 10;
        public float 最大加速度 = 10;
        [Space]
        //导入数据
        public List<ImportData> 导入参数 = new List<ImportData>()
        {
            new ImportData(ObjectDataName.行走方向),
            new ImportData( ObjectDataName.地表法线),
        };
        //导出
        [Space]
        public List<ExportData> 导出参数 = new List<ExportData>()
        {
            new ExportData(ObjectDataName.行走施力),
        };







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
            //获取数据行走施力
            EventDataHandler<Vector2> moveForce = EventDataF.GetData<Vector2>(gameObject, 导出参数[0].DataName);
            //获取刚体
            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();


            //计算移动施力
            void CalculateMoveForce()
            {
                Vector2 currentVelocity = rigidbody2D.velocity;
                float mass = rigidbody2D.mass;
                Vector2 groundNormalV = groundNormal.Data.magnitude > 0 ? groundNormal.Data.normalized : Vector2.up;
                float moveInputV = moveInput.Data.x;
                Debug.Log(moveInputV);
                float speed = 行走速度;
                float maxForce = 最大加速度 * mass;
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
                Debug.Log("计算移动施力成功" + vector2);
                moveForce.Data = vector2;

            }

            void OnFail()
            {
                Debug.Log("计算移动施力失败");
                moveForce.Data = Vector2.zero;
            }

            enabler = EventDataF.OnDataCondition(CalculateMoveForce, OnFail, moveInput.OnCustom(() =>
            {
                float x = moveInput.Data.x;
                Debug.Log(moveInput.Data);
                return x != 0;
            }), groundNormal.OnUpdate);







            return enabler;
        }





    }

    //枚举：对象数据名称
    public enum ObjectDataName
    {
        输入指令_移动,
        输入指令_跳跃,
        输入指令_冲刺,
        行走方向,
        地表法线,
        行走施力,
    }


    //静态类：核心方法
    public static class CoreF
    {
        //方法:将字段链接到事件数据

    }


    //类：事件数据类型T
    [System.Serializable]
    public class InData<T>
    {
        public string dataName;
        //数据
        private T data;
        public EventDataHandler<T> dataHandler;

        public InData(string dataName)
        {
            this.dataName = dataName;
        }

        //属性:数据
        public T Data { get => dataHandler.Data; set => dataHandler.Data = value; }
    }



    //类：输入数据,可序列化
    [System.Serializable]
    public class ImportData
    {
        [HideInInspector]
        public string name = "";


        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //导入方式
        public NameType 导入名;


        private bool hide0 = false;
        [HideIf("hide0")]
        [AllowNesting]
        //数据名称
        public ObjectDataName 预设名;


        private bool hide1 = true;
        [HideIf("hide1")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //数据类型
        public string 自定义名 = "自定义数据名";

        public ImportData(ObjectDataName name)
        {
            this.name = name.ToString();
            导入名 = NameType.预设名;
            预设名 = name;
            OnValueChanged();
            bool h0 = hide0;
            bool h1 = hide1;
        }
        public ImportData(string customName)
        {
            this.name = customName;
            导入名 = NameType.自定义名;
            自定义名 = customName;
            OnValueChanged();
        }

        //属性:数据名,字符串,get
        public string DataName
        {
            get
            {
                if (导入名 == NameType.预设名)
                {
                    return 预设名.ToString();
                }
                else if (导入名 == NameType.自定义名)
                {
                    return 自定义名;
                }
                return name;
            }
        }



        //枚举：导入方式
        public enum NameType
        {
            预设名,
            自定义名,
        }

        private void OnValueChanged()
        {
            //如果导入方式为预设名
            if (导入名 == NameType.预设名)
            {
                hide0 = false;
                hide1 = true;
                name = 预设名.ToString();
            }
            //如果导入方式为自定义名
            else if (导入名 == NameType.自定义名)
            {
                hide0 = true;
                hide1 = false;
                name = 自定义名;
            }
        }


    }
    //类：导出数据,可序列化
    [System.Serializable]
    public class ExportData
    {
        [HideInInspector]
        public string name = "";
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //导入方式
        public NameType 导入名;


        private bool hide0 = false;
        [HideIf("hide0")]
        [AllowNesting]
        //数据名称
        public ObjectDataName 预设名;


        private bool hide1 = true;
        [HideIf("hide1")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //数据类型
        public string 自定义名 = "自定义数据名";

        public string DataName
        {
            get
            {
                if (导入名 == NameType.可选预设)
                {
                    return 预设名.ToString();
                }
                else if (导入名 == NameType.自定义名)
                {
                    return 自定义名;
                }
                return name;
            }
        }

        public ExportData(ObjectDataName name)
        {
            this.name = name.ToString();
            导入名 = NameType.可选预设;
            预设名 = name;
            OnValueChanged();
            //引用一下防止报错
            bool h0 = hide0;
            bool h1 = hide1;
        }
        public ExportData(string customName)
        {
            this.name = customName;
            导入名 = NameType.自定义名;
            自定义名 = customName;
            OnValueChanged();
        }



        //枚举：导入方式
        public enum NameType
        {
            可选预设,
            自定义名,
        }


        private void OnValueChanged()
        {
            //如果导入方式为预设名
            if (导入名 == NameType.可选预设)
            {
                hide0 = false;
                hide1 = true;
                name = 预设名.ToString();
            }
            //如果导入方式为自定义名
            else if (导入名 == NameType.自定义名)
            {
                hide0 = true;
                hide1 = false;
                name = 自定义名;
            }
        }
    }
    //类：执行条件,可序列化
    [System.Serializable]
    public class Condition
    {
        //数据名
        public ObjectDataName 数据名;
        //条件执行条件
        public ConditionType 执行条件;


        //枚举：条件执行条件
        public enum ConditionType
        {
            为真,
            为假,
        }
    }



    //类：数据管理器
    public class DataManager
    {
        //导入数据列表
        public List<EventDataHandler> importDatas = new List<EventDataHandler>();
        //导出数据列表
        public List<EventDataHandler> exportDatas = new List<EventDataHandler>();
    }


}

