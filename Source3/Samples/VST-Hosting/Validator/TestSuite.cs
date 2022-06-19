using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using System.Collections.Generic;

namespace Steinberg.Vst
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
        public int AddTest(string name, ITest test) { tests.Add(new Test(name, test)); return TResult.S_True; }
        public int AddTestSuite(string name, ITestSuite testSuite) { testSuites.Add((name, testSuite)); return TResult.S_True; }
        public int SetEnvironment(ITest environment) => TResult.E_NotImplemented;
        public int GetTestCount() => tests.Count;
        public int GetTest(int index, out ITest test, out string name)
        {
            var foundTest = tests[index];
            if (foundTest != null)
            {
                test = foundTest.test;
                name = foundTest.name;
                return TResult.S_True;
            }
            test = default;
            name = default;
            return TResult.S_False;
        }
        public int GetTestSuite(int index, out ITestSuite testSuite, out string name)
        {
            if (index < 0 || index >= testSuites.Count)
            {
                name = default;
                testSuite = default;
                return TResult.E_InvalidArg;
            }
            var ts = testSuites[index];
            name = ts.Item1;
            testSuite = ts.Item2;
            return TResult.S_True;
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