// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public abstract class TestResultAnalyzerTest
    {
        protected string ResultFile;

        protected ProjectInfo[] Projects;
        protected string FixtureCheck;

        protected int FixtureCount;
        protected int TestCount;
        protected int FailureCount;
        protected int NotRunCount;
        protected int InconclusiveCount;

        protected TestResultLoader loader;

        public TestResultAnalyzerTest()
        {
            ExpectedTestData data = GetExpectedTestData();

            this.ResultFile = data.ResultFile;
            this.Projects = data.Projects;
            this.FixtureCheck = data.FixtureCheck;
            this.FixtureCount = data.FixtureCount;
            this.TestCount = data.TestCount;
            this.FailureCount = data.FailureCount;
            this.NotRunCount = data.NotRunCount;
            this.InconclusiveCount = data.InconclusiveCount;
        }

        [TestFixtureSetUp]
        public void LoadResultFile()
        {
            loader = new TestResultLoader(ResultFile);
        }

        protected abstract ExpectedTestData GetExpectedTestData();

        [Test]
        public void AnalyzeEntireFile()
        {
            TestResultAnalyzer analyzer = new TestResultAnalyzer(loader.TopLevelResult);

            analyzer.FindFixtures(loader.TopLevelResult);
            analyzer.Analyze();

            Assert.AreEqual(this.FixtureCount, analyzer.Children.Count);

            Assert.AreEqual(this.TestCount, analyzer.TestCount);
            Assert.AreEqual(this.FailureCount, analyzer.FailureCount);
            Assert.AreEqual(this.NotRunCount, analyzer.NotRunCount);
            Assert.AreEqual(this.InconclusiveCount, analyzer.InconclusiveCount);
        }

        [Test]
        public void AnalyzeEntireFileByProject()
        {
            TestResultAnalyzer topLevel = new TestResultAnalyzer("Top Level");

            int fixtureCount = 0;
            foreach (TestResult result in loader.ProjectResults)
            {
                TestResultAnalyzer analyzer = new TestResultAnalyzer(result);
                analyzer.FindFixtures(result);
                topLevel.Children.Add(analyzer);
                fixtureCount += analyzer.Children.Count;
            }

            topLevel.Analyze();

            Assert.AreEqual(Projects.Length, topLevel.Children.Count);
            Assert.AreEqual(FixtureCount, fixtureCount);

            Assert.AreEqual(TestCount, topLevel.TestCount);
            Assert.AreEqual(FailureCount, topLevel.FailureCount);
            Assert.AreEqual(NotRunCount, topLevel.NotRunCount);
            Assert.AreEqual(InconclusiveCount, topLevel.InconclusiveCount);
        }

        [Test]
        public void AnalyzeEachProject()
        {
            foreach (ProjectInfo project in Projects)
            {
                TestResult result = loader.FindProjectResult(project.Name);
                TestResultAnalyzer analyzer = new TestResultAnalyzer(result);

                analyzer.FindFixtures(result);
                analyzer.Analyze();

                Assert.AreEqual(project.FixtureCount, analyzer.Children.Count, project.Name);

                Assert.AreEqual(project.TestCount, analyzer.TestCount, project.Name);
                Assert.AreEqual(project.FailureCount, analyzer.FailureCount, project.Name);
                Assert.AreEqual(project.NotRunCount, analyzer.NotRunCount, project.Name);
                Assert.AreEqual(project.InconclusiveCount, analyzer.InconclusiveCount, project.Name);
            }
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_2_10 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_2_10;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_4_8 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_4_8;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_5_2 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_5_2;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_5_10 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_5_10;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_6_0 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_6_0;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_NUnit_2_6_2 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_6_2;
        }
    }

    [TestFixture]
    public class TestResultAnalyzerTest_MockAssembly_2_6_2 : TestResultAnalyzerTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_6_2;
        }
    }
}
