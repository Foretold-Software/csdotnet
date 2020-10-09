// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections.ObjectModel
{
	public class TieredListTestType
	{
		public TieredListTestType(string value)
		{
			Value = value;
		}
		public string Value { get; set; }

		public override string ToString()
		{
			return "Value = " + Value;
		}
	}
}
