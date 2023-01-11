using UnityEngine;


//命名空间：配置
namespace Configure
{
    //类:配置运行器构建参数
    public class ConfigureRunnerBuilderData
    {
        private GameObject gameObject;
        private MonoBehaviour monoBehaviour;

        public ConfigureRunnerBuilderData(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
            gameObject = monoBehaviour.gameObject;
        }

        public GameObject GameObject { get => gameObject; }
        public MonoBehaviour MonoBehaviour { get => monoBehaviour; }
    }


}

