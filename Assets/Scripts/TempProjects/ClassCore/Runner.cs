using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassCore
{
    /// <summary> 用于控制初始化和启用 </summary>
    public class Runner : IRunner
    {
        public bool Enabled => _enabled;
        public bool Initialized => _initialized;
        public Func<bool?> AutoEnabled { get => _autoEnabled; set => _autoEnabled = value; }
        public Func<bool?> AutoInitialized { get => _autoInitialized; set => _autoInitialized = value; }
        ///<summary> 更新状态至约束 </summary>
        public void Update()
        {
            //~如果冲突则警告
            var enable = _autoEnabled?.Invoke();
            var init = _autoInitialized?.Invoke();
            if (enable != null && init != null)
            {
                if (enable == true && init == false)
                {
                    Debug.LogWarning("Runner: " + " 的自动启用和初始化状态不一致");
                }
            }
            if (enable == true)
            {
                Enable();
            }
            else
            {
                Disable();
            }
            if (init == true)
            {
                Init();
            }
            else
            {
                UnInit();
            }
        }

        ///<summary> 初始化 </summary>
        public void Init()
        {
            if (_initialized == false)
            {
                _onInitialize?.Invoke();
                _initialized = true;
            }
        }
        ///<summary> 取消初始化 </summary>
        public void UnInit()
        {
            Disable();
            if (_initialized == true)
            {
                _onUnInitialize?.Invoke();
                _initialized = false;
            }
        }

        ///<summary> 启动 </summary>
        public void Enable()
        {
            Init();
            if (_enabled == false)
            {
                _onEnable?.Invoke();
                _enabled = true;
            }
        }
        ///<summary> 停止 </summary>
        public void Disable()
        {
            if (_enabled == true)
            {
                _onDisable?.Invoke();
                _enabled = false;
            }
        }

        private bool _enabled = false;
        private bool _initialized = false;





        public Runner(Action enable = null, Action disable = null, Action initialize = null, Action unInitialize = null)
        {
            _onEnable = enable;
            _onDisable = disable;
            _onInitialize = initialize;
            _onUnInitialize = unInitialize;
        }
        public Runner(Func<Config, Config> actionSetter)
        {
            var value = actionSetter(new());
            _onEnable = value.enable;
            _onDisable = value.disable;
            _onInitialize = value.initialize;
            _onUnInitialize = value.unInitialize;
        }
        public Runner(IRunnerConfig enabler, Func<ConfigCondition, ConfigCondition> configModifier = null)
        {
            var conf = configModifier?.Invoke(new());
            _onEnable = enabler.OnEnable;
            _onDisable = enabler.OnDisable;
            _onInitialize = enabler.OnInit;
            _onUnInitialize = enabler.OnUnInit;
            _autoEnabled = conf?.ConstraintEnabled;
            _autoInitialized = conf?.ConstraintInitialized;
        }

        private Action _onEnable;
        private Action _onDisable;
        private Action _onInitialize;
        private Action _onUnInitialize;
        //约束状态只能为, null, true, false
        private Func<bool?> _autoEnabled;
        //约束状态只能为, null, true, false
        private Func<bool?> _autoInitialized;

        public class Config
        {
            public Action enable;
            public Action disable;
            public Action initialize;
            public Action unInitialize;
        }
        public class ConfigCondition
        {
            public Func<bool?> ConstraintEnabled;
            public Func<bool?> ConstraintInitialized;
        }


    }









    public interface IRunner
    {
        void Init();
        void UnInit();
        void Enable();
        void Disable();
        void Update();
    }
    public interface IRunnerGetter
    {
        IRunner GetRunner();
    }



    public interface IRunnerConfig
    {
        void OnInit();
        void OnUnInit();
        void OnEnable();
        void OnDisable();

    }
}