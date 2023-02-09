using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace EaseTool
{
    //公有方法
    public static partial class EaseLog
    {
        ///<summary>输入日志参数</summary>
        public class LogParam
        {
            /// <summary>深度</summary>
            public int depth = 2;

            /// <summary>一次最大显示数量</summary>
            public int maxShowLine = 50;


            #region //&何如显示
            /// <summary>显示行号</summary>
            public bool showLineIndex = true;

            /// <summary>显示标题</summary>
            public bool showLabel = true;
            /// <summary>标题</summary>
            public string label;
            /// <summary>标题颜色</summary>
            public Color labelColor = Color.red;
            /// <summary>标题模板</summary>
            public string labelTemplate = "{0} : ";


            /// <summary>显示颜色</summary>
            public bool showColor = true;


            /// <summary>缩进</summary>
            public int indent = 10;
            /// <summary>缩进字符</summary>
            public string indentContent = " ";


            /// <summary>显示类型</summary>
            public bool showType = true;
            /// <summary>类型颜色</summary>
            public Color typeColor = Color.green;
            /// <summary>类型模板</summary>
            public string typeTemplate = "({0})";


            /// <summary>显示值</summary>
            public bool showValue = true;
            /// <summary>值颜色</summary>
            public Color valueColor = Color.cyan;
            /// <summary>值模板</summary>
            public string valueTemplate = "{0}";


            /// <summary>显示索引</summary>
            public bool showIndex = true;
            /// <summary>索引颜色</summary>
            public Color indexColor = Color.green;
            /// <summary>索引模板</summary>
            public string indexTemplate = "[{0}]";


            /// <summary>显示名称</summary>
            public bool showName = true;
            /// <summary>名称颜色</summary>
            public Color nameColor = Color.yellow;
            /// <summary>名称模板</summary>
            public string nameTemplate = "{0}";


            /// <summary>条目显示模板</summary>
            public string lineTemplate = "{0}{1} : {2}{3}";



            #endregion
            //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


            /// <summary>是否显示共有字段</summary>
            public bool showContentFieldPublic = true;
            /// <summary>是否显示实例字段</summary>
            public bool showContentFieldInstance = true;
            /// <summary>是否显示私有字段</summary>
            public bool showContentFieldPrivate = false;

            public bool showContentPropertyPublic = true;
            /// <summary>是否显示实例字段和属性</summary>
            public bool showContentPropertyInstance = true;
            /// <summary>是否显示私有字段和属性</summary>
            public bool showContentPropertyPrivate = false;

            /// <summary>显示父对象的模板</summary>
            public string contentHolderValueTemplate = "({0} members)⇣";
            /// <summary>显示父对象的模板, 类专用</summary>
            public string contentHolderValueTemplateClass = "({0} fields, {1} properties)⇣";





        }
        ///<summary>输入日志参数</summary>
        private class LogParam_Flag
        {


            public BindingFlags PropertyFlags => (parm.showContentPropertyPublic ? BindingFlags.Public : 0) |
            (parm.showContentPropertyInstance ? BindingFlags.Instance : 0) |
            (parm.showContentPropertyPrivate ? BindingFlags.NonPublic : 0);

            public BindingFlags FieldFlags => (parm.showContentFieldPublic ? BindingFlags.Public : 0) |
            (parm.showContentFieldInstance ? BindingFlags.Instance : 0) |
            (parm.showContentFieldPrivate ? BindingFlags.NonPublic : 0);



            public LogParam_Flag(LogParam parm)
            {
                this.parm = parm;
            }
            private readonly LogParam parm;
        }




        ///<summary> 分析对象并整合成对应的数据 </summary>
        ///<summary culture="en-US">Get the log infos of an object</summary>
        public static List<LogInfo> GetLogInfos(Object content, LogParam param)
        {
            List<LogInfo> logInfos = new List<LogInfo>();
            logInfos.Add(new LogInfo());
            LogInfo info = logInfos[0];

            List<LogInfo> GetReturn() => logInfos;


            info.depth = param.depth;
            info.index = null;
            info.name = "";
            info.type = content == null ? null : content.GetType();
            info.value = "";

            //~根据优先级, 逐个判断类型

            foreach (var contentChecker in contentCheckers)
            {
                if (contentChecker.typeChecker(content))
                {
                    contentChecker.contentAnalyzer(content, param, logInfos);
                    return GetReturn();
                }
            }


            return GetReturn();
        }
        ///<summary>输出日志信息</summary>
        public class LogInfo
        {
            public int depth;
            public int? index;
            public string name;
            public Type type;
            public string value;
        }









        ///<summary>标准输出组装</summary>
        public static string GetLogString(string content, bool enabled, Color color, bool useColor, string template)
        {
            string re = "";
            if (enabled == false | String.IsNullOrEmpty(content)) return re;
            if (useColor == true)
            {
                string colorStr = ColorUtility.ToHtmlStringRGB(color);
                re = $"<color=#{colorStr}>{content}</color>";
            }
            else
            {
                re = content;
            }


            re = string.Format(template, re);
            return re;
        }


    }

    //私有方法
    public static partial class EaseLog
    {
        private static List<(Func<Object, bool> typeChecker, Action<object, LogParam, List<LogInfo>> contentAnalyzer)> contentCheckers = new()
        {
            //如果是: null
            (x => x == null,
            (x, param, logInfos) => logInfos.Last().value = "Null"),

            //如果是:字符串
            (x => x is string,
            (x, param, logInfos) => logInfos.Last().value = $"\"{x}\""),

            //如果是:Delegate
            (x => x is Delegate,
            (x, param, logInfos) => logInfos.Last().value = x.ToString()),

            //如果是:可枚举的
            (x => x is System.Collections.IEnumerable,
            ContentAnalyzer_IEnumerable),

            //如果是:元组
            (x => x.GetType().FullName.StartsWith("System.ValueTuple"),
            ContentAnalyzer_ValueTuple),

            //如果是:值类型
            (x => x.GetType().IsValueType,
            (x, param, logInfos) => logInfos.Last().value = x.ToString()),

            

            //如果是:自身有自定义的ToString方法
            (x => x.GetType().GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance).DeclaringType != typeof(object),
            (x, param, logInfos) => logInfos.Last().value = x.ToString()),

            //如果是:其他类,则打印出所有字段和属性
            (x => x.GetType().GetFields().Length+ x.GetType().GetProperties().Length > 0,
            ContentAnalyzer_Class),
            
            //如果是:其他
            (x => true,
            (x, param, logInfos) => logInfos.Last().value = x.ToString()),
        };


        private static void ContentAnalyzer_IEnumerable(Object content, LogParam param, List<LogInfo> logInfos)
        {
            int curIndex = logInfos.Count - 1;
            int i = 0;
            int curDepth = param.depth;
            param.depth = param.depth - 1;

            foreach (var valueObject in content as IEnumerable)
            {
                if (curDepth > 0)
                {
                    int currentDepth = param.depth;

                    var ChildInfos = GetLogInfos(valueObject, param);
                    ChildInfos[0].index = i;
                    logInfos.AddRange(ChildInfos);
                }
                i++;
            }
            param.depth = curDepth;
            logInfos[curIndex].value = String.Format(param.contentHolderValueTemplate, i);
        }
        private static void ContentAnalyzer_ValueTuple(Object content, LogParam param, List<LogInfo> logInfos)
        {
            var type = content.GetType();
            LogParam_Flag flag = new(param);
            var fields = type.GetFields(flag.PropertyFlags);
            int curIndex = logInfos.Count - 1;
            int currDepth = param.depth;
            param.depth = param.depth - 1;
            for (int i = 0; i < fields.Length; i++)
            {
                if (currDepth > 0)
                {
                    var field = fields[i];
                    var value = field.GetValue(content);
                    var ChildInfos = GetLogInfos(value, param);
                    ChildInfos[0].index = i;
                    ChildInfos[0].name = field.Name;
                    logInfos.AddRange(ChildInfos);
                }
            }
            param.depth = curIndex;
            logInfos[curIndex].value = String.Format(param.contentHolderValueTemplate, fields.Length);
        }
        private static void ContentAnalyzer_Class(Object content, LogParam param, List<LogInfo> logInfos)
        {
            var type = content.GetType();

            LogParam_Flag flag = new(param);
            var fields = type.GetFields(flag.FieldFlags);
            int fieldLength = fields.Length;
            var properties = type.GetProperties(flag.PropertyFlags);
            int curIndex = logInfos.Count - 1;
            int currDepth = param.depth;



            param.depth = currDepth - 1;
            for (int i = 0; i < fieldLength; i++)
            {
                if (currDepth > 0)
                {
                    var field = fields[i];
                    var value = field.GetValue(content);
                    var ChildInfos = GetLogInfos(value, param);
                    ChildInfos[0].index = i;
                    ChildInfos[0].name = field.Name;
                    logInfos.AddRange(ChildInfos);
                }

            }
            param.depth = currDepth - 1;


            int propertiesCount = properties.Length;
            int breakCount = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                if (currDepth > 0)
                {
                    try
                    {
                        var property = properties[i];
                        var value = property.GetValue(content);
                        var ChildInfos = GetLogInfos(value, param);
                        ChildInfos[0].index = i + fieldLength - breakCount;
                        ChildInfos[0].name = property.Name;
                        logInfos.AddRange(ChildInfos);
                    }
                    catch (Exception)
                    {
                        propertiesCount--;
                        breakCount++;
                    }
                }
            }
            param.depth = currDepth;

            logInfos[curIndex].value = string.Format(param.contentHolderValueTemplateClass, fieldLength, propertiesCount);
        }

    }
}
