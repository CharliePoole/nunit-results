NUnitResults 1.0 - January 18, 2008

NUnitResults is a console application, which produces web reports from the output of 
NUnit test runs. It is part of the NUnitExtras suite.

COPYRIGHT AND LICENSE

NUnitResults is CopyRight © 2008, Charlie Poole
and is licensed under the Open Software License version 3.0

A copy of the license is distributed with the program in the file LICENSE.txt and is 
also available at http://www.opensource.org/licenses/osl-3.0.

INSTALLATION

Copy the NUnitResults.exe file together with the included nunit.core.dll and 
nunit.util.dll into any convenient directory. Note that the NUnit assemblies are 
part of the NUnit 2.2.10 distribution, and are required for the program to execute 
correctly.

USAGE

Execute from the command line as follows:

	NUnitResults [ <result-file> [ <output-dir> ] ]
			
Where <result-file> is the path to a test result xml file produced by NUnit and 
<output-dir> is the directory to receive the generated report files. If not provided, 
the input file defaults to TestResults.xml and the output directory to TestResults.

Wildcards may be used in specifying the result file in order to combine the contents of 
multiple files. If a directory is specified, rather than a file, all xml files in that 
directory are combined.

Input files from any 2.x version of NUnit are supported.