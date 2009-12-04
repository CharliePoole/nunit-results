// *****************************************************
// Copyright 2008, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using System.Web;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using NUnit.Core;

namespace NUnit.Extras
{
	class TestResultReport
	{
		/// <summary>
		/// Location of the test result file to be analyzed
		/// </summary>
		private string testResultPath;

		/// <summary>
		/// Location for storing the html output files
		/// </summary>
		private string outputDirectory;

		/// <summary>
		/// Top level html page for summary info and links to other pages
		/// </summary>
		private HtmlPage indexPage;
		
		/// <summary>
		/// Loader for examining the xml result file
		/// </summary>
		private TestResultLoader resultLoader = new TestResultLoader();

		/// <summary>
		/// Top-level counters of test results
		/// </summary>
		private int testCount;
		private int notRunCount;
		private int failureCount;

		/// <summary>
		/// Construct a TestResultReport object
		/// </summary>
		/// <param name="testResultPath">Location of the test result xml file to be analyzed</param>
		/// <param name="outputDirectory">Location for storing the html output</param>
		public TestResultReport( string testResultPath, string outputDirectory )
		{
			this.testResultPath = testResultPath;
			this.outputDirectory = outputDirectory;
		}

		/// <summary>
		/// Perform top-level analysis and produce standard reports
		/// </summary>
		public void DoReport()
		{
			if( !Directory.Exists( outputDirectory ) )
				Directory.CreateDirectory( outputDirectory );

			string title = string.Format( "{0} Test Results", Path.GetFileNameWithoutExtension( testResultPath ) );
			indexPage = new HtmlPage( Path.Combine( outputDirectory, "index.html" ) );
			indexPage.Begin( title );

			try
			{
				resultLoader.Load( testResultPath );
			}
			catch ( ApplicationException ex )
			{
				indexPage.WriteLine( ex.Message );
				throw;
			}
			catch ( Exception ex )
			{
				indexPage.WriteLine( ex.ToString() );
				throw;
			}

			InitializeProjectIndex();

			testCount = 0;
			notRunCount = 0;
			failureCount = 0;
			

			foreach( TestSuiteResult suiteResult in resultLoader.ProjectResults )
			{
				string projectName = Path.GetFileNameWithoutExtension( suiteResult.Name );
				Console.WriteLine( "Analyzing {0}", projectName );

				TestResultAnalyzer analyzer = new TestResultAnalyzer( projectName );
				analyzer.FindFixtures( suiteResult );
				analyzer.Analyze();

				testCount += analyzer.TestCount;
				notRunCount += analyzer.NotRunCount;
				failureCount += analyzer.FailureCount;

				WriteIndexEntry( analyzer );
				WriteDetailPage( analyzer );
			}

			TerminateProjectIndex();

			indexPage.End();

			Console.WriteLine( "Analysis Complete" );
			Console.WriteLine( "Report created in directory {0}", Path.GetFullPath( outputDirectory ) );
		}

		private void InitializeProjectIndex()
		{
			indexPage.BeginTable("class=center");
			indexPage.BeginRow( "hdr-row" );
			
			indexPage.WriteHeaderCell( "Component" );
			indexPage.WriteHeaderCell( "Tests" );
			indexPage.WriteHeaderCell( "Not&nbsp;Run" );
			indexPage.WriteHeaderCell( "Failures" );
			indexPage.EndRow();
		}

		private void TerminateProjectIndex()
		{
			indexPage.BeginRow( "total-row" );
			indexPage.WriteCell( "Total" );
			indexPage.WriteCell( testCount );
			indexPage.WriteCell( notRunCount );
			indexPage.WriteCell( failureCount );
			indexPage.EndRow();

			indexPage.EndTable();
		}

		private void WriteIndexEntry( TestResultAnalyzer analyzer )
		{
			if ( analyzer.FailureCount > 0 )
				indexPage.BeginRow( "failure-row" );
			else
			if ( analyzer.NotRunCount > 0 )
				indexPage.BeginRow( "notrun-row" );
			else
				indexPage.BeginRow();
			
			string link = string.Format( "<a href=\"{0}.html\">{0}</a>", analyzer.Name );
			indexPage.WriteCell( link, "name" );
			indexPage.WriteCell( analyzer.TestCount );
			indexPage.WriteCell( analyzer.NotRunCount );
			indexPage.WriteCell( analyzer.FailureCount );
			indexPage.EndRow();
		}

		private void WriteDetailPage( TestResultAnalyzer analyzer )
		{
			HtmlPage page = new HtmlPage( Path.Combine( outputDirectory, analyzer.Name + ".html" ) );
			
			WriteDetailTable( page, analyzer );
			
			if ( analyzer.FailureCount > 0 )
				WriteFailureTable( page, analyzer );
			
			if ( analyzer.NotRunCount > 0 )
				WriteNotRunTable( page, analyzer );

			page.End();
		}

