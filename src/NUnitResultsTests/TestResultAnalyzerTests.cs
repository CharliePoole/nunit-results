using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    [TestFixture]
    public class TestResultAnalyzerTests
    {
        static readonly string resultFile = @"..\..\TestResult-2.2.10.xml";
        
        static readonly int PROJECT_COUNT = 9;
        static readonly int FIXTURE_COUNT = 148;
        static readonly int TEST_COUNT = 1194;
        static readonly int FAILURE_COUNT = 0;
        static readonly int NOTRUN_COUNT = 1;

        static TestResultLoader loader;

        [TestFixtureSetUp]
        public void LoadResultFile()
        {
            loader = new TestResultLoader(resultFile);
        }

        [Test]
        public void AnalyzeEntireFile()
        {
            TestResultAnalyzer analyzer = new TestResultAnalyzer(loader.TopLevelResult);

            analyzer.FindFixtures(loader.TopLevelResult);
            analyzer.Analyze();

            Assert.AreEqual(FIXTURE_COUNT, analyzer.Children.Count);

            Assert.AreEqual(TEST_COUNT, analyzer.TestCount);
            Assert.AreEqual(FAILURE_COUNT, analyzer.FailureCount);
            Assert.AreEqual(NOTRUN_COUNT, analyzer.NotRunCount);
        }

        [Test]
        public void AnalyzeSingleProject()
        {
            TestSuiteResult result = (TestSuiteResult)loader.FindProjectResult("nunit.core.tests");
            TestResultAnalyzer analyzer = new TestResultAnalyzer(result);

            analyzer.FindFixtures(result);
            analyzer.Analyze();

            Assert.AreEqual(52, analyzer.Children.Count);

            Assert.AreEqual(371, analyzer.TestCount);
            Assert.AreEqual(0, analyzer.FailureCount);
            Assert.AreEqual(1, analyzer.NotRunCount);
        }

        [Test]
        public void AnalyzeEntireFileByProject()
        {
            TestResultAnalyzer topLevel = new TestResultAnalyzer("Top Level");

            int fixtureCount = 0;
            foreach (TestSuiteResult result in loader.ProjectResults)
            {
                TestResultAnalyzer analyzer = new TestResultAnalyzer(result);
                analyzer.FindFixtures(result);
                topLevel.Children.Add(analyzer);
                fixtureCount += analyzer.Children.Count;
            }

            topLevel.Analyze();

            Assert.AreEqual(PROJECT_COUNT, topLevel.Children.Count);
            Assert.AreEqual(FIXTURE_COUNT, fixtureCount);

            Assert.AreEqual(TEST_COUNT, topLevel.TestCount);
            Assert.AreEqual(FAILURE_COUNT, topLevel.FailureCount);
            Assert.AreEqual(NOTRUN_COUNT, topLevel.NotRunCount);
        }
    }
}
