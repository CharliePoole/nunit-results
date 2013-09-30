// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Collections.Generic;

namespace NUnit.Extras
{
	/// <summary>
	/// Base Class for all analyzers. Represents a set of counts
	/// (declared in derived classes) and a set of methods for analyzing 
	/// nested analyzers.
	/// </summary>
	public class TestResultAnalyzer : Analyzer
	{
		#region Properties

		public TestResult TestResult { get; private set; }
        private List<TestResult> testCaseResults = new List<TestResult>();
        public IList<TestResult> TestCaseResults { get { return testCaseResults; } }
		public int TestCount { get; private set; }
        public int NotRunCount { get; private set; }
        public int InconclusiveCount { get; private set; }
		public int FailureCount { get; private set; }

		#endregion

		#region Construction

		public TestResultAnalyzer( string name ) : base( name ) { }

		public TestResultAnalyzer( TestResult result ) : this( result.Name, result ) { }

		public TestResultAnalyzer( string name, TestResult result ) : base( name )
		{
			this.TestResult = result;
		}

		public void FindFixtures( TestResult result )
		{
			bool hasTestCases = false;
			bool hasTestSuites = false;

			foreach( TestResult childResult in result.Results )
			{
				if ( childResult.IsTestCase || IsParameterizedTestMethod(childResult) )
					hasTestCases = true;
				else
					hasTestSuites = true;
			}

			if ( hasTestCases )
				Children.Add( new TestResultAnalyzer( result ) );
			else if ( hasTestSuites )
				foreach( TestResult childResult in result.Results )
					if ( childResult.IsSuite )
						FindFixtures(childResult );
           
		}

        private bool IsParameterizedTestMethod(TestResult result)
        {
            foreach (TestResult child in result.Results)
                if ( child.IsTestCase && child.Name.EndsWith(")") )
                    return true;

            return false;
        }

		#endregion

		#region Public Methods

		/// <summary>
		/// Perform required analysis. Default implementation
		/// analyzes any children and summarizes the results.
		/// </summary>
		public override void Analyze()
		{
			InitializeCounts();
			TestCaseResults.Clear();

			if ( TestResult != null )
				SummarizeTestResults( TestResult );
			else
				SummarizeChildren();
		}

		public override void InitializeCounts()
		{
			this.TestCount = 0;
			this.NotRunCount = 0;
			this.FailureCount = 0;
            this.InconclusiveCount = 0;
		}

		private void SummarizeTestResults( TestResult result )
		{
			if ( result.IsTestCase )
			{
				TestCaseResults.Add( result );
			
				++this.TestCount;

                switch (result.ResultState.Status)
                {
                    case TestStatus.Skipped:
    					++this.NotRunCount;
                        break;
                    case TestStatus.Failed:
                        ++this.FailureCount;
                        break;
                    case TestStatus.Inconclusive:
                        ++this.InconclusiveCount;
                        break;
                }
			}
			else
			{
				if ( result.IsSuite )
					foreach( TestResult childResult in result.Results )
						SummarizeTestResults( childResult );
			}
		}

		private void SummarizeChildren()
		{
			foreach( TestResultAnalyzer child in children )
			{
				child.Analyze();

				this.TestCount += child.TestCount;
				this.NotRunCount += child.NotRunCount;
				this.FailureCount += child.FailureCount;
                this.InconclusiveCount += child.InconclusiveCount;

                foreach (TestResult result in child.TestCaseResults)
				    TestCaseResults.Add( result );
			}
		}

		#endregion
	}
}
