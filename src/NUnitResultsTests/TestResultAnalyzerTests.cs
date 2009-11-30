using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    [TestFixture]
    public class TestResultAnalyzerTests
    {
        static readonly string resultFile = @"..\..\TestResult.xml";
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

            Assert.AreEqual(148, analyzer.Children.Count);

            Assert.AreEqual(1194, analyzer.TestCount);
            Assert.AreEqual(0, analyzer.FailureCount);
            Assert.AreEqual(1, analyzer.NotRunCount);
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
    }
}
