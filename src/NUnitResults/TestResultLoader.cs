// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Xml;
using System.Collections;

namespace NUnit.Extras
{
	/// <summary>
	/// Summary description for TestResultLoader.
	/// </summary>
    public class TestResultLoader
    {
        #region Private Fields

        private TestResult topLevelResult;

        #endregion

        #region Properties

        public TestResult TopLevelResult
        {
            get { return topLevelResult; }
        }

        #endregion

        #region Constructors

        public TestResultLoader() { }

        public TestResultLoader(string fileSpec)
        {
            Load(fileSpec);
        }

        #endregion

        public void Load(string fileSpec)
        {
            if (Directory.Exists(fileSpec))
                topLevelResult = LoadFiles(Path.Combine(fileSpec, "*.xml"));
            else if (fileSpec.IndexOfAny(new char[] { '*', '?' }) >= 0)
                topLevelResult = LoadFiles(fileSpec);
            else
                topLevelResult = LoadFile(fileSpec);
        }

        private TestResult LoadFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNode topLevelNode = doc.SelectSingleNode("/test-results");
            //TestResult fileResult = new TestResult(fileName, true);

            XmlNode suiteNode = topLevelNode.SelectSingleNode("test-suite");
            TestResult suiteResult = new TestResult(suiteNode);

            //fileResult.Results.Add(suiteResult);

            return suiteResult;
        }

        private TestResult LoadFiles(string pattern)
        {
            TestResult topLevelResult = new TestResult(pattern, true);

            string dir = Path.GetDirectoryName(pattern);
            string pat = Path.GetFileName(pattern);

            foreach (string fileName in Directory.GetFiles(dir, pat))
            {
                topLevelResult.Results.Add(LoadFile(fileName));
            }

            return topLevelResult;
        }

        public TestResult FindProjectResult(string name)
        {
            return FindProjectResult(name, topLevelResult);
        }

        public IList ProjectResults
        {
            get
            {
                ArrayList list = new ArrayList();
                AccumulateProjects(list, topLevelResult);
                return list;
            }
        }

        private static void AccumulateProjects(ArrayList list, TestResult testResult)
        {
            if (!testResult.IsSuite) return;

            string resultName = testResult.Name.ToLower();
            string extension = Path.GetExtension(resultName);

            switch (extension)
            {
                case ".dll":
                case ".exe":
                    list.Add(testResult);
                    break;
                case "":
                    break;
                default:
                    foreach (TestResult childResult in testResult.Results)
                    {
                        if (childResult.IsSuite)
                            AccumulateProjects(list, childResult);
                    }
                    break;
            }
        }

        public TestResult FindTestResult(string projectName, string className)
        {
            TestResult projectResult = FindProjectResult(projectName);
            if (projectResult == null) return null;

            return FindTestResult(className, projectResult);
        }

        private static TestResult FindProjectResult(string name, TestResult testResult)
        {
            if (testResult == null) return null;

            if (!testResult.IsSuite) return null;

            string resultName = testResult.Name.ToLower();
            string extension = Path.GetExtension(resultName);

            switch (extension)
            {
                case ".dll":
                case ".exe":
                    if (name.ToLower() == Path.GetFileNameWithoutExtension(resultName))
                        return testResult;
                    break;
                case "":
                    break;
                default:
                    foreach (TestResult childResult in testResult.Results)
                    {
                        if (childResult.IsSuite)
                        {
                            TestResult result = FindProjectResult(name, childResult);
                            if (result != null)
                                return result;
                        }
                    }
                    break;
            }

            return null;
        }

        public static TestResult FindTestResult(string name, TestResult result)
        {
            if (result == null) return null;

            if (result.Name.ToLower() == name.ToLower()) return result;

            if (result.IsSuite)
                foreach (TestResult childResult in result.Results)
                {
                    TestResult tempResult = FindTestResult(name, childResult);
                    if (tempResult != null) return tempResult;
                }

            return null;
        }
    }
}
