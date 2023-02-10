using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;



//命名空间：配置
namespace Configure
{
    //属性:组件名,配置安装器
    [AddComponentMenu("配置/配置安装器")]
    public partial class ConfigureBuilderMono : MonoBehaviour
    {
        //*按钮:检查缺失组件
        [Button("检查缺失组件")]
        [PropertyOrder(-1)]
        private void CheckRequiredTypes()
        {
            //~缺失组件
            List<string> types = new List<string>();
            List<string> types_;



            types_ =
            配置列表
            // .Log("配置列表")//Test
            .WhereNotNull()
            .SelectManyNotNull(x => x.配置文件列表)
            // .Log("配置文件列表")//Test
            .SelectManyNotNull(x => x.RequiredTypes == null ? new List<Type>() : x.RequiredTypes.Where(y => gameObject.GetComponent(y) == null))
            // .Log("RequiredTypes")//Test
            .Select(x => x.ToString()).ToList();

            types.AddRange(types_);

            缺失组件 = types.Count == 0 ? "无" : string.Join("\n", types);

        }
        [ReadOnly]
        public string 缺失组件 = "无";



        //*配置列表
        [Space]
        public List<ConfigureItemManager> 配置列表 = new List<ConfigureItemManager>();





        //列表:配置启用器列表
        private List<(Action Enable, Action Disable)> enablerList = new List<(Action Enable, Action Disable)>();
        private Dictionary<ConfigureItem, ConfigureRunner> runnerList = new();















        ///<summary> 根据变动更新实际运行的配置, 并执行对应启用等操作 </summary>
        public void UpdateRunners()
        {
            List<ConfigureItem> configureBase_s = 配置列表.SelectMany(x => x.配置文件列表).WhereNotNull().ToList();

            //如果执行列表里没有某配置则添加
            foreach (var item in configureBase_s)
            {
                if (!runnerList.ContainsKey(item))
                {
                    var runner = item.CreateRunner(this);
                    if (runner == null)
                        continue;
                    runner.Init();
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








    }














    ///*初始化
    public partial class ConfigureBuilderMono
    {
        public ConfigureBuilderMono()
        {
            InitedEnabler = new();




            RunnerListMonitor = new(cf =>
            {
                CoreClass.InitedEnablerActive monoRunner = InitedEnabler;
                cf.OnAdd = (configItem, runner) =>
                {
                    CoreClass.AutoEnabler itemEnabler = (configItem as IGetter<CoreClass.AutoEnabler>).Get();

                    itemEnabler.OnEnable += runner.Update;
                    itemEnabler.OnDisable += runner.Update;


                    monoRunner.OnEnable += runner.Update;
                    monoRunner.OnDisable += runner.Update;
                    monoRunner.OnUnInit += runner.Update;
                    monoRunner.OnInit += runner.Update;


                    runner.AccessEnabled = () => itemEnabler.Enabled && monoRunner.Enabled;
                    runner.AccessInited = () =>  monoRunner.Initialized;

                };
                cf.OnRemove = (configItem, runner) =>
                {
                    CoreClass.AutoEnabler itemEnabler = (configItem as IGetter<CoreClass.AutoEnabler>).Get();

                    itemEnabler.OnEnable -= runner.Update;
                    itemEnabler.OnDisable -= runner.Update;

                    monoRunner.OnEnable -= runner.Update;
                    monoRunner.OnDisable -= runner.Update;
                    monoRunner.OnUnInit -= runner.Update;
                    monoRunner.OnInit -= runner.Update;

                };
                return cf;
            });



            BuilderListMonitor = new();
            BuilderListMonitor.SelectToDict(b => b, (b, k) => (k as IGetter<MonoBehaviour, CoreClass.InitedEnabler>).Get(this), RunnerListMonitor);








            ConfigListMonitor = new(配置列表);
            ConfigListMonitor.SelectManyToList(x => ((x as IConfigureRunnerBuilders)?.RunnerBuilders), BuilderListMonitor);

            ConfigListMonitor.Update();






        }
        CoreClass.InitedEnablerActive InitedEnabler;
        CoreClass.ListMonitor<ConfigureItemManager> ConfigListMonitor;
        CoreClass.ListMonitor<IConfigureRunnerBuilder> BuilderListMonitor;
        CoreClass.CoreDict<IConfigureRunnerBuilder, CoreClass.InitedEnabler> RunnerListMonitor;






    }


    //*Unity事件
    public partial class ConfigureBuilderMono
    {
        //*苏醒
        void Awake()
        {
            InitedEnabler.Init();
        }
        void OnEnable()
        {
            InitedEnabler.Enable();
        }

        void OnDisable()
        {
            InitedEnabler.Disable();
        }

        void OnDestroy()
        {
            InitedEnabler.UnInit();
        }


        //*编辑器变动
        void OnValidate()
        {
            ConfigListMonitor.Update();
            // SelfEnabler.Update();
            Debug.Log("OnValidate");
        }
    }
}