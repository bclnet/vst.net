using Jacobi.Vst3;
using System.Collections.Generic;
using System.Diagnostics;
using static Jacobi.Vst3.TResult;
using TestFunc = System.Func<Jacobi.Vst3.ITestResult, bool>;
using TestFuncWithContext = System.Func<object, Jacobi.Vst3.ITestResult, bool>;

namespace Jacobi.Vst3.Utility
{
    public static class Testing
    {
        class TestRegistry
        {
            public struct TestWithContext
            {
                public string Desc;
                public TestFuncWithContext Func;
            }
            public static readonly TestRegistry Instance = new();
            public List<(string first, ITest second)> Tests = new();
            public List<(string first, TestWithContext second)> TestsWithContext = new();
        }

        class TestBase : ITest
        {
            readonly string desc;
            public TestBase(string desc) => this.desc = desc;
            public virtual bool Setup() => true;
            public virtual bool Teardown() => true;
            public virtual string GetDescription() => desc;
            public virtual bool Run(ITestResult result) => true;
        }

        class FuncTest : TestBase
        {
            readonly TestFunc func;
            public FuncTest(string desc, TestFunc func) : base(desc) => this.func = func;
            public override bool Run(ITestResult testResult) => func(testResult);
        }

        class FuncWithContextTest : TestBase
        {
            readonly object context;
            readonly TestFuncWithContext func;
            public FuncWithContextTest(object context, string desc, TestFuncWithContext func) : base(desc) { this.context = context; this.func = func; }
            public override bool Run(ITestResult testResult) => func(context, testResult);
        }

        class TestFactoryImpl : ITestFactory
        {
            public TResult CreateTests(object context, ITestSuite parentSuite)
            {
                foreach (var (first, second) in TestRegistry.Instance.Tests) parentSuite.AddTest(first, second);
                foreach (var (first, second) in TestRegistry.Instance.TestsWithContext) parentSuite.AddTest(first, new FuncWithContextTest(context, second.Desc, second.Func));
                return kResultTrue;
            }
        }

        public static void RegisterTest(string name, string desc, TestFunc func) => RegisterTest(name, new FuncTest(desc, func));
        public static void RegisterTest(string name, ITest test) { Debug.Assert(name != null); TestRegistry.Instance.Tests.Add((name, test)); }
        public static void RegisterTest(string name, string desc, TestFuncWithContext func) => TestRegistry.Instance.TestsWithContext.Add((name, new TestRegistry.TestWithContext { Desc = desc, Func = func }));

        public static object CreateTestFactoryInstance(object _) => new TestFactoryImpl();

        //Guid GetTestFactoryUID()
        //{
        //    static FUID uid = FUID::fromTUID(TestFactoryUID);
        //    return uid;
        //}
    }
}
