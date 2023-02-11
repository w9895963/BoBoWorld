using System;
using UnityEngine;

namespace CoreClass
{
    public class InitedEnabler
    {
        public Func<bool> AccessInited;
        public Func<bool> AccessEnabled;
        public Action OnInit;
        public Action OnUnInit;
        public Action OnDisable;
        public Action OnEnable;
        public bool Enabled => enabled;
        public bool Initialized => initialized;
        public void Update()
        {
            bool targetInitialized = AccessInited?.Invoke() ?? false;
            bool targetEnabled = AccessEnabled?.Invoke() ?? false;
            bool? doEnable = null;
            bool? doInit = null;


            if (targetInitialized == true && targetEnabled == true)
            {
                if (initialized == false)
                {
                    doInit = true;
                }
                if (enabled == false)
                {
                    doEnable = true;
                }
            }
            else if (targetInitialized == true && targetEnabled == false)
            {
                if (initialized == false && enabled == false)
                {
                   doInit = true;
                }
                else if (initialized == true && enabled == true)
                {
                   doEnable = false;
                }
                else if (initialized == true && enabled == false)
                {
                    //啥都不做
                }
                else
                {
                    Debug.LogError($"不可能的情况:{initialized},{enabled}");
                }

            }
            else if (targetInitialized == false && targetEnabled == true)
            {
                if (enabled == true)
                {
                    doEnable = false;
                }
                if (initialized == true)
                {
                    doInit = false;
                }
            }
            else if (targetInitialized == false && targetEnabled == false)
            {
                if (initialized == true && enabled == true)
                {
                    doEnable = false;
                    doInit = false;
                }
                else if (initialized == true && enabled == false)
                {
                    doInit = false;
                }
                else if (initialized == false && enabled == false)
                {
                    //啥都不做
                }
                else
                {
                    Debug.LogError($"不可能的情况:{initialized},{enabled}");
                }
            }


            initialized = doInit ?? initialized;
            enabled = doEnable ?? enabled;
            if (doInit == true)
            {
                OnInit?.Invoke();
            }
            else if (doInit == false)
            {
                OnUnInit?.Invoke();
            }

            if (doEnable == true)
            {
                OnEnable?.Invoke();
            }
            else if (doEnable == false)
            {
                OnDisable?.Invoke();
            }


            //测试用
            // Debug.Log($"targetInitialized:{AccessInited?.Invoke()},targetEnabled:{AccessEnabled?.Invoke()}");
            // Debug.Log($"targetInitialized:{targetInitialized},targetEnabled:{targetEnabled}");
            // Debug.Log($"OnInit: {OnInit != null},OnUnInit: {OnUnInit != null},OnDisable: {OnDisable != null},OnEnable: {OnEnable != null}");
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
    public class InitedEnablerActive
    {
        public event Action OnInit { add => enabler.OnInit += value; remove => enabler.OnInit -= value; }
        public event Action OnUnInit { add => enabler.OnUnInit += value; remove => enabler.OnUnInit -= value; }
        public event Action OnDisable { add => enabler.OnDisable += value; remove => enabler.OnDisable -= value; }
        public event Action OnEnable { add => enabler.OnEnable += value; remove => enabler.OnEnable -= value; }
        public bool Enabled => enabler.Enabled;
        public bool Initialized => enabler.Initialized;

        public void Init()
        {
            enabler.AccessInited = () => true;
            enabler.Update();
        }
        public void UnInit()
        {
            enabler.AccessEnabled = () => false;
            enabler.AccessInited = () => false;
            enabler.Update();
        }
        public void Enable()
        {
            enabler.AccessInited = () => true;
            enabler.AccessEnabled = () => true;
            enabler.Update();

        }
        public void Disable()
        {
            enabler.AccessEnabled = () => false;
            enabler.Update();
        }











        private InitedEnabler enabler = new();
    }






}