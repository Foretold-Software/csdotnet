// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	/// <summary>
	/// Used to create serializable lists of objects which can be converted to type <see cref="string"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the objects contained in the list.
	/// This type must inherit from <see cref="IConvertible"/>.
	/// </typeparam>
	[Serializable]
	public class SerializableList<T> : List<T>, ISerializable where T : IConvertible
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of type <see cref="SerializableList{T}"/>.
		/// </summary>
		public SerializableList() : base() { }

		/// <summary>
		/// Creates a new instance of type <see cref="SerializableList{T}"/>.
		/// </summary>
		/// <param name="collection">
		/// The collection whose elements are copied to the new list.
		/// </param>
		public SerializableList(IEnumerable<T> collection) : base(collection) { }

		/// <summary>
		/// Creates a new instance of type <see cref="SerializableList{T}"/>.
		/// </summary>
		/// <param name="capacity">
		/// The number of elements that the new list can initially store.
		/// </param>
		public SerializableList(int capacity) : base(capacity) { }

		/// <summary>
		/// Creates a new instance of type <see cref="SerializableList{T}"/>.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo"/> object to populate with data.
		/// </param>
		/// <param name="context">
		/// The destination <see cref="StreamingContext"/> object for this serialization.
		/// </param>
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
		/// <param name="info">
		/// The <see cref="SerializationInfo"/> object to populate with data.
		/// </param>
		/// <param name="context">
		/// The destination <see cref="StreamingContext"/> object for this serialization.
		/// </param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(propName, this.ToString(), typeof(string));
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string representing the current object.
		/// </returns>
		public override string ToString()
		{
			return SerializableList.ToString(this);
		}
		#endregion
	}

	/// <summary>
	/// A static class containing useful helper methods for serializing
	/// and deserializing an object of type <see cref="SerializableList{T}"/>.
	/// </summary>
	public static class SerializableList
	{
		#region Fields
		internal const string delimiter = ",";
		#endregion

		/// <summary>
		/// Converts a delimited string to a list.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the objects contained in the list.
		/// This type must inherit from <see cref="IConvertible"/>.
		/// </typeparam>
		/// <param name="list">
		/// A string representing the serialized list.
		/// </param>
		/// <returns>
		/// Returns a collection of items contained in the serialized list.
		/// </returns>
		internal static IEnumerable<T> Deserialize<T>(string list) where T : IConvertible
		{
			return list.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(item => (T)Convert.ChangeType(item, typeof(T)));
		}

		/// <summary>
		/// Convert a delimited string to a list.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the objects contained in the list.
		/// This type must inherit from <see cref="IConvertible"/>.
		/// </typeparam>
		/// <param name="list">
		/// A string representing the serialized list.
		/// </param>
		/// <returns>
		/// A string representing the serializable list.
		/// </returns>
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
		/// <typeparam name="T">
		/// The type of the objects contained in the list.
		/// This type must inherit from <see cref="IConvertible"/>.
		/// </typeparam>
		/// <param name="list">
		/// A list to be serialized to a string.
		/// </param>
		/// <returns>
		/// A string representing the serializable list.
		/// </returns>
		internal static string ToString<T>(SerializableList<T> list) where T : IConvertible
		{
#if NET35 || NET35_CLIENT
			return ToString_Net35(list);
#else
			return string.Join(delimiter, list);
#endif
		}

		/// <summary>
		/// Convert a list to a delimited string.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the objects contained in the list.
		/// This type must inherit from <see cref="IConvertible"/>.
		/// </typeparam>
		/// <param name="list">
		/// A list to be serialized to a string.
		/// </param>
		/// <returns>
		/// A string representing the serializable list.
		/// </returns>
		internal static string ToString_Net35<T>(SerializableList<T> list) where T : IConvertible
		{
			return string.Join(delimiter, list.Select(item => item?.ToString()).ToArray());
		}
	}
}