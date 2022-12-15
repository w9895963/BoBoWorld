using System;
using System.Collections.Generic;
using EventDataS;
using NaughtyAttributes;
using UnityEngine;


//命名空间：配置
namespace ConfigureS
{
    //类：输入数据,可序列化,界面显示类
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
        public DataName 预设名;

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

        public ImportData(DataName name, DataType type)
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
            SetDataForConstructor(data);
            OnValueChanged();
        }
        public ImportData(System.Object data, System.Enum Name)
        {
            导入方式 = ImportType.恒定数据;
            数据名 = Name.ToString();

            //历遍枚举Name的类型
            SetDataForConstructor(data);
            OnValueChanged();
        }

        //*内部:界面相关
        //当界面值改变时
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
                title += GetCustomData().ToString();
            }
            title += $" => {数据名}";
            return title;



        }

        //方法：获取恒定数据
        public object GetCustomData()
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

        //方法：获取数据访问器
        public Func<T> GetDataAccessor<T>()
        {
            return () => (T)GetCustomData();
        }



        //方法：构造赋予数据
        private void SetDataForConstructor(object data)
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




        //*外部接口
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


        public DataHandler<T> GetDataHandler<T>(GameObject gameObject)
        {
            return new DataHandler<T>(this, gameObject);
        }


        //方法：获取数据处理
        public EventDataHandler GetEventDataHandler(GameObject gameObject)
        {
            //如果是恒定数据
            if (导入方式 == ImportType.恒定数据)
            {
               return null;
            }
            //获取事件数据
            // EventDataF.GetData(DataName, out EventDataS.EventDataCore.EventData eventData);
            return null;
        }


        //*枚举
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
        public DataName 预设名;


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

        public ExportData(DataName name)
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
        public DataName 数据名;
        //条件执行条件
        public ConditionType 执行条件;


        //枚举：条件执行条件
        public enum ConditionType
        {
            为真,
            为假,
        }
    }

    public class DataHandler
    {
        
    }
    //类：数据处理器
    public class DataHandler<T>
    {
        private GameObject gameObject;
        //导入数据
        private ImportData importData;
        //恒定数据
        private T constantData;
        //外部数据
        private EventDataHandler<T> dataHandler;



        //*构造函数
        public DataHandler(ImportData importData, GameObject gameObject)
        {
            this.importData = importData;
            this.gameObject = gameObject;
            //*获取数据
            //如果导入方式为恒定数据
            if (importData.导入方式 == ImportData.ImportType.恒定数据)
            {
                constantData = (T)(object)importData.GetCustomData();
            }
            //如果导入方式为预设名
            else if (importData.导入方式 == ImportData.ImportType.从预设数据导入)
            {
                dataHandler = EventDataF.GetData<T>( importData.预设名,gameObject);
            }
            //如果导入方式为自定义名
            else if (importData.导入方式 == ImportData.ImportType.从自定义数据导入)
            {
                dataHandler = EventDataF.GetData<T>( importData.自定义名,gameObject);
            }
        }




        //方法：设置数据
        public void SetData(T data)
        {
            //如果外部数据为空则报错
            if (dataHandler == null)
            {
                Debug.LogError("尝试为恒定数据赋值");
                return;
            }
            //如果外部数据不为空则设置数据
            dataHandler.Data = data;
        }
        //方法：获取数据
        public T GetData()
        {
            //*已经获取过数据则直接返回
            //如果数据或事件数据不为空则返回
            if (constantData != null)
            {
                return constantData;
            }
            //如果事件数据不为空则返回
            if (dataHandler != null)
            {
                return dataHandler.Data;
            }

            //*如果都为空则返回默认值,报错
            Debug.LogError("数据为空");
            return default(T);
        }
        //方法：获取事件数据
        public EventDataHandler GetEventDataHandler()
        {
            return dataHandler;
        }
    }




    //*枚举
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
    //枚举：对象数据名称
    public enum DataName
    {
        输入指令_移动,
        输入指令_跳跃,
        输入指令_冲刺,
        行走方向,
        地表法线,
        行走施力,
    }

}

