// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public class ValueString
	{
		public ValueString(string value)
		{
			Value = value;
		}

		public string Value { get; set; }

		public static implicit operator ValueString(string s)
		{
			return new ValueString(s);
		}

		public override string ToString()
		{
			return "Value = " + Value;
		}
	}
}
