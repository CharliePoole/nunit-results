using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Extras.Tests
{
    [TestFixture]
    public class TestResultLoaderTests
    {
        static readonly string resultFile = @"..\..\TestResult.xml";
        static TestResultLoader loader;

        [TestFixtureSetUp]
        public void LoadResultFile()
        {
            loader = new TestResultLoader(resultFile);
        }

        [Test]
        public void CanLoadFromFile()
        {
            TestSuiteResult result = loader.TopLevelResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(resultFile, result.Name);
            Assert.AreEqual(1194, CountTestCases(result));
            Assert.AreEqual(148, CountTestFixtures(result));

            Assert.AreEqual(1, result.Results.Count);
            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual(@"D:\Dev\NUnit\nunit-2.5-branch\src\NUnitDevTests.nunit", result.Name);
            Assert.AreEqual(9, result.Results.Count);

            result = (TestSuiteResult)result.Results[1];
            Assert.AreEqual(@"D:\Dev\NUnit\nunit-2.5-branch\src\NUnitCore\tests\bin\Debug\nunit.core.tests.dll", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("NUnit", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("Core", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("Tests", result.Name);
            Assert.AreEqual(52, result.Results.Count);

            result = (TestSuiteResult)result.Results[20];
            Assert.AreEqual("PlatformDetectionTests", result.Name);
            Assert.AreEqual(24, result.Results.Count);
        }

        [Test]
        public void CanListProjects()
        {
            IList projects = loader.ProjectResults;
            Assert.AreEqual(9, projects.Count);
            TestSuiteResult result = (TestSuiteResult)projects[1];
            StringAssert.EndsWith("nunit.core.tests.dll", result.Name);
        }

        [Test]
        public void CanFindProject()
        {
            TestResult result = loader.FindProjectResult("nunit.core.tests");
            Assert.IsNotNull(result);
            StringAssert.EndsWith("nunit.core.tests.dll", result.Name);
        }

        [Test]
        public void CanFindTestResultByProjectAndClass()
        {
            TestResult result = loader.FindTestResult("nunit.core.tests", "PlatformDetectionTests");
            Assert.AreEqual("PlatformDetectionTests", result.Name);
        }

        [Test]
        public void CanFindTestResultByName()
        {
            TestResult result = TestResultLoader.FindTestResult("PlatformDetectionTests", loader.TopLevelResult);
            Assert.AreEqual("PlatformDetectionTests", result.Name);
        }

        #region Helper Methods
        private int CountTestCases(TestResult result)
        {
            if (result is TestCaseResult)
                return 1;

            int sum = 0;
            foreach (TestResult child in ((TestSuiteResult)result).Results)
                sum += CountTestCases(child);
            return sum;
        }

        private int CountTestFixtures(TestResult result)
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
        #endregion
    }
}
