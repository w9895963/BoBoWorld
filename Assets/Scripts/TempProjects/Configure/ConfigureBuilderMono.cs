using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//命名空间：配置
namespace Configure
{
    //属性:组件名,配置安装器
    [AddComponentMenu("配置/配置安装器")]
    public class ConfigureBuilderMono : MonoBehaviour
    {
        //*按钮:检查缺失组件

        [StackableDecorator.StackableField]
        [StackableDecorator.SideButtons(titles = "检查缺失组件", onLeft = true, actions = nameof(CheckRequiredTypes))]
        [StackableDecorator.Label(0)]
        public string 缺失组件 = "无";
        private void CheckRequiredTypes()
        {
            //~缺失组件
            List<string> types = new List<string>();
            List<string> types_;



            types_ = 配置列表.WhereNotNull()
            .SelectMany(x => x.配置文件列表)
            .SelectMany(x => x.RequiredTypes.Where(y => gameObject.GetComponent(y) == null))
            .Select(x => x.ToString()).ToList();

            types.AddRange(types_);

            缺失组件 = types.Count == 0 ? "无" : string.Join("\n", types);

        }


        //*配置列表
        public List<ConfigureItemManager> 配置列表 = new List<ConfigureItemManager>();



        //列表:配置启用器列表
        private List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>();
        private Dictionary<ConfigureBase, ConfigureRunner> runnerList = new();












        public void UpdateRunners()
        {
            List<ConfigureBase> configureBase_s = 配置列表.SelectMany(x => x.配置文件列表).WhereNotNull().ToList();

            //如果执行列表里没有某配置则添加
            foreach (var item in configureBase_s)
            {
                if (!runnerList.ContainsKey(item))
                {
                    var runner = item.CreateRunner(gameObject, this);
                    runner.Initialize();
                    runnerList.Add(item, runner);
                }
            }
            //如果执行列表里有某配置但配置列表里没有则移除并取消激活
            foreach (var item in runnerList.Keys.ToList())
            {
                if (!configureBase_s.Contains(item))
                {
                    runnerList[item].Enabled = false;
                    runnerList[item].Destroy();
                    runnerList.Remove(item);
                }
            }


            //每一个执行器的激活状态由配置安装器的激活状态以及配置文件的激活状态决定
            foreach (var item in runnerList.Keys)
            {
                if (this.enabled)
                {
                    runnerList[item].Enabled = item.Enabled;
                }
                else
                {
                    runnerList[item].Enabled = false;
                }
            }



        }



        //*Unity事件
        //苏醒
        void Awake()
        {
            // UpdateEnablers();
        }
        void OnEnable()
        {
            UpdateRunners();

        }

        void OnDisable()
        {
            UpdateRunners();
        }









    }
}