//命名空间：配置
namespace Configure
{
    public interface IConfigureRunnerManager
    {
        void AddRunner(CoreClass.IRunner runner);
        void RemoveRunner(CoreClass.IRunner runner);
        void Initialize();
        void Destroy();
        void Enable();
        void Disable();
    }



}
