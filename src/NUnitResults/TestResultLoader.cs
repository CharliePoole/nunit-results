// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Xml;
using System.Collections;
using NUnit.Core;
//using NUnit.Util;

namespace NUnit.Extras
{
	/// <summary>
	/// Summary description for TestResultLoader.
	/// </summary>
    public class TestResultLoader
    {
        #region Private Fields

        private int assemblyKey = -1;

        private TestSuiteResult topLevelResult;

        #endregion

        #region Properties

        public TestSuiteResult TopLevelResult
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

        private class DummyTestCase : Test
        {
            public DummyTestCase(string testName, int assemblyKey)
                : base(testName, assemblyKey) { }


            public override bool IsFixture
            {
                get
                {
                    return false;
                }
            }

            public override bool IsTestCase
            {
                get
                {
                    return true;
                }
            }

            public override ArrayList Tests
            {
                get
                {
                    return null;
                }
            }

            public override int CountTestCases()
            {
                return 1;
            }

            public override int CountTestCases(IFilter filter)
            {
                return 1;
            }

            public override bool Filter(IFilter filter)
            {
                return true;
            }

            public override bool IsSuite
            {
                get
                {
                    return false;
                }
            }

            public override TestResult Run(EventListener listener)
            {
                return null;
            }

            public override TestResult Run(EventListener listener, IFilter filter)
            {
                return null;
            }




        }

        public void Load(string fileSpec)
        {
            if (Directory.Exists(fileSpec))
                topLevelResult = LoadFiles(Path.Combine(fileSpec, "*.xml"));
            else if (fileSpec.IndexOfAny(new char[] { '*', '?' }) >= 0)
                topLevelResult = LoadFiles(fileSpec);
            else
                topLevelResult = LoadFile(fileSpec);
        }

        private TestSuiteResult LoadFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNode topLevelNode = doc.SelectSingleNode("/test-results");
            //string topLevelName = GetTestName( topLevelNode );
            TestSuite fileSuite = new TestSuite(fileName);
            TestSuiteResult fileResult = new TestSuiteResult(fileSuite, fileName);

            XmlNode suiteNode = topLevelNode.SelectSingleNode("test-suite");
            TestSuiteResult suiteResult = LoadSuite(suiteNode);

            fileResult.Results.Add(suiteResult);

            return fileResult;
        }

        private TestSuiteResult LoadFiles(string pattern)
        {
            TestSuite topLevelSuite = new TestSuite(pattern);
            TestSuiteResult topLevelResult = new TestSuiteResult(topLevelSuite, pattern);

            string dir = Path.GetDirectoryName(pattern);
            string pat = Path.GetFileName(pattern);

            foreach (string fileName in Directory.GetFiles(dir, pat))
            {
                topLevelResult.Results.Add(LoadFile(fileName));
            }

            return topLevelResult;
        }

        public TestSuiteResult FindProjectResult(string name)
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
            TestSuiteResult suiteResult = testResult as TestSuiteResult;
            if (suiteResult == null) return;

            string resultName = suiteResult.Name.ToLower();
            string extension = Path.GetExtension(resultName);

