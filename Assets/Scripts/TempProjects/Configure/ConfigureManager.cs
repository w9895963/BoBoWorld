using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Configure
{
    public class ConfigureManager : IConfigureRunnerManager
    {



        private List<ConfigureRunner> runners = new List<ConfigureRunner>();
        private bool runnerEnabled = false;
        private bool runnerInitialize = false;

        public void Destroy()
        {
            runners.ForEach(x => x.Destroy());
            runnerEnabled = false;
            runnerInitialize = false;
        }

        public void Disable()
        {
            runners.ForEach(x => x.Disable());
            runnerEnabled = false;
        }

        public void Enable()
        {
            runners.ForEach(x => x.Enable());
            runnerEnabled = true;
            runnerInitialize = true;
        }

        public void Initialize()
        {
            runners.ForEach(x => x.Initialize());
            runnerEnabled = true;
        }
        ///<summary> 不添加重复 </summary>
        public void AddRunner(IConfigureRunner runner)
        {
            ConfigureRunner r = new ConfigureRunner(runner);
            if (runnerInitialize)
            {
                r.Initialize();
            }
            if (runnerEnabled)
            {
                r.Enable();
            }
            runners.Add(r);
        }

        public void RemoveRunner(IConfigureRunner runner)
        {
            if (runners.TryRemoveAll(x => x.runner == runner, out var r))
            {
                r.ForEach(x => x.Destroy());
            }
        }



        public class ConfigureRunner : IConfigureRunner
        {
            public IConfigureRunner runner;
            private bool enabled = false;
            private bool initialize = false;


            public ConfigureRunner(IConfigureRunner runner)
            {
                this.runner = runner;
            }

            public bool AllowEnable => runner.AllowEnable;

            public void Destroy()
            {
                Disable();
                if (initialize)
                {
                    runner.Initialize();
                    initialize = false;
                }
            }

            public void Disable()
            {
                if (enabled)
                {
                    runner.Disable();
                    enabled = false;
                }
            }

            public void Enable()
            {
                Initialize();
                if (!enabled)
                {
                    runner.Enable();
                    enabled = true;
                }
            }

            public void Initialize()
            {
                if (initialize)
                {
                    return;
                }
                runner.Initialize();
                initialize = true;
            }
        }
    }





}