using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Extras.Tests
{
    [TestFixture]
    public class TestResultLoaderTests_2_4_8
    {
        static readonly string resultFile = @"..\..\TestResult-2.4.8.xml";

        static readonly int PROJECT_COUNT = 9;
        static readonly int FIXTURE_COUNT = 159;
        static readonly int TEST_COUNT = 1266;
        
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
            Assert.AreEqual(TEST_COUNT, CountTestCases(result));
            Assert.AreEqual(FIXTURE_COUNT, CountTestFixtures(result));

            Assert.AreEqual(1, result.Results.Count);
            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual(@"C:\Program Files\NUnit 2.4.8\bin\NUnitTests.nunit", result.Name);
            Assert.AreEqual(PROJECT_COUNT, result.Results.Count);

            result = (TestSuiteResult)result.Results[1];
            Assert.AreEqual(@"C:\Program Files\NUnit 2.4.8\bin\nunit.core.tests.dll", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("NUnit", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("Core", result.Name);
            Assert.AreEqual(1, result.Results.Count);

            result = (TestSuiteResult)result.Results[0];
            Assert.AreEqual("Tests", result.Name);
            Assert.AreEqual(51, result.Results.Count);

            result = (TestSuiteResult)result.Results[19];
            Assert.AreEqual("PlatformDetectionTests", result.Name);
            Assert.AreEqual(24, result.Results.Count);
        }

        [Test]
        public void CanListProjects()
        {
            IList projects = loader.ProjectResults;
            Assert.AreEqual(PROJECT_COUNT, projects.Count);
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
