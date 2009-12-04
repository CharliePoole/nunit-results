NUnitResults 1.1 - December 4, 2009

NUnitResults is a console application used to produce HTML reports from the 
output of NUnit test runs. It is part of the NUnit.Extras suite.

The program was based on an earlier reporting program created for a client
application by Charlie Poole. Application-specific features were removed to
create the first public release in January, 2008.

COPYRIGHT AND LICENSE

NUnitResults is CopyRight © 2008-2009, Charlie Poole
and is licensed under the Open Software License version 3.0

A copy of the license is distributed with the program in the file LICENSE.txt
and is also available at http://www.opensource.org/licenses/osl-3.0.

INSTALLATION

Copy the NUnitResults.exe file together with the included nunit.core.dll and 
nunit.util.dll into any convenient directory. Note that the NUnit assemblies
are part of the NUnit 2.2.10 distribution, and are required for the program 
to execute correctly.

USAGE

Execute from the command line as follows:

	NUnitResults [ <result-file> [ <output-dir> ] ]
			
Where <result-file> is the path to a test result xml file produced by NUnit 
and <output-dir> is the directory to receive the generated report files. If 
not provided, the input file defaults to TestResults.xml and the output 
directory to TestResults.

Wildcards may be used in specifying the result file in order to combine the 
contents of multiple files. If a directory is specified, rather than a file, 
all xml files in that directory are combined.

Input files from any 2.x version of NUnit are supported.

RELEASE HISTORY

January, 2008  - NUnitResults 1.0 released on the NUnit Site. 
November, 2009 - Moved to Launchpad, NUnitResults 1.0 re-released.
December, 2009 - Changed name to NUnit-Results for 1.1 release

The file CHANGES.txt lists specific changes for each release.

LIMITATIONS

There are several limitations due to lack of information in the current format
of the xml result file:

1) Results other than success, failure or generic "not run" are not available.
In particular, individual cases of a Theory that fail an assumption are shown 
as failures rather than as inconclusive.

2) If all the test methods in a fixture are parameterized and all are renamed
by the user in such a way that the argument list does not appear, the program
is unable to recognize the fixture and will report each of the parameterized
methods as a separate fixure.