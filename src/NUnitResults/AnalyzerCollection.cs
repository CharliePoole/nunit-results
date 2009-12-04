// *****************************************************
// Copyright 2008-2009, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;

namespace NUnit.Extras
{
	/// <summary>
	/// A typesafe Collection of Analyzers
	/// </summary>
	public class AnalyzerCollection : CollectionBase
	{
		public void Add( Analyzer analyzer )
		{
			InnerList.Add( analyzer );
		}

		public Analyzer this[int index]
		{
			get { return (Analyzer)InnerList[index]; }
		}

		public Analyzer this[string name]
		{
			get
			{
				foreach( Analyzer analyzer in InnerList )
					if ( analyzer.Name == name )
						return analyzer;

				return null;
			}
		}

		public void Sort()
		{
			InnerList.Sort();
		}
	}
}
