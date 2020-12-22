// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
	internal static class _Assert
	{
		/// <summary>
		/// Fails a test based on whether the specified file exists.
		/// </summary>
		/// <param name="assert">
		/// Throw-away parameter to enable Assert extension methods.
		/// </param>
		/// <param name="file">
		/// The absolute or relative path to file to check.
		/// </param>
		/// <param name="fileExists">
		/// If true, the test will be failed if the file does not exist.
		/// If false, the test will be failed if the file does exist.
		/// </param>
		/// <param name="inconclusive">
		/// Indicates whether to mark the test inconclusive instead of failing the test.
		/// </param>
		internal static void FileExists(this Assert assert, string file, bool fileExists = true, bool inconclusive = false)
		{
			if (File.Exists(file) != fileExists)
			{
				if (inconclusive)
				{
					Assert.Inconclusive("This test cannot produce conclusive results because the file exists: {0}", file);
				}
				else
				{
					Assert.Fail("This test fails because the file exists: {0}", file);
				}
			}
		}

		/// <summary>
		/// Fails the test if only one of the actions throws an exception,
		/// or if both throw an exception but the exception type is not equal.
		/// Otherwise, this method does not cause a test failure.
		/// </summary>
		/// <param name="assert">Throw-away parameter to enable Assert extension methods.</param>
		/// <param name="expectedAction">The action that throws the expected exception, if any.</param>
		/// <param name="actualAction">The action that throws the actual exception, if any.</param>
		internal static void SameException(this Assert assert, Action expectedAction, Action actualAction)
		{
			Exception actualException   = null;
			Exception expectedException = null;
			Type actualExceptionType    = null;
			Type expectedExceptionType  = null;

			//Get the actual guid or error.
			try
			{
				actualAction();
			}
			catch (Exception exc)
			{
				actualException = exc;
				actualExceptionType = actualException.GetType();
			}

			//Get the expected guid or error.
			try
			{
				expectedAction();
			}
			catch (Exception exc)
			{
				expectedException = exc;
				expectedExceptionType = expectedException.GetType();
			}

			//Compare the guid values and exceptions thrown.
			if (expectedException != null)
			{
				if (actualException != null)
				{
					Assert.IsInstanceOfType(actualException, expectedExceptionType);
				}
				else
				{
					Assert.Fail($"An exception of type {expectedExceptionType} is expected.");
				}
			}
			else if (actualException != null)
			{
				Assert.Fail("Unexpected exception thrown. Type: " + actualExceptionType);
			}
		}

		/// <summary>
		/// Fails the test if the two actions produce different return values or throw different exceptions.
		/// Otherwise, this method does not cause a test failure.
		/// </summary>
		/// <typeparam name="T">Return type of the action.</typeparam>
		/// <param name="assert">Throw-away parameter to enable Assert extension methods.</param>
		/// <param name="expectedAction">The action returning the expected value, or throwing the expected exception.</param>
		/// <param name="actualAction">The action returning the actual value, or throwing the actual exception.</param>
		internal static void SameReturnValueAndException<T>(this Assert assert, Func<T> expectedAction, Func<T> actualAction)
		{
			T actualValue               = default(T);
			T expectedValue             = default(T);
			Exception actualException   = null;
			Exception expectedException = null;
			Type actualExceptionType    = null;
			Type expectedExceptionType  = null;

			//Get the actual guid or error.
			try
			{
				actualValue = actualAction();
			}
			catch (Exception exc)
			{
				actualException = exc;
				actualExceptionType = actualException.GetType();
			}

			//Get the expected guid or error.
			try
			{
				expectedValue = expectedAction();
			}
			catch (Exception exc)
			{
				expectedException = exc;
				expectedExceptionType = expectedException.GetType();
			}

			//Compare the guid values and exceptions thrown.
			if (expectedException != null)
			{
				if (actualException != null)
				{
					Assert.IsInstanceOfType(actualException, expectedExceptionType);
				}
				else
				{
					Assert.Fail($"An exception of type {expectedExceptionType} is expected.");
				}
			}
			else if (actualException != null)
			{
				Assert.Fail("Unexpected exception thrown. Type: " + actualExceptionType);
			}
			else
			{
				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		/// <summary>
		/// Fails the test if the two actions produce different return collections or throw different exceptions.
		/// Otherwise, this method does not cause a test failure.
		/// </summary>
		/// <typeparam name="T">Item type of the returned collection.</typeparam>
		/// <param name="assert">Throw-away parameter to enable Assert extension methods.</param>
		/// <param name="expectedAction">The action returning the expected value, or throwing the expected exception.</param>
		/// <param name="actualAction">The action returning the actual value, or throwing the actual exception.</param>
		internal static void SameReturnCollectionAndException<T>(this Assert assert, Func<IList<T>> expectedAction, Func<IList<T>> actualAction)
		{
			IList<T> actualValue        = default(IList<T>);
			IList<T> expectedValue      = default(IList<T>);
			Exception actualException   = null;
			Exception expectedException = null;
			Type actualExceptionType    = null;
			Type expectedExceptionType  = null;

			//Get the actual guid or error.
			try
			{
				actualValue = actualAction();
			}
			catch (Exception exc)
			{
				actualException = exc;
				actualExceptionType = actualException.GetType();
			}

			//Get the expected guid or error.
			try
			{
				expectedValue = expectedAction();
			}
			catch (Exception exc)
			{
				expectedException = exc;
				expectedExceptionType = expectedException.GetType();
			}

			//Compare the guid values and exceptions thrown.
			if (expectedException != null)
			{
				if (actualException != null)
				{
					Assert.IsInstanceOfType(actualException, expectedExceptionType);
				}
				else
				{
					Assert.Fail($"An exception of type {expectedExceptionType} is expected.");
				}
			}
			else if (actualException != null)
			{
				Assert.Fail("Unexpected exception thrown. Type: " + actualExceptionType);
			}
			else
			{
				SameCollectionValues(assert, expectedValue, actualValue);
			}
		}

		/// <summary>
		/// Fails the test if two collections have different lengths, or it the items at any
		/// given index are not equal, or if one collection is null and the other is not.
		/// Otherwise, this method does not cause a test failure.
		/// </summary>
		/// <typeparam name="T">Item type of the collection items.</typeparam>
		/// <param name="assert">Throw-away parameter to enable Assert extension methods.</param>
		/// <param name="expected">The expected collection.</param>
		/// <param name="actual">The actual collection.</param>
		internal static void SameCollectionValues<T>(this Assert assert, IList<T> expected, IList<T> actual)
		{
			if (expected == null)
			{
				if (actual != null)
				{
					Assert.Fail("The collection is expected to be null.");
				}
			}
			else if (actual == null)
			{
				Assert.Fail("The collection is expected to not be null.");
			}
			else
			{
				int expectedLength = expected.Count;
				int actualLength = actual.Count;

				Assert.AreEqual(expectedLength, actualLength,
					"The collections have different lengths. Expected: {0}, Actual: {1}", expectedLength, actualLength);

				for (int i = 0; i < expectedLength; i++)
				{
					Assert.AreEqual(expected[i], actual[i],
						"The collection item at index {0} does not match. Expected: {1} Actual: {2}", i, expected[i]?.ToString() ?? "<null>", actual[i]?.ToString() ?? "<null>");
				}
			}
		}
	}
}
