// *****************************************************
// Copyright 2008, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using NUnit.Core;

namespace NUnit.Extras
{
	/// <summary>
	/// Base Class for all analyzers. Represents a set of counts
	/// (declared in derived classes) and a set of methods for analyzing 
	/// nested analyzers.
	/// </summary>
	public abstract class Analyzer : IComparable
	{
		#region Fields

		/// <summary>
		/// The name of the item we are analyzing
		/// </summary>
		protected string name;

		/// <summary>
		/// An analyzer may have subordinate analyzers
		/// </summary>
		protected AnalyzerCollection children = new AnalyzerCollection();

		#endregion

		#region Properties

		public string Name
		{
			get { return name; }
		}

		public AnalyzerCollection Children
		{
			get { return children; }
		}

		#endregion

		#region Construction

		public Analyzer( string name )
		{
			this.name = name;
		}

		#endregion

		#region Public Methods

		public abstract void Analyze();

		public abstract void InitializeCounts();

		public int CompareTo( object obj )
		{
			Analyzer other = obj as Analyzer;

			if ( obj == null )
				return -1;
			else
				return this.Name.CompareTo( other.Name );
		}

		#endregion
	}
}
