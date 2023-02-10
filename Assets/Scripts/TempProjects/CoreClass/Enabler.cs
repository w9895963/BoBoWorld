using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreClass
{
    public abstract partial class Enabler
    {
        public virtual bool Enabled
        {
            get
            {
                Debug.LogWarning($"{this.GetType()} 未实现 {nameof(Enabled)}");
                return default;
            }
        }
        public virtual Action<bool> OnEnabled
        {
            get
            {
                Debug.LogWarning($"{this.GetType()} 未实现 {nameof(OnEnabled)}");
                return default;
            }
            set
            {
                Debug.LogWarning($"{this.GetType()} 未实现 {nameof(OnEnabled)}");
            }
        }
        public virtual void Enable()
        {
            Debug.LogWarning($"{this.GetType()} 未实现 {nameof(Enable)}");
        }
        public virtual void Disable()
        {
            Debug.LogWarning($"{this.GetType()} 未实现 {nameof(Disable)}");
        }
        public virtual void Toggle()
        {
            Debug.LogWarning($"{this.GetType()} 未实现 {nameof(Toggle)}");
        }
        public virtual void Update()
        {
            Debug.LogWarning($"{this.GetType()} 未实现 {nameof(Update)}");
        }


    }
    public abstract partial class Enabler
    {
        public class AutoEnabler : Enabler
        {

            public override Action<bool> OnEnabled { get => OnEnabledAction; set => OnEnabledAction = value; }
            public Func<bool> EnabledAccessor;
            public override bool Enabled
            {
                get
                {
                    if (EnabledAccessor == null)
                    {
                        Debug.LogWarning($"{this.GetType()} 未定义 {nameof(EnabledAccessor)}");
                        return default;
                    }
                    return EnabledAccessor.Invoke();
                }
            }
            public override void Update()
            {
                if (EnabledAccessor == null)
                {
                    Debug.LogWarning($"{this.GetType()} 未定义 {nameof(EnabledAccessor)}");
                    return;
                }

                if (EnabledAccessor?.Invoke() == true)
                {
                    OnEnabledAction?.Invoke(true);
                }
                else
                {
                    OnEnabledAction?.Invoke(false);
                }
            }


            private Action<bool> OnEnabledAction;
        }
    }


    public interface IEnabled
    {
        Enabler Enabler { get; }
    }

}


 
