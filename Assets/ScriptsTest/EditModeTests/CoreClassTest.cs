using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class CoreClassTest
{
    [Test]
    public void InitedEnablerTest()
    {
        //测试初始化
        CoreClass.InitedEnabler runner = new();
        int initedInvoke = 0;
        int unInitedInvoke = 0;
        int enableInvoke = 0;
        int disableInvoke = 0;
        bool enable = false;
        bool init = false;
        void Test(
            bool initFr = false,
            bool enableFr = false,
            bool initTo = false,
            bool enableTo = false,

            int initedTest = 0,
            int unInitedTest = 0,
            int enabledTest = 0,
            int disableTest = 0)
        {
            init = initFr;
            enable = enableFr;
            runner.Update();
            Assert.AreEqual(initFr, runner.Initialized, "runner.Initialized");
            Assert.AreEqual(enableFr, runner.Enabled, "runner.Enabled");
            initedInvoke = 0;
            unInitedInvoke = 0;
            disableInvoke = 0;
            enableInvoke = 0;

            init = initTo;
            enable = enableTo;
            Assert.AreEqual((initTo, enableTo), (runner.AccessInited(), runner.AccessEnabled()), "TargetStateTest");
            runner.Update();

            Assert.AreEqual(
            (initedTest, unInitedTest, enabledTest, disableTest),
            (initedInvoke, unInitedInvoke, enableInvoke, disableInvoke),
            "Invoke");
        }


        runner.OnEnable += () => enableInvoke++;
        runner.OnDisable += () => disableInvoke++;
        runner.OnInit += () => initedInvoke++;
        runner.OnUnInit += () => unInitedInvoke++;

        runner.AccessInited = () => init;
        runner.AccessEnabled = () => enable;

        //执行测试
        Test(false, false, false, false, 0, 0, 0, 0);
        Test(false, false, false, true, 0, 0, 0, 0);
        Test(false, false, true, false, 1, 0, 0, 0);
        Test(false, false, true, true, 1, 0, 1, 0);
        Test(true, true, false, false, 0, 1, 0, 1);
        Test(true, true, false, true, 0, 1, 0, 1);
        Test(true, true, true, false, 0, 0, 0, 1);
        Test(true, true, true, true, 0, 0, 0, 0);




    }


    [Test]
    public void InitedEnablerActiveTest()
    {
        CoreClass.InitedEnablerActive runner = new();
        void InvokeA(int state)
        {
            if (state == 0)
            {
                runner.Init();
                Assert.AreEqual(true, runner.Initialized, "runner.Initialized");
            }
            else if (state == 1)
            {
                runner.UnInit();
                Assert.AreEqual(false, runner.Initialized, "runner.Initialized");
            }
            else if (state == 2)
            {
                runner.Enable();
                Assert.IsTrue(
                 (runner.Enabled == true && runner.Initialized == true) || (runner.Enabled == false && runner.Initialized == false),
                 "runner.Enabled");
            }
            else if (state == 3)
            {
                runner.Disable();
                Assert.AreEqual(false, runner.Enabled, "runner.Enabled");
            }
        }
        void Test(int stateFr, int stateTo, (int, int, int, int) InvokeTest)
        {
            InvokeA(stateFr);
            (int, int, int, int) Invoke = (0, 0, 0, 0);
            runner.OnInit += () => Invoke.Item1++;
            runner.OnUnInit += () => Invoke.Item2++;
            runner.OnEnable += () => Invoke.Item3++;
            runner.OnDisable += () => Invoke.Item4++;

            InvokeA(stateTo);
            Assert.AreEqual(InvokeTest, Invoke, "Invoke");
        }




        //测试基本功能
        InvokeA(0);
        InvokeA(2);
        InvokeA(3);
        InvokeA(1);

        //测试条件触发
        Test(0, 0, (0, 0, 0, 0));
        Test(0, 1, (0, 1, 0, 0));
        Test(0, 2, (0, 0, 1, 0));
        Test(0, 3, (0, 0, 0, 1));
        Test(3, 0, (1, 0, 0, 0));
        Test(3, 1, (0, 1, 0, 0));
        Test(3, 2, (0, 0, 1, 0));
        Test(3, 3, (0, 0, 0, 0));
    }


}