		private void WriteDetailTable( HtmlPage page, TestResultAnalyzer analyzer )
		{
			StringBuilder sbFailures = new StringBuilder();
			StringBuilder sbNotRun = new StringBuilder();

			page.Begin( analyzer.Name );
			page.BeginTable( "class=center" );

			page.BeginRow( "hdr-row" );
			page.WriteHeaderCell( "Class", "name" );
			page.WriteHeaderCell( "Tests" );
			page.WriteHeaderCell( "Not&nbsp;Run" );
			page.WriteHeaderCell( "Failures" );
			page.EndRow();

			foreach( TestResultAnalyzer classAnalyzer in analyzer.Children )
			{
				if ( classAnalyzer.FailureCount > 0 )
					page.BeginRow( "failure-row" );
				else
				if ( classAnalyzer.NotRunCount > 0 )
					page.BeginRow( "notrun-row" );
				else
					page.BeginRow();

				page.WriteCell( classAnalyzer.Name, "name" );
				page.WriteCell( classAnalyzer.TestCount );
				page.WriteCell( classAnalyzer.NotRunCount );
				page.WriteCell( classAnalyzer.FailureCount );
				page.EndRow();
			}

			page.BeginRow( "total-row" );
			page.WriteCell( "Total", "name" );
			page.WriteCell( analyzer.TestCount );
			page.WriteCell( analyzer.NotRunCount );
			page.WriteCell( analyzer.FailureCount );
			page.EndRow();
			page.EndTable();
		}

		private void WriteFailureTable( HtmlPage page, TestResultAnalyzer analyzer )
		{
			page.BeginTable( "class=center" );
			page.BeginRow( "failure-row" );
			page.WriteHeaderCell( "Failures", 2);
			page.EndRow();
			
			int count = 0;
			foreach( TestCaseResult result in analyzer.TestCaseResults )
			{
				if ( result.IsFailure )
				{
					page.BeginRow();
					page.WriteCell( ++count );
					StringBuilder sb = new StringBuilder( TruncateTestName(result.Name) );
					sb.Append( "<br>" );
					sb.Append( HttpUtility.HtmlEncode( result.Message ) );
					string[] stack = result.StackTrace.Split( new char[] { '\n' } );
					foreach( string line in stack )
					{
						sb.Append( "<p>" );
						sb.Append( HttpUtility.HtmlEncode( line ) );
					}
					page.WriteCell( sb.ToString(), "text" );
					page.EndRow();
				}
			}

			page.EndTable();
		}

		private void WriteNotRunTable( HtmlPage page, TestResultAnalyzer analyzer )
		{			
			page.BeginTable( "class=center" );
			page.BeginRow( "notrun-row" );
			page.WriteHeaderCell( "Not&nbsp;Run", 2 );
			page.EndRow();

			int count = 0;
			foreach( TestCaseResult result in analyzer.TestCaseResults )
			{
				if ( !result.Executed )
				{
					page.BeginRow();
					page.WriteCell( ++count );
					page.WriteCell( TruncateTestName(result.Name) + "<br>" + HttpUtility.HtmlEncode( result.Message ), "text" );
					page.EndRow();
				}
			}

			page.EndTable();
		}

        private string TruncateTestName(string testName)
        {
            int lpar = -1;

            if ( testName.EndsWith(")"))
            {
                int nest = 1;
                int pos = testName.Length - 2;

                while (nest > 0 && pos >= 0)
                {
                    char c = testName[pos--];
                    switch (c)
                    { 
                        case ')':
                            nest++;
                            break;
                        case '(':
                            nest--;
                            break;
                        default:
                            break;
                    }
                }

                if (nest == 0)
                    lpar = pos + 1;
            }

            //int lpar = testName.IndexOf('(');

            int dot = lpar < 0
                ? testName.LastIndexOf('.')
                : testName.LastIndexOf('.', lpar - 1, lpar);

            if ( dot > 0 )
            {
                int dot2 = testName.LastIndexOf('.', dot - 1, dot);
                if ( dot2 >= 0 ) dot = dot2;
            }

            if (dot >= 0) testName = testName.Substring(dot + 1);

            return testName;
        }

        private void WriteMissingProjectsTable(HtmlPage page, ArrayList missingProjects)
		{
			page.BeginTable();
			
			page.BeginRow( "hdr-row" );
			page.WriteHeaderCell( "Missing&nbsp;Projects"  );
			page.EndRow();

			foreach( TestResultAnalyzer analyzer in missingProjects )
			{
				page.BeginRow();
				page.WriteCell( analyzer.Name, "name" );
				page.EndRow();
			}

			page.EndTable();
		}

	}
}