            switch (extension)
            {
                case ".dll":
                case ".exe":
                    list.Add(suiteResult);
                    break;
                case "":
                    break;
                default:
                    foreach (TestResult childResult in suiteResult.Results)
                    {
                        TestSuiteResult childSuiteResult = childResult as TestSuiteResult;
                        if (childSuiteResult != null)
                            AccumulateProjects(list, childSuiteResult);
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

        private static TestSuiteResult FindProjectResult(string name, TestResult testResult)
        {
            if (testResult == null) return null;

            TestSuiteResult suiteResult = testResult as TestSuiteResult;
            if (suiteResult == null) return null;

            string resultName = suiteResult.Name.ToLower();
            string extension = Path.GetExtension(resultName);

            switch (extension)
            {
                case ".dll":
                case ".exe":
                    if (name.ToLower() == Path.GetFileNameWithoutExtension(resultName))
                        return suiteResult;
                    break;
                case "":
                    break;
                default:
                    foreach (TestResult childResult in suiteResult.Results)
                    {
                        TestSuiteResult childSuiteResult = childResult as TestSuiteResult;
                        if (childSuiteResult != null)
                        {
                            TestSuiteResult result = FindProjectResult(name, childSuiteResult);
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

            TestSuiteResult testSuiteResult = result as TestSuiteResult;
            if (testSuiteResult != null)
                foreach (TestResult childResult in testSuiteResult.Results)
                {
                    TestResult tempResult = FindTestResult(name, childResult);
                    if (tempResult != null) return tempResult;
                }

            return null;
        }

        private TestSuiteResult LoadSuite(XmlNode xmlNode)
        {
            string suiteName = GetTestName(xmlNode);

            if (suiteName.EndsWith(".dll") || suiteName.EndsWith(".exe"))
                ++assemblyKey;

            TestSuite suite = new TestSuite(suiteName, assemblyKey);
            TestSuiteResult result = new TestSuiteResult(suite, suiteName);

            result.Executed = true;

            XmlNode resultsNode = xmlNode.SelectSingleNode("results");

            if (resultsNode != null)
            {
                foreach (XmlNode childNode in resultsNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "test-suite":
                            TestSuiteResult childResult = LoadSuite(childNode);
                            if (childResult.Name != result.Name)
                                result.Results.Add(childResult);
                            else
                                foreach (TestResult grandChildResult in childResult.Results)
                                    result.Results.Add(grandChildResult);
                            break;
                        case "test-case":
                            TestCaseResult testCaseResult = LoadTestCase(suiteName, childNode);
                            result.Results.Add(testCaseResult);
                            break;
                        default:
                            break;
                    }
                }
            }

            //result.IsSuccess = GetSuccess( xmlNode );
            result.Time = GetTime(xmlNode);

            return result;
        }

        private TestCaseResult LoadTestCase(string suiteName, XmlNode xmlNode)
        {
            string testName = GetTestName(xmlNode);
            UITestNode testCase = new UITestNode(new DummyTestCase(testName, assemblyKey));

            TestCaseResult result = new TestCaseResult(testCase);
            if (GetExecuted(xmlNode))
            {
                if (GetSuccess(xmlNode))
                    result.Success();
                else
                {
                    XmlNode messageNode = xmlNode.SelectSingleNode("failure/message");
                    string message = messageNode != null ? messageNode.InnerText : string.Empty;

                    XmlNode stackTraceNode = xmlNode.SelectSingleNode("failure/stack-trace");
                    string stackTrace = stackTraceNode != null ? stackTraceNode.InnerText : string.Empty;

                    result.Failure(message, stackTrace);
                }
            }
            else
            {
                XmlNode reasonNode = xmlNode.SelectSingleNode("reason/message");
                if (reasonNode != null)
                {
                    string reason = reasonNode.InnerText;
                    result.NotRun(reason);
                }
            }

            result.Time = GetTime(xmlNode);

            return result;
        }

        private object GetAttribute(XmlNode node, string name)
        {
            XmlNode attr = node.Attributes[name];
            return attr != null ? attr.Value : null;
        }

        private bool GetBoolean(XmlNode node, string name)
        {
            string trueFalse = GetAttribute(node, name) as string;
            return trueFalse != null ? bool.Parse(trueFalse) : false;
        }

        private string GetTestName(XmlNode node)
        {
            return GetAttribute(node, "name") as string;
        }

        private bool GetExecuted(XmlNode node)
        {
            return GetBoolean(node, "executed");
        }

        private bool GetSuccess(XmlNode node)
        {
            return GetBoolean(node, "success");
        }

        private double GetTime(XmlNode node)
        {
            string doubleString = GetAttribute(node, "time") as string;
            return doubleString != null ? double.Parse(doubleString) : 0.0;
        }
    }
}
