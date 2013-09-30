// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

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
        public int InconclusiveCount;

        public ProjectInfo(string name, int fixtureCount, int testCount, int failureCount, int notRunCount, int inconclusiveCount)
        {
            this.Name = name;
            this.FixtureCount = fixtureCount;
            this.TestCount = testCount;
            this.FailureCount = failureCount;
            this.NotRunCount = notRunCount;
            this.InconclusiveCount = inconclusiveCount;
        }
    }

    public class ExpectedTestData
    {
        public ExpectedTestData(
            string resultFile,
            string projectFile,
            ProjectInfo[] projects,
            string fixtureCheck)
        {
            this.ResultFile = resultFile;
            this.ProjectFile = projectFile;
            this.Projects = projects;
            this.FixtureCheck = fixtureCheck;

            foreach (ProjectInfo project in projects)
            {
                this.FixtureCount += project.FixtureCount;
                this.TestCount += project.TestCount;
                this.FailureCount += project.FailureCount;
                this.NotRunCount += project.NotRunCount;
                this.InconclusiveCount += project.InconclusiveCount;
            }
        }

        public string ResultFile { get; private set; }
        public string ProjectFile { get; private set; }
        public ProjectInfo[] Projects { get; private set; }
        public int FixtureCount { get; private set; }
        public int TestCount { get; private set; }
        public string FixtureCheck { get; private set; }
        public int FailureCount { get; private set; }
        public int NotRunCount { get; private set; }
        public int InconclusiveCount { get; private set; }

        public static ExpectedTestData NUnit_2_2_10 = new ExpectedTestData(
            @"..\..\TestResult-2.2.10.xml",
            @"C:\Program Files\NUnit-Net-2.0 2.2.10\bin\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.extensions.tests", 3, 8, 0, 0, 0),
                new ProjectInfo("nunit.framework.tests", 51, 397, 0, 2, 0),
                new ProjectInfo("nunit.mocks.tests", 2, 42, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 7, 32, 0, 0, 0),
                new ProjectInfo("nunit.util.tests", 26, 211, 1, 0, 0),
                new ProjectInfo("nunit-console.tests", 3, 34, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 6, 52, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.framework.tests" );

        public static ExpectedTestData NUnit_2_4_8 = new ExpectedTestData(
            @"..\..\TestResult-2.4.8.xml",
            @"C:\Program Files\NUnit 2.4.8\bin\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 45, 426, 0, 0, 0),
                new ProjectInfo("nunit.core.tests", 51, 374, 0, 2, 0),
                new ProjectInfo("nunit.util.tests", 30, 256, 0, 0, 0),
                new ProjectInfo("nunit.mocks.tests", 2, 45, 0, 0, 0),
                new ProjectInfo("nunit.extensions.tests", 9, 68, 0, 0, 0),
                new ProjectInfo("nunit-console.tests", 3, 36, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 10, 40, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0, 0),
                new ProjectInfo("nunit.fixtures.tests", 2, 6, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData NUnit_2_5_2 = new ExpectedTestData(
            @"..\..\TestResult-2.5.2.xml",
            @"C:\Program Files\NUnit 2.5.2\bin\net-2.0\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 172, 1322, 0, 0, 0),
                new ProjectInfo("nunit.core.tests", 81, 658, 7, 2, 0),
                new ProjectInfo("nunit.util.tests", 33, 287, 0, 0, 0),
                new ProjectInfo("nunit.mocks.tests", 2, 46, 0, 0, 0),
                new ProjectInfo("nunit-console.tests", 3, 36, 0, 0, 0),
                new ProjectInfo("nunit.uiexception.tests", 31, 207, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 11, 50, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0, 0),
                new ProjectInfo("nunit.fixtures.tests", 2, 6, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData NUnit_2_5_10 = new ExpectedTestData(
            @"..\..\TestResult-2.5.10.xml",
            @"C:\Program Files\NUnit 2.5.10\bin\net-2.0\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 183, 1475, 0, 0, 0),
                new ProjectInfo("nunit.core.tests", 98, 842, 0, 2, 4),
                new ProjectInfo("nunit.util.tests", 37, 324, 0, 0, 9),
                new ProjectInfo("nunit.mocks.tests", 2, 46, 0, 0, 0),
                new ProjectInfo("nunit-console.tests", 4, 49, 0, 0, 0),
                new ProjectInfo("nunit.uiexception.tests", 31, 208, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 11, 43, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 14, 0, 0, 0),
                new ProjectInfo("nunit.fixtures.tests", 1, 2, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData NUnit_2_6_0 = new ExpectedTestData(
            @"..\..\TestResult-2.6.0.xml",
            @"C:\Program Files\NUnit 2.6\bin\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 186, 1528, 0, 0, 0),
                new ProjectInfo("nunit.core.tests", 100, 886, 0, 8, 8),
                new ProjectInfo("nunit.util.tests", 38, 327, 0, 0, 9),
                new ProjectInfo("nunit.mocks.tests", 2, 46, 0, 0, 0),
                new ProjectInfo("nunit-console.tests", 4, 69, 0, 0, 0),
                new ProjectInfo("nunit.uiexception.tests", 31, 208, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 10, 40, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0, 0),
                new ProjectInfo("nunit-editor.tests", 15, 131, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData NUnit_2_6_2 = new ExpectedTestData(
            @"..\..\TestResult-2.6.2.xml",
            @"C:\Program Files\NUnit 2.6\bin\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("nunit.framework.tests", 187, 1545, 0, 0, 0),
                new ProjectInfo("nunit.core.tests", 103, 1091, 0, 8, 10),
                new ProjectInfo("nunit.util.tests", 38, 327, 0, 0, 9),
                new ProjectInfo("nunit.mocks.tests", 2, 46, 0, 0, 0),
                new ProjectInfo("nunit-console.tests", 4, 76, 0, 0, 0),
                new ProjectInfo("nunit.uiexception.tests", 31, 208, 0, 0, 0),
                new ProjectInfo("nunit.uikit.tests", 10, 49, 0, 0, 0),
                new ProjectInfo("nunit-gui.tests", 3, 15, 0, 0, 0),
                new ProjectInfo("nunit-editor.tests", 15, 131, 0, 0, 0) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");

        public static ExpectedTestData MockAssembly_2_6_2 = new ExpectedTestData(
            @"..\..\mock-assembly-2.6.2.xml",
            @"C:\Program Files\NUnit 2.6\bin\NUnitTests.nunit",
            new ProjectInfo[] {
                new ProjectInfo("mock-assembly", 11, 21, 2, 7, 1) },
            "NUnit.Core.Tests.PlatformDetectionTests,nunit.core.tests");
    }
}
