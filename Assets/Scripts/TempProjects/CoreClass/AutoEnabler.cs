using System;


namespace CoreClass
{
    public class AutoEnabler
    {
        public Action OnEnable;
        public Action OnDisable;
        public Func<bool> AccessorEnabled;

        public bool Enabled => enabled;
        public void Update()
        {
            bool target = AccessorEnabled?.Invoke() ?? false;

            if (target != enabled)
            {
                enabled = target;
                if (enabled)
                {
                    OnEnable?.Invoke();
                }
                else
                {
                    OnDisable?.Invoke();
                }
            }
        }


        private bool enabled;
    }
}
