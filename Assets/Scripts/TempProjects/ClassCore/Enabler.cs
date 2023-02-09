using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCore
{
    /// <summary> 用于控制初始化和启用 </summary>
    public class Enabler
    {
        public Enabler(Action enable = null, Action disable = null, Action initialize = null, Action unInitialize = null)
        {
            _enable = enable;
            _disable = disable;
            _initialize = initialize;
            _unInitialize = unInitialize;
        }
        public Enabler(Func<Config,Config>actionSetter)
        {
            var value = actionSetter(new());
            _enable = value.enable;
            _disable = value.disable;
            _initialize = value.initialize;
            _unInitialize = value.unInitialize;
        }
       
        private Action _enable;
        private Action _disable;
        private Action _initialize;
        private Action _unInitialize;
        public class Config
        {
            public Action enable;
            public Action disable;
            public Action initialize;
            public Action unInitialize;
        }




        public bool Enabled => _enabled;
        public bool Initialized => _initialized;


        ///<summary> 初始化 </summary>
        public void Init()
        {
            if (_initialized == false)
            {
                _initialize?.Invoke();
                _initialized = true;
            }

        }
        ///<summary> 取消初始化 </summary>
        public void UnInit()
        {
            Disable();
            if (_initialized)
            {
                _unInitialize?.Invoke();
                _initialized = false;
            }
        }

        ///<summary> 启动 </summary>
        public void Enable()
        {
            Init();
            if (_enabled == false)
            {
                _enable?.Invoke();
                _enabled = true;
            }
        }
        ///<summary> 停止 </summary>
        public void Disable()
        {
            if (_enabled)
            {
                _disable?.Invoke();
                _enabled = false;
            }
        }
        private bool _enabled = false;
        private bool _initialized = false;

    }

}