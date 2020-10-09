// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Diagnostics;
using System.Linq;

namespace System.Collections.Generic
{
	/// <summary>
	/// Represents a collection of keys and values.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	public class TwoWayDictionary<TKey, TValue> : Dictionary<TKey, TValue>
		where TKey : IEquatable<TKey>
		where TValue : IEquatable<TValue>
	{
		public TwoWayDictionary()
		{
			var keyType = typeof(TKey);
			var valueType = typeof(TValue);
			if (keyType == valueType)
			{
				throw new Exception("This won't work.");
			}
		}

		public TKey this[TValue tValue]
		{
			get
			{
				if (this.ContainsValue(tValue))
				{
					return this.FirstOrDefault(kvp => object.Equals(kvp.Value, tValue)).Key;
				}
				else throw new KeyNotFoundException("Value could not be found.");
			}
			set
			{
				var newKey = value;
				var newKeyExists = this.ContainsKey(newKey);
				var valueExists = this.ContainsValue(tValue);
				var existingKey = this.FirstOrDefault(kvp => object.Equals(kvp.Value, tValue)).Key;

				if (!object.Equals(newKey, existingKey))
				{
					if (newKeyExists)
					{
						this[newKey] = tValue;
					}
					if (valueExists)
					{
						this.Remove(existingKey);
					}
					if (!newKeyExists && !valueExists)
					{
						this.Add(newKey, tValue);
					}
				}
			}
		}

		private static void asdf()
		{
			TwoWayDictionary<string, string> twd = new TwoWayDictionary<string, string>();
			twd["A"] = "1";
		}
	}
}
