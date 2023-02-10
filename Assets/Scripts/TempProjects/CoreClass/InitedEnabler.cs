using System;


namespace CoreClass
{
    public partial class InitedEnabler
    {
        public Func<bool> AccessEnabled;
        public Func<bool> AccessInited;
        public Action OnInit;
        public Action OnUnInit;
        public Action OnDisable;
        public Action OnEnable;
        public bool Enabled => enabled;
        public bool Initialized => initialized;
        public void Update()
        {
            bool targetEnabled = AccessEnabled?.Invoke() ?? false;
            bool targetInitialized = AccessInited?.Invoke() ?? false;

            if (targetInitialized != initialized)
            {
                //如果需要初始化, 则初始化
                if (targetInitialized)
                {
                    initialized = true;
                    OnInit?.Invoke();
                }
                //如果需要取消初始化, 则取消初始化, 取消前先停用
                else
                {
                    if (enabled == true)
                    {
                        enabled = false;
                        OnDisable?.Invoke();
                    }
                    initialized = false;
                    OnUnInit?.Invoke();
                }
            }
            //如果已经初始化, 且启用状态需要改变, 则改变启用状态
            else
            if (initialized == true || targetInitialized != initialized)
            {
                if (targetEnabled)
                {
                    enabled = true;
                    OnEnable?.Invoke();
                }
                else
                {
                    enabled = false;
                    OnDisable?.Invoke();
                }
            }
        }







        private bool initialized;
        private bool enabled;

        public InitedEnabler(IRunnerConfig config)
        {
            OnInit = config.OnInit;
            OnUnInit = config.OnUnInit;
            OnDisable = config.OnDisable;
            OnEnable = config.OnEnable;
        }

        public InitedEnabler()
        {
        }
    }

    //*额外方法
    public partial class InitedEnablerActive : InitedEnabler
    {
        public void Init()
        {
            base.AccessInited = () => true;
            base.Update();
        }
        public void UnInit()
        {
            base.AccessEnabled = () => false;
            base.AccessInited = () => false;
            base.Update();
        }
        public void Enable()
        {
            base.AccessInited = () => true;
            base.AccessEnabled = () => true;
            base.Update();
        }
        public void Disable()
        {
            base.AccessEnabled = () => false;
            base.Update();
        }


        //隐藏:Update
        private new void Update()
        {
            throw new NotImplementedException();
        }
        //隐藏:AccessorEnabled
        private new Func<bool> AccessEnabled;
        //隐藏:AccessorInitialized
        private new Func<bool> AccessInited;
    }

}