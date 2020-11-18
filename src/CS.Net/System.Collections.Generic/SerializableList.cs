// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	[Serializable]
	public class SerializableList<T> : List<T>, ISerializable where T : IConvertible
	{
		#region Constructors
		public SerializableList() : base() { }
		public SerializableList(IEnumerable<T> collection) : base(collection) { }
		public SerializableList(int capacity) : base(capacity) { }
		public SerializableList(SerializationInfo info, StreamingContext context) : base(SerializableList.Deserialize<T>(info.GetString(propName))) { }
		#endregion

		#region Fields
		private const string propName = "list";
		#endregion

		#region Methods
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/>
		/// with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization.</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(propName, this.ToString(), typeof(string));
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return SerializableList.ToString(this);
		}
		#endregion
	}

	public static class SerializableList
	{
		#region Fields
		private const string delimiter = ",";
		#endregion

		/// <summary>
		/// Convert a delimited string to a list.
		/// </summary>
		internal static IEnumerable<T> Deserialize<T>(string list) where T : IConvertible
		{
			return list.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(item => (T)Convert.ChangeType(item, typeof(T)));
		}

		/// <summary>
		/// Convert a delimited string to a list.
		/// </summary>
		public static SerializableList<T> FromString<T>(string list) where T : IConvertible
		{
			try
			{
				return new SerializableList<T>(Deserialize<T>(list));
			}
			catch { return null; }
		}

		/// <summary>
		/// Convert a list to a delimited string.
		/// </summary>
		internal static string ToString<T>(SerializableList<T> list) where T : IConvertible
		{
#if NET35 || NET35_CLIENT
			throw new NotImplementedException(); //TODO: This in .Net 3.5
#else
			return string.Join(delimiter, list);
#endif
		}
	}
}