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
                new ImportData(ObjectDataName.输入指令_移动 ,DataType.浮点数),
                new ImportData(ObjectDataName.地表法线 ,DataType.向量),
                new ImportData(10f, DataName.行走速度),
                new ImportData(10f, "最大加速度"),
            };
            //重设导出参数
            导出参数 = new List<ExportData>()
            {
                new ExportData(ObjectDataName.行走施力),
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


    //枚举：数据类型
    public enum DataType
    {
        浮点数,
        整数,
        布尔值,
        字符串,
        向量,
        游戏物体,
    }

    //类：输入数据,可序列化
    [System.Serializable]
    public class ImportData
    {
        //列表标题
        [HideInInspector]
        public string name = "";
        private string 数据名 = "";
        private DataType 数据类型 = DataType.浮点数;

        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //导入方式
        public ImportType 导入方式;

        //*预设导入数据名
        private bool hide0 = false;
        [HideIf("hide0")]
        [AllowNesting]
        //数据名称
        public ObjectDataName 预设名;

        //*自定义导入数据名
        private bool hide1 = true;
        [HideIf("hide1")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        //数据类型
        public string 自定义名 = "自定义数据名";


        //*自定义数据
        #region            自定义数据


        private bool hideFl = true;
        [HideIf("hideFl")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public float 浮点数 = 0;

        private bool hideInt = true;
        [HideIf("hideInt")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public int 整数 = 0;

        private bool hideBool = true;
        [HideIf("hideBool")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public bool 布尔值 = false;

        private bool hideStr = true;
        [HideIf("hideStr")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public string 字符串 = "字符串";

        private bool hideVec2 = true;
        [HideIf("hideVec2")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public Vector2 向量 = Vector2.zero;

        private bool hideGameObj = true;
        [HideIf("hideGameObj")]
        [OnValueChanged("OnValueChanged")]
        [AllowNesting]
        public GameObject 游戏物体 = null;

        #endregion
        //Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        //*构造函数       

        public ImportData(ObjectDataName name, DataType type)
        {
            数据类型 = type;
            导入方式 = ImportType.从预设数据导入;
            预设名 = name;
            数据名 = name.ToString();
            OnValueChanged();
            bool h0 = hide0;
            bool h1 = hide1;
        }
        public ImportData(string customName, DataType type)
        {
            数据类型 = type;
            导入方式 = ImportType.从自定义数据导入;
            自定义名 = customName;
            数据名 = customName;
            OnValueChanged();
        }
        public ImportData(System.Object data, string name)
        {
            导入方式 = ImportType.恒定数据;
            数据名 = name;
            SetData(data);
            OnValueChanged();
        }
        public ImportData(System.Object data, System.Enum Name)
        {
            导入方式 = ImportType.恒定数据;
            数据名 = Name.ToString();

            //历遍枚举Name的类型
            SetData(data);
            OnValueChanged();
        }


        private void OnValueChanged()
        {
            //设置隐藏数据列表
            List<Action<bool>> hideList = new List<Action<bool>>()
            {
                (b)=>hideFl=b,
                (b)=>hideInt=b,
                (b)=>hideBool=b,
                (b)=>hideStr=b,
                (b)=>hideVec2=b,
                (b)=>hideGameObj=b,
            };
            //如果导入方式为预设名
            if (导入方式 == ImportType.从预设数据导入)
            {
                hide0 = false;
                hide1 = true;
                hideList.ForEach((h) => h(true));


            }
            //如果导入方式为自定义名
            else if (导入方式 == ImportType.从自定义数据导入)
            {
                hide0 = true;
                hide1 = false;
                hideList.ForEach((h) => h(true));
            }
            //如果数据类型
            else if (导入方式 == ImportType.恒定数据)
            {
                hide0 = true;
                hide1 = true;
                hideList.ForEach((h) => h(true));
                //根据数据类型显示对应的数据
                hideList[(int)数据类型](false);


            }




            //设置标题名
            name = GenerateTitle();

        }
        //方法：生产标题
        private string GenerateTitle()
        {
            //定义返回
            string title = 数据类型.ToString() + " : ";
            if (导入方式 == ImportType.从预设数据导入)
            {
                title += 预设名.ToString();
            }
            else if (导入方式 == ImportType.从自定义数据导入)
            {
                title += 自定义名;
            }
            else if (导入方式 == ImportType.恒定数据)
            {
                title += GetData().ToString();
            }
            title += $" => {数据名}";
            return title;
        }

        //方法：获取数据
        private object GetData()
        {

            if (数据类型 == DataType.浮点数)
            {
                return 浮点数;
            }
            else if (数据类型 == DataType.整数)
            {
                return 整数;
            }
            else if (数据类型 == DataType.布尔值)
            {
                return 布尔值;
            }
            else if (数据类型 == DataType.字符串)
            {
                return 字符串;
            }
            else if (数据类型 == DataType.向量)
            {
                return 向量;
            }
            else if (数据类型 == DataType.游戏物体)
            {
                return 游戏物体;
            }

            return null;
        }
        public T GetData<T>()
        {
            return (T)GetData();
        }
        //方法：获取数据访问器
        public Func<T> GetDataAccessor<T>()
        {
            return () => (T)GetData();
        }



        //方法：赋予数据
        private void SetData(object data)
        {
            if (数据类型 == DataType.浮点数)
            {
                浮点数 = (float)data;
                数据类型 = DataType.浮点数;
            }
            else if (数据类型 == DataType.整数)
            {
                整数 = (int)data;
                数据类型 = DataType.整数;
            }
            else if (数据类型 == DataType.布尔值)
            {
                布尔值 = (bool)data;
                数据类型 = DataType.布尔值;
            }
            else if (数据类型 == DataType.字符串)
            {
                字符串 = (string)data;
                数据类型 = DataType.字符串;
            }
            else if (数据类型 == DataType.向量)
            {
                向量 = (Vector2)data;
                数据类型 = DataType.向量;
            }
            else if (数据类型 == DataType.游戏物体)
            {
                游戏物体 = (GameObject)data;
                数据类型 = DataType.游戏物体;
            }
        }



        //属性:数据名,字符串,get
        public string DataName
        {
            get
            {
                if (导入方式 == ImportType.从预设数据导入)
                {
                    return 预设名.ToString();
                }
                else if (导入方式 == ImportType.从自定义数据导入)
                {
                    return 自定义名;
                }
                return name;
            }
        }



        //枚举：导入方式
        public enum ImportType
        {
            从预设数据导入,
            从自定义数据导入,
            恒定数据,
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



    //类：数据存储器
    public class DataHolder
    {
        //导入数据列表
        public List<EventDataHandler> importDatas = new List<EventDataHandler>();
        //导出数据列表
        public List<EventDataHandler> exportDatas = new List<EventDataHandler>();
    }



}
