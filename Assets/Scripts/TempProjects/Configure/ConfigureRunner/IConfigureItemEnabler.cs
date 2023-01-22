//命名空间：配置
using UnityEngine;

namespace Configure
{
    public interface IConfigureItemEnabler
    {
        void Destroy(GameObject gameObject);
        void Disable(GameObject gameObject);
        void Enable(GameObject gameObject);
        void Init(GameObject gameObject);
    }
}

