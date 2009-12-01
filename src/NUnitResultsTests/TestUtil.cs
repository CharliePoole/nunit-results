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

        public static int CountTestFixtures(TestResult result)
        {
            TestSuiteResult suiteResult = result as TestSuiteResult;

            if (suiteResult == null)
                return 0;

            if (suiteResult.Results.Count == 0)
                return 1; // Empty suite can only be a fixture

            if (suiteResult.Results[0] is TestCaseResult)
                return 1; // Only fixtures contain test cases (so far)

            int sum = 0;
            foreach (TestResult child in suiteResult.Results)
                sum += CountTestFixtures(child);
            return sum;
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
