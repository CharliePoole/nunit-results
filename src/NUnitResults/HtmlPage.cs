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
	/// Utility class that represents a page of html output
	/// </summary>
	public class HtmlPage : StreamWriter
	{
		string name;
		string directory;

		/// <summary>
		/// Construct a new page in the output directory
		/// </summary>
		public HtmlPage( string name ) : base( name )
		{
			this.name = Path.GetFileName( name );
			this.directory = Path.GetDirectoryName( Path.GetFullPath( name ) );
		}

		/// <summary>
		/// Begin a page, writing a title
		/// </summary>
		public void Begin( string title)
		{
			WriteLine( "<html>" );
			WriteStyles();
			WriteLine( "<body>" );
			WritePageHeading( title );
		}

		private void WriteStyles()
		{
			WriteLine( "<style> <!--" );
			WriteLine( "  body { text-align: center; }" );
			WriteLine( "  table, td, th { border: 1px solid black; border-collapse: collapse; }" );
			WriteLine( "  th { width: 60px; padding: 3px; text-align: center; vertical-align: top }" );
			WriteLine( "  td { width: 60px; padding: 3px; text-align: right; vertical-align: top }" );
			WriteLine( "  table.center { margin-left: auto; margin-right: auto; border: 0 }" );
			WriteLine( "  .noborder { border: 0 }" );
			WriteLine( "  .navlinks { border: 0; text-align: right; vertical-align: top }" );
			WriteLine( "  .title { border: 0; width: 80%; text-align: center; vertical-align: top }" );
			WriteLine( "  .name { width: 250px; text-align: left }" );
			WriteLine( "  .break { border-left: 3px double black; }" );
			WriteLine( "  .top-row { background-color: rgb(0,224,255); }" );
			WriteLine( "  .hdr-row, .total-row { background-color: rgb(224, 224, 255); font-weight: bold; }" );
			WriteLine( "  .failure-row { background-color: rgb( 255, 128, 128 ); }" );
			WriteLine( "  .notrun-row { background-color: rgb( 255, 255, 128 ); }" );
			WriteLine( "  .text { text-align: left }" );
			WriteLine( "--> </style>" );
		}

		private void WritePageHeading( string title )
		{
			BeginTable( "width=90% class=\"center\"" );
			BeginRow();
			
			BeginCell( "navlinks" );

            if (this.name != "index.html")
            {
                WriteLine("<h3><a href=\"index.html\">Index</a></h3>");
            }

			EndCell();
			
			BeginCell( "title" );
			WriteLine( "<h1>{0}</h1>", title.Replace( " ", "&nbsp;" ) );

            DateTime now = DateTime.Now;
            WriteLine("<h3>{0}</h3>", now.ToString("yyyy-MM-dd HH:mm:ss"));
            EndCell();
			
			WriteEmptyCell( "noborder" );
	
			EndRow();
			EndTable();
		}

		/// <summary>
		/// Terminate and close the page
		/// </summary>
		public void End()
		{
			WriteLine( "</body>" );
			WriteLine( "</html>" );
			Close();
		}

		/// <summary>
		/// Begin a table
		/// </summary>
		public void BeginTable()
		{
			WriteLine( "<table>" );
		}

		/// <summary>
		/// Begin a table, specifying attributes
		/// </summary>
		public void BeginTable( string attrs )
		{
			WriteLine( "<table {0}>", attrs );
		}

		/// <summary>
		/// End a table, skipping a line for the next table
		/// </summary>
		public void EndTable()
		{
			WriteLine( "</table>" );
			WriteLine( "<p>&nbsp;</p>" );
		}

		/// <summary>
		/// Begin a row
		/// </summary>
		public void BeginRow()
		{
			WriteLine( "<tr>" );
		}

		public void BeginRow( string style )
		{
			WriteLine( "<tr class=\"{0}\">", style );
		}

		/// <summary>
		/// End current row
		/// </summary>
		public void EndRow()
		{
			WriteLine( "</tr>" );
		}

		/// <summary>
		/// Begin a Cell
		/// </summary>
		public void BeginCell()
		{
			WriteLine( "<td>" );
		}

		public void BeginCell( string style )
		{
			WriteLine( "<td class=\"{0}\">", style );
		}

		/// <summary>
		/// End current cell
		/// </summary>
		public void EndCell()
		{
			WriteLine( "</td>" );
		}

		/// <summary>
		/// Write a header cell containing specified text
		/// </summary>
		public void WriteHeaderCell( string text )
		{
			WriteLine( "  <th>{0}</th>", text );
		}

		/// <summary>
		/// Write a header cell spanning a number of columns
		/// </summary>
		public void WriteHeaderCell( string text, int colspan )
		{
			WriteLine( "  <th colspan={0}>{1}</th>", colspan, text );
		}

		/// <summary>
		/// Write header cell specifying class for css style
		/// </summary>
		public void WriteHeaderCell( string text, string style )
		{
			WriteLine( "  <th class={0}>{1}</th>", style, text );
		}

		/// <summary>
		/// Write header cell spanning a number of columns using a style
		/// </summary>
		public void WriteHeaderCell( string text, string style, int colspan )
		{
			WriteLine( "  <th colspan={0} class={1}>{2}</th>", colspan, style, text );
		}

		/// <summary>
		/// Write a normal cell containing text
		/// </summary>
		public void WriteCell( string text )
		{
			WriteLine( "  <td>{0}</td>", text );
		}

		public void WriteEmptyCell()
		{
			WriteCell( "&nbsp;" );
		}

		public void WriteEmptyCell( string style )
		{
			WriteCell( "&nbsp;", style );
		}

		public void WriteSpannedCell( string text, int colspan )
		{
			WriteLine( "  <td colspan={0}>{1}</td>", colspan, text );
		}

		public void WriteSpannedCell( string text, string style, int colspan )
		{
			WriteLine( "  <td class={0} colspan={1}>{2}</td>", style, colspan, text );
		}

		/// <summary>
		/// Write a cell specifying class for css style
		/// </summary>
		public void WriteCell( string text, string style )
		{
			WriteLine( "  <td class={0}>{1}</td>", style, text );
		}

		/// <summary>
		/// Write a cell containing a double as a percentage
		/// </summary>
		public void WritePercentCell( double dblValue )
		{
			WriteCell( dblValue.ToString( "##0%" ) );
		}

		public void WriteCell( int intValue, string style )
		{
			WriteCell( intValue.ToString(), style );
		}

		/// <summary>
		///  Write a cell containing an integer
		/// </summary>
		public void WriteCell( int intValue )
		{
			WriteCell( intValue.ToString() );
		}
	}
}
