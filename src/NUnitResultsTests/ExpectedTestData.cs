using System;

namespace NUnit.Extras.Tests
{
    public class ExpectedTestData
    {
        private string resultFile;
        private string[] projects;
        private int fixtureCount;
        private int testCount;
        private string fixtureCheck;

        public ExpectedTestData(
            string resultFile,
            string[] projects,
            int fixtureCount,
            int testCount,
            string fixtureCheck)
        {
            this.resultFile = resultFile;
            this.projects = projects;
            this.fixtureCount = fixtureCount;
            this.testCount = testCount;
            this.fixtureCheck = fixtureCheck;
        }

        public string ResultFile
        {
            get { return resultFile; }
        }

        public string[] Projects
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

        public static ExpectedTestData NUnit_2_2_10 = new ExpectedTestData(
            @"..\..\TestResult-2.2.10.xml",
            new string[] {
                "nunit.extensions.tests",
                "nunit.framework.tests",
                "nunit.mocks.tests",
                "nunit.uikit.tests",
                "nunit.util.tests",
                "nunit-console.tests",
                "nunit-gui.tests" },
            98,
            776,
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.framework.tests");

        public static ExpectedTestData NUnit_2_4_8 = new ExpectedTestData(
        @"..\..\TestResult-2.4.8.xml",
        new string[] {
                "nunit.framework.tests",
                "nunit.core.tests",
                "nunit.util.tests",
                "nunit.mocks.tests",
                "nunit.extensions.tests",
                "nunit-console.tests",
                "nunit.uikit.tests",
                "nunit-gui.tests",
                "nunit.fixtures.tests" },
        159,
        1266,
        "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");
    }
}
