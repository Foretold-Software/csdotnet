// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
	internal class _Assert
	{
		/// <summary>
		/// Fails the test if only one of the actions throws an exception,
		/// or if both throw an exception but the exception type is not equal.
		/// Otherwise, this method does cause a test failure.
		/// </summary>
		/// <param name="expectedAction">The action that throws the expected exception, if any.</param>
		/// <param name="actualAction">The action that throws the actual exception, if any.</param>
		internal static void SameException(Action expectedAction, Action actualAction)
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
		/// Otherwise, this method does cause a test failure.
		/// </summary>
		/// <typeparam name="T">Return type of the action.</typeparam>
		/// <param name="expectedAction">The action returning the expected value, or throwing the expected exception.</param>
		/// <param name="actualAction">The action returning the actual value, or throwing the actual exception.</param>
		internal static void SameReturnValueAndException<T>(Func<T> expectedAction, Func<T> actualAction)
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
	}
}
