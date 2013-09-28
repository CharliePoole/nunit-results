// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Collections;

namespace NUnit.Extras
{
	/// <summary>
	/// Base Class for all analyzers. Represents a set of counts
	/// (declared in derived classes) and a set of methods for analyzing 
	/// nested analyzers.
	/// </summary>
	public class TestResultAnalyzer : Analyzer
	{
		#region Fields

		/// <summary>
		/// The test result we are analyzing
		/// </summary>
		protected TestResult testResult;

		/// <summary>
		/// The number of test cases represented by the result
		/// </summary>
		protected int testCount;

		/// <summary>
		/// The number of test cases not run
		/// </summary>
		protected int notRunCount;

		/// <summary>
		/// The number of test failures
		/// </summary>
		protected int failureCount;

		protected ArrayList testCaseResults = new ArrayList();

		#endregion

		#region Properties

		public TestResult TestResult
		{
			get { return testResult; }
		}

		public IList TestCaseResults
		{
			get { return testCaseResults; }
		}

		public int TestCount
		{
			get { return testCount; }
		}

		public int NotRunCount
		{
			get { return notRunCount; }
		}

		public int FailureCount
		{
			get { return failureCount; }
		}

		#endregion

		#region Construction

		public TestResultAnalyzer( string name ) : base( name ) { }

		public TestResultAnalyzer( TestResult result ) : this( result.Name, result ) { }

		public TestResultAnalyzer( string name, TestResult result ) : base( name )
		{
			this.testResult = result;
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
			testCaseResults.Clear();

			if ( testResult != null )
				SummarizeTestResults( testResult );
			else
				SummarizeChildren();
		}

		public override void InitializeCounts()
		{
			this.testCount = 0;
			this.notRunCount = 0;
			this.failureCount = 0;
		}

		private void SummarizeTestResults( TestResult result )
		{
			if ( result.IsTestCase )
			{
				testCaseResults.Add( result );
			
				++this.testCount;

				if ( !result.Executed ) 
				{
					++this.notRunCount;
				}
				else if ( result.IsFailure ) 
				{
					++this.failureCount;
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

				this.testCount += child.TestCount;
				this.notRunCount += child.NotRunCount;
				this.failureCount += child.FailureCount;

				testCaseResults.AddRange( child.TestCaseResults );
			}
		}

		#endregion
	}
}
