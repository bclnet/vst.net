using Steinberg.Vst3;
using System.Collections.Generic;
using static Steinberg.Vst3.TResult;

namespace Steinberg
{
    public class TestSuite : ITestSuite
    {
        protected class Test
        {
            public string name;
            public ITest test;
            public Test(string name, ITest test) { this.name = name; this.test = test; }
        }

        string name;
        List<Test> tests = new();
        List<(string, ITestSuite)> testSuites = new();

        public TestSuite(string name) => this.name = name;
        public TResult AddTest(string name, ITest test) { tests.Add(new Test(name, test)); return kResultTrue; }
        public TResult AddTestSuite(string name, ITestSuite testSuite) { testSuites.Add((name, testSuite)); return kResultTrue; }
        public TResult SetEnvironment(ITest environment) => kNotImplemented;
        public int GetTestCount() => tests.Count;
        public TResult GetTest(int index, out ITest test, out string name)
        {
            var foundTest = tests[index];
            if (foundTest != null)
            {
                test = foundTest.test;
                name = foundTest.name;
                return kResultTrue;
            }
            test = default;
            name = default;
            return kResultFalse;
        }
        public TResult GetTestSuite(int index, out ITestSuite testSuite, out string name)
        {
            if (index < 0 || index >= testSuites.Count)
            {
                name = default;
                testSuite = default;
                return kInvalidArgument;
            }
            var ts = testSuites[index];
            name = ts.Item1;
            testSuite = ts.Item2;
            return kResultTrue;
        }
        public ITestSuite GetTestSuite(string name)
        {
            foreach (var testSuite in testSuites)
                if (testSuite.Item1 == name) return testSuite.Item2;
            return null;
        }

        public string Name => name;
    }
}