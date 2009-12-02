using System;

namespace NUnit.Extras.Tests
{
    public struct ProjectInfo
    {
        public string Name;
        public int FixtureCount;
        public int TestCount;
        public int FailureCount;
        public int NotRunCount;

        public ProjectInfo(string name, int fixtureCount, int testCount, int failureCount, int notRunCount)
        {
            this.Name = name;
            this.FixtureCount = fixtureCount;
            this.TestCount = testCount;
            this.FailureCount = failureCount;
            this.NotRunCount = notRunCount;
        }
    }

    public class ExpectedTestData
    {
        private string resultFile;
        private ProjectInfo[] projects;
        private string fixtureCheck;
        private int fixtureCount;
        private int testCount;
        private int failureCount;
        private int notRunCount;

        public ExpectedTestData(
            string resultFile,
            ProjectInfo[] projects,
            string fixtureCheck)
        {
            this.resultFile = resultFile;
            this.projects = projects;
            this.fixtureCheck = fixtureCheck;

            foreach (ProjectInfo project in projects)
            {
                this.fixtureCount += project.FixtureCount;
                this.testCount += project.TestCount;
                this.failureCount += project.FailureCount;
                this.notRunCount += project.NotRunCount;
            }
        }

        public string ResultFile
        {
            get { return resultFile; }
        }

        public ProjectInfo[] Projects
        {
            get { return projects; }
        }

        public int FixtureCount
        {
            get { return fixtureCount; }
        }

        public int TestCount
        {
            get { return testCount; }
        }

        public string FixtureCheck
        {
            get { return fixtureCheck; }
        }

        public int FailureCount
        {
            get { return failureCount; }
        }

        public int NotRunCount
        {
            get { return notRunCount; }
        }

        public static ExpectedTestData NUnit_2_2_10 = new ExpectedTestData(
            @"..\..\TestResult-2.2.10.xml",
            new ProjectInfo[] {
                new ProjectInfo("nunit.extensions.tests", 3, 8, 0, 0),
                new ProjectInfo("nunit.framework.tests", 51, 397, 0, 2),
                new ProjectInfo("nunit.mocks.tests", 2, 42, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 7, 32, 0, 0),
                new ProjectInfo("nunit.util.tests", 26, 211, 1, 0),
                new ProjectInfo("nunit-console.tests", 3, 34, 0, 0),
                new ProjectInfo("nunit-gui.tests", 6, 52, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.framework.tests" );

        public static ExpectedTestData NUnit_2_4_8 = new ExpectedTestData(
            @"..\..\TestResult-2.4.8.xml",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 45, 426, 0, 0),
                new ProjectInfo("nunit.core.tests", 51, 374, 0, 2),
                new ProjectInfo("nunit.util.tests", 30, 256, 0, 0),
                new ProjectInfo("nunit.mocks.tests", 2, 45, 0, 0),
                new ProjectInfo("nunit.extensions.tests", 9, 68, 0, 0),
                new ProjectInfo("nunit-console.tests", 3, 36, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 10, 40, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0),
                new ProjectInfo("nunit.fixtures.tests", 2, 6, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData NUnit_2_5_2 = new ExpectedTestData(
            @"..\..\TestResult-2.5.2.xml",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 172, 1322, 0, 0),
                new ProjectInfo("nunit.core.tests", 81, 658, 0, 2),
                new ProjectInfo("nunit.util.tests", 33, 287, 0, 0),
                new ProjectInfo("nunit.mocks.tests", 2, 46, 0, 0),
                new ProjectInfo("nunit-console.tests", 3, 36, 0, 0),
                new ProjectInfo("nunit.uiexception.tests", 31, 207, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 11, 50, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0),
                new ProjectInfo("nunit.fixtures.tests", 2, 6, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");
    }
}
