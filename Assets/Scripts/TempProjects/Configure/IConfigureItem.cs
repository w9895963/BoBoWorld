using UnityEngine;


//命名空间：配置
namespace Configure
{
    public interface IConfigureItem : ICreate<MonoBehaviour, CoreClass.InitedEnabler> 
    {
        public CoreClass.InitedEnabler CreateRunnerConfig(MonoBehaviour mono) => Create(mono);
    }



}
