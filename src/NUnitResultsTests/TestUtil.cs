using System;
using NUnit.Core;

namespace NUnit.Extras.Tests
{
    public class TestUtil
    {
        public static int CountTestCases(TestResult result)
        {
            TestSuiteResult suiteResult = result as TestSuiteResult;

            if (suiteResult == null)
                return 1;

            int sum = 0;
            foreach (TestResult child in suiteResult.Results)
                sum += CountTestCases(child);
            return sum;
        }

        public static int CountTestFixtures(TestSuiteResult result)
        {
            if (IsFixture(result))
                return 1;

            int sum = 0;
            foreach (TestResult child in result.Results)
                sum += CountTestFixtures((TestSuiteResult)child);

            return sum;
        }

        public static bool IsFixture(TestSuiteResult result)
        {
            if (result.Results.Count == 0)
                return true; // Empty suite can only be a fixture

            bool hasTestCases = false;

            foreach (TestResult child in result.Results)
            {
                TestSuiteResult childSuite = child as TestSuiteResult;
                if (childSuite == null)
                    hasTestCases = true;
                else if (IsParameterizedTestMethod(childSuite))
                    return true;
            }
                
            return hasTestCases;
        }

        public static bool IsParameterizedTestMethod(TestSuiteResult result)
        {
            foreach(TestResult child in result.Results)
                if ( child.Name.EndsWith(")"))
                    return true;

            return false;
        }

        public static TestResult FindChildResult(TestSuiteResult result, string name)
        {
            foreach (TestResult child in result.Results)
                if (child.Name == name) return child;

            //Assert.Fail("Unable to locate {0} under test result (1)", name, result.Name);

            return null;
        }
    }
}
