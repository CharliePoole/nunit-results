// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;

namespace NUnit.Extras.Tests
{
    public class TestUtil
    {
        public static int CountTestCases(TestResult result)
        {
            if (result.IsTestCase)
                return 1;

            int sum = 0;
            foreach (TestResult child in result.Results)
                sum += CountTestCases(child);
            return sum;
        }

        public static int CountTestFixtures(TestResult result)
        {
            if (IsFixture(result))
                return 1;

            int sum = 0;
            foreach (TestResult child in result.Results)
                sum += CountTestFixtures(child);

            return sum;
        }

        public static bool IsFixture(TestResult result)
        {
            if (result.Results.Count == 0)
                return true; // Empty suite can only be a fixture

            bool hasTestCases = false;

            foreach (TestResult child in result.Results)
            {
                if (child.IsTestCase)
                    hasTestCases = true;
                else if (IsParameterizedTestMethod(child))
                    return true;
            }
                
            return hasTestCases;
        }

        public static bool IsParameterizedTestMethod(TestResult result)
        {
            foreach(TestResult child in result.Results)
                if ( child.IsTestCase && child.Name.EndsWith(")"))
                    return true;

            return false;
        }

        public static TestResult FindChildResult(TestResult result, string name)
        {
            foreach (TestResult child in result.Results)
                if (child.Name == name) return child;

            //Assert.Fail("Unable to locate {0} under test result (1)", name, result.Name);

            return null;
        }
    }
}
