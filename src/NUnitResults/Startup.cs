// *****************************************************
// Copyright 2008, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;

namespace NUnit.Extras
{
	/// <summary>
	/// Startup for the TestResults applciation.
	/// </summary>
	public class Startup
	{
		[STAThread]
		static void Main( string[] args )
		{
			string testResultPath;
			string outputDirectory;

            try
            {
                if (args.Length < 1)
                {
                    Error("No arguments provided");
                    return;
                }

                if (args.Length > 2)
                {
                    Error("Too many arguments");
                    return;
                }

                if (args[0] == "?" || args[0] == "/?")
                {
                    Usage();
                    return;
                }

                testResultPath = args.Length > 0 ? args[0] : "TestResult.xml";
                outputDirectory = args.Length > 1 ? args[1] : "TestResults";

                TestResultReport report = new TestResultReport(testResultPath, outputDirectory);

                report.DoReport();
            }
            catch (FileNotFoundException exception)
            {
                Error(exception.Message);
            }
            catch (ApplicationException exception)
            {
                Error(exception.Message);
            }
            catch (Exception exception)
            {
                Error(exception.ToString());
            }
		}

		/// <summary>
		/// Write usage help message to console
		/// </summary>
		static void Usage()
		{
			Console.WriteLine( "Usage: TESTRESULTS xml-results [ output-dir ]\n" );
			
			Console.WriteLine( "Where\txml-results\t= Path to a test result xml file (default: TestResult.xml)" );
			Console.WriteLine( "\toutput-dir\t= Directory to receive the generated report files (default: TestResults)\n" );

			Console.WriteLine( "Wildcards may be used in specifying the result file in order to combine" );
			Console.WriteLine( "the contents of multiple files. If a directory is specified, all xml" );
			Console.WriteLine( "files in that directory are combined.\n" );
		}

		/// <summary>
		/// Write an error message followed by usage help
		/// </summary>
		/// <param name="message">The message to write</param>
		static void Error(string message)
		{
			Console.WriteLine( message );
			Console.WriteLine();
			Usage();
		}
	}
}
