// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class SingletonTest
	{
		[TestMethod]
		public void ReferenceTest()
		{
			int numberOfThreads = 50;
			SingletonTestType1 instance1;
			SingletonTestType1[] instances1 = new SingletonTestType1[numberOfThreads];
			Thread[] threads = new Thread[numberOfThreads];

			instance1 = SingletonManager.GetInstance<SingletonTestType1>();

			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i] = new Thread((obj) =>
				{
					int index = (int)obj;

					instances1[index] = SingletonManager.GetInstance<SingletonTestType1>();

					Assert.IsTrue(instance1 is SingletonTestType1);
					Assert.IsTrue(instances1[index] is SingletonTestType1);
					Assert.IsTrue(object.ReferenceEquals(instance1, instances1[index]));
				});
			}

			//Start all threads.
			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i].Start(i);
			}

			//Wait for all threads to finish.
			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i].Join();
			}

			Assert.IsTrue(instance1 is SingletonTestType1);
			Assert.IsTrue(instances1.All(inst => inst is SingletonTestType1));
			Assert.IsTrue(instances1.All(inst => object.ReferenceEquals(inst, instance1)));
		}

		[TestMethod]
		public void ReferenceTestMultipleTypes()
		{
			int numberOfThreads = 50;
			SingletonTestType1 instance1;
			SingletonTestType2 instance2;
			SingletonTestType1[] instances1 = new SingletonTestType1[numberOfThreads];
			SingletonTestType2[] instances2 = new SingletonTestType2[numberOfThreads];
			Thread[] threads = new Thread[numberOfThreads];

			instance1 = SingletonManager.GetInstance<SingletonTestType1>();
			instance2 = SingletonManager.GetInstance<SingletonTestType2>();

			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i] = new Thread((obj) =>
				{
					int index = (int)obj;

					instances1[index] = SingletonManager.GetInstance<SingletonTestType1>();
					instances2[index] = SingletonManager.GetInstance<SingletonTestType2>();

					Assert.IsTrue(instance1 is SingletonTestType1);
					Assert.IsTrue(instance2 is SingletonTestType2);
					Assert.IsTrue(instances1[index] is SingletonTestType1);
					Assert.IsTrue(instances2[index] is SingletonTestType2);
					Assert.IsTrue(object.ReferenceEquals(instance1, instances1[index]));
					Assert.IsTrue(object.ReferenceEquals(instance2, instances2[index]));
				});
			}

			//Start all threads.
			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i].Start(i);
			}

			//Wait for all threads to finish.
			for (int i = 0; i < numberOfThreads; i++)
			{
				threads[i].Join();
			}

			Assert.IsTrue(instance1 is SingletonTestType1);
			Assert.IsTrue(instance2 is SingletonTestType2);
			Assert.IsTrue(instances1.All(inst => inst is SingletonTestType1));
			Assert.IsTrue(instances2.All(inst => inst is SingletonTestType2));
			Assert.IsTrue(instances1.All(inst => object.ReferenceEquals(inst, instance1)));
			Assert.IsTrue(instances2.All(inst => object.ReferenceEquals(inst, instance2)));
		}

		[TestMethod]
		public void ParameterlessConstructorTest()
		{
			SingletonTestTypeNoCtor instance;

			try
			{
				instance = SingletonManager.GetInstance<SingletonTestTypeNoCtor>();
				Assert.Fail("ArgumentException not thrown.");
			}
			catch (Exception exc)
			{
				Assert.IsInstanceOfType(exc, typeof(ArgumentException));
			}
		}
	}
}
