using System;
using UnityEngine;


//命名空间：配置
namespace Configure
{
    //类:配置运行器
    public class ConfigureRunner
    {

        private bool enabled;
        private bool lived;




        protected Action init;
        protected Action enable;
        protected Action disable;
        protected Action destroy;




        #region //&构造函数

        public ConfigureRunner()
        {
            Construct();
        }

        public ConfigureRunner(Action initialize, Action enable, Action disable, Action destroy)
        {
            Construct(initialize, enable, disable, destroy);
        }
        private void Construct(Action initialize = null, Action enable = null, Action disable = null, Action destroy = null)
        {
            if (initialize != null) this.init += initialize;
            if (enable != null) this.enable += enable;
            if (disable != null) this.disable += disable;
            if (destroy != null) this.destroy += destroy;
            if (this is IConfigureRunnerBuilder configureRunner)
            {
                this.init += configureRunner.Init;
                this.enable += configureRunner.Enable;
                this.disable += configureRunner.Disable;
                this.destroy += configureRunner.Destroy;
            }
        }

        #endregion
        //&Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



        public void Init()
        {
            if (lived == false)
            {
                init?.Invoke();
                lived = true;
            }
            else
            {
                Debug.LogError("已经初始化了");
            }
        }


        public bool Enabled
        {
            set
            {
                //还没初始化或已经被销毁了
                if (lived == false)
                {
                    Debug.LogError("还没初始化或已经被销毁了");
                    return;
                }



                if (enabled == value) return;

                if (value)
                {
                    enable?.Invoke();
                }
                else
                {
                    disable?.Invoke();
                }

                enabled = value;
            }
            get => enabled;
        }

        public void Destroy()
        {
            //如果没初始化
            if (lived)
            {
                Enabled = false;


                destroy?.Invoke();
                lived = false;


            }
            else
            {
                //报错
                Debug.LogError("还没初始化");
            }

        }




    }




}

