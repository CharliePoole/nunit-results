#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Extras
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// The TestResult abstract class represents
    /// the result of a test and is used to
    /// communicate results across AppDomains.
    /// </summary>
    public class TestResult
    {
        #region Constructors

        public TestResult(string name, bool isSuite)
        {
            this.Name = name;
            this.IsSuite = isSuite;
            this.Results = new List<TestResult>();
        }

        public TestResult(XmlNode thisNode)
        {
            this.Name = GetAttribute(thisNode, "name");
            this.Results = new List<TestResult>();

            switch (thisNode.Name)
            {
                case "test-results":
                    this.IsSuite = true;

                    XmlNode suiteNode = thisNode.SelectSingleNode("test-suite");
                    TestResult suiteResult = new TestResult(suiteNode);

                    this.Results.Add(suiteResult);
                    return;

                case "test-suite":
                    this.IsSuite = true;
                    break;

                case "test-case":
                    this.IsSuite = false;
                    break;
            }

            // Common processing for test-case and test-suite
            this.Executed = GetBoolean(thisNode, "executed");
            this.Time = GetDouble(thisNode, "time");

            if (Executed)
            {
                if (GetBoolean(thisNode, "success"))
                    this.Success();
                else
                {
                    XmlNode messageNode = thisNode.SelectSingleNode("failure/message");
                    string message = messageNode != null ? messageNode.InnerText : string.Empty;

                    XmlNode stackTraceNode = thisNode.SelectSingleNode("failure/stack-trace");
                    string stackTrace = stackTraceNode != null ? stackTraceNode.InnerText : string.Empty;

                    this.Failure(message, stackTrace);
                }
            }
            else
            {
                XmlNode reasonNode = thisNode.SelectSingleNode("reason/message");
                if (reasonNode != null)
                {
                    string reason = reasonNode.InnerText;
                    this.NotRun(reason);
                }
            }

            if (IsSuite)
            {
                XmlNode resultsNode = thisNode.SelectSingleNode("results");

                if (resultsNode != null)
                    foreach (XmlNode childNode in resultsNode.ChildNodes)
                        this.Results.Add(new TestResult(childNode));
            }
        }

        #endregion

        #region Properties

        public bool Executed { get; set; }

        public virtual string Name { get; private set; }

        public virtual bool IsSuccess { get { return !IsFailure; } }

        public bool IsSuite { get; private set; }

        public bool IsTestCase { get { return !IsSuite; } }

        public virtual bool IsFailure { get; set; }

        public virtual string Description { get; private set; }

        public double Time { get; set; }

        public string Message { get; private set; }

        public virtual string StackTrace { get; set; }

        public int AssertCount { get; private set; }

        public List<TestResult> Results { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Mark the test as running successfully
        /// </summary>
        public void Success()
        {
            Executed = true;
            IsFailure = false;
        }

        /// <summary>
        /// Mark the test as not run.
        /// </summary>
        /// <param name="reason">The reason the test was not run</param>
        public void NotRun(string reason)
        {
            NotRun(reason, null);
        }

        /// <summary>
        /// Mark the test as not run.
        /// </summary>
        /// <param name="reason">The reason the test was not run</param>
        /// <param name="stackTrace">Stack trace giving the location of the command</param>
        public void NotRun(string reason, string stackTrace)
        {
            this.Executed = false;
            this.Message = reason;
            this.StackTrace = stackTrace;
        }

        /// <summary>
        /// Mark the test as a failure due to an
        /// assertion having failed.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="stackTrace">Stack trace giving the location of the failure</param>
        public void Failure(string message, string stackTrace)
        {
            this.Executed = true;
            this.IsFailure = true;
            this.Message = message;
            this.StackTrace = stackTrace;
        }

        #endregion

        #region Helper Methods

        private static string GetAttribute(XmlNode node, string name)
        {
            XmlNode attr = node.Attributes[name];
            return attr != null ? attr.Value : null;
        }

        private static bool GetBoolean(XmlNode node, string name)
        {
            string trueFalse = GetAttribute(node, name);
            return trueFalse != null ? bool.Parse(trueFalse) : false;
        }

        private static double GetDouble(XmlNode node, string name)
        {
            string doubleString = GetAttribute(node, name);
            return doubleString != null ? double.Parse(doubleString) : 0.0;
        }

        #endregion
    }
}
