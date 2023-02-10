using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassCore
{
    /// <summary> 统一启动器 </summary>
    public class ListEnabler<T>
    {
       
        private List<Runner> _enablers = new();

        private Runner selfEnabler = new();


        public bool Enabled => selfEnabler.Enabled;
        public bool Initialized => selfEnabler.Initialized;

        public void Enabler()
        {
            _enablers.ForEach(x => x.Enable());
            selfEnabler.Enable();
        }
        public void Disabler()
        {
            _enablers.ForEach(x => x.Disable());
            selfEnabler.Disable();
        }
        public void Initialize()
        {
            _enablers.ForEach(x => x.Init());
            selfEnabler.Init();
        }
        public void UnInitialize()
        {
            _enablers.ForEach(x => x.UnInit());
            selfEnabler.UnInit();
        }



         private ListMonitor<T> _listMonitor;


    }
}