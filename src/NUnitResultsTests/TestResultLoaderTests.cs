// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using NUnit.Framework;

namespace NUnit.Extras.Tests
{
    public abstract class TestResultLoaderTest
    {
        protected string ResultFile;
        protected string ProjectFile;
        protected ProjectInfo[] Projects;
        protected int FixtureCount;
        protected int TestCount;
        protected string FixtureCheck;

        protected TestResultLoader loader;

        public TestResultLoaderTest()
        {
            ExpectedTestData data = GetExpectedTestData();

            this.ResultFile = data.ResultFile;
            this.ProjectFile = data.ProjectFile;
            this.Projects = data.Projects;
            this.FixtureCount = data.FixtureCount;
            this.TestCount = data.TestCount;
            this.FixtureCheck = data.FixtureCheck;
        }

        [TestFixtureSetUp]
        public void LoadResultFile()
        {
            loader = new TestResultLoader(ResultFile);
        }

        protected abstract ExpectedTestData GetExpectedTestData();

        [Test]
        public void CanLoadFromFile()
        {
            TestResult result = loader.TopLevelResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(ProjectFile, result.Name);

            //Assert.AreEqual(1, result.Results.Count);
            //result = result.Results[0];
            //StringAssert.EndsWith("NUnitTests.nunit", result.Name);
            Assert.AreEqual(Projects.Length, result.Results.Count);

            for (int i = 0; i < Projects.Length; i++)
            {
                TestResult projectResult = result.Results[i];
                StringAssert.EndsWith(Projects[i].Name + ".dll", projectResult.Name);
                Assert.AreEqual(Projects[i].TestCount, TestUtil.CountTestCases(projectResult), projectResult.Name);
                Assert.AreEqual(Projects[i].FixtureCount, TestUtil.CountTestFixtures(projectResult), projectResult.Name);
            }

            Assert.AreEqual(TestCount, TestUtil.CountTestCases(result));
            Assert.AreEqual(FixtureCount, TestUtil.CountTestFixtures(result));
        }

        [Test]
        public void CheckPathToFixture()
        {
            string[] parts = this.FixtureCheck.Split(new char[] { ',' });
            string[] path = parts[0].Split(new char[] { '.' });
            string project = parts[1];

            TestResult result = loader.FindProjectResult(project);
            Assert.IsNotNull(result);

            foreach (string name in path)
            {
                result = TestUtil.FindChildResult(result, name);
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void CanListProjects()
        {
            IList projects = loader.ProjectResults;
            Assert.AreEqual(Projects.Length, projects.Count);
            for (int i = 0; i < Projects.Length; i++)
                StringAssert.EndsWith(Projects[i].Name + ".dll", ((TestResult)projects[i]).Name);
        }

        [Test]
        public void CanFindProjects()
        {
            foreach (ProjectInfo project in Projects)
            {
                TestResult result = loader.FindProjectResult(project.Name);
                Assert.IsNotNull(result, "Unable to find project " + project.Name);
                StringAssert.EndsWith(project.Name + ".dll", result.Name);
            }
        }

        [Test]
        public void CanFindTestResultByProjectAndClass()
        {
            string[] parts = this.FixtureCheck.Split(new char[] { ',' });
            string[] path = parts[0].Split(new char[] { '.' });
            string project = parts[1];
            string name = path[path.Length - 1];
            TestResult result = loader.FindTestResult(project, name);
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void CanFindTestResultByName()
        {
            string[] parts = this.FixtureCheck.Split(new char[] { ',' });
            string[] path = parts[0].Split(new char[] { '.' });
            string name = path[path.Length - 1];
            TestResult result = TestResultLoader.FindTestResult(name, loader.TopLevelResult);
            Assert.AreEqual(name, result.Name);
        }
    }

    [TestFixture]
    public class TestResultLoaderTest_2_2_10 : TestResultLoaderTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_2_10;
        }
    }

    [TestFixture]
    public class TestResultLoaderTest_2_4_8 : TestResultLoaderTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_4_8;
        }
    }

    [TestFixture]
    public class TestResultLoaderTest_2_5_2 : TestResultLoaderTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_5_2;
        }
    }

    [TestFixture]
    public class TestResultLoaderTest_2_5_10 : TestResultLoaderTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_5_10;
        }
    }

    [TestFixture]
    public class TestResultLoaderTest_2_6_0 : TestResultLoaderTest
    {
        protected override ExpectedTestData GetExpectedTestData()
        {
            return ExpectedTestData.NUnit_2_6_0;
        }
    }
}
