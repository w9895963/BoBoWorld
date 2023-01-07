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




        private Action initialize;
        private Action enable;
        private Action disable;
        private Action destroy;

        public ConfigureRunner(Action initialize, Action enable, Action disable, Action destroy)
        {
            this.initialize = initialize;
            this.enable = enable;
            this.disable = disable;
            this.destroy = destroy;
        }

        public void Initialize()
        {
            if (lived == false)
            {
                initialize?.Invoke();
                lived = true;

                Enabled = true;
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
                enabled = value;
                if (enabled)
                {
                    enable?.Invoke();
                }
                else
                {
                    disable?.Invoke();
                }


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

