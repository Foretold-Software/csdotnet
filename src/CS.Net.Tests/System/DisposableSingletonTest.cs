// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class DisposableSingletonTest
	{

		[TestMethod]
		public void ReferenceTest()
		{
			int numberOfThreads = 50;
			DisposableSingletonTestType instance;
			DisposableSingletonTestType[] instances = new DisposableSingletonTestType[numberOfThreads];
			Thread[] threads = new Thread[numberOfThreads];

			using (instance = DisposableSingletonTestType.GetInstance())
			{
				for (int i = 0; i < numberOfThreads; i++)
				{
					threads[i] = new Thread((obj) =>
					{
						int index = (int)obj;

						using (instances[index] = DisposableSingletonTestType.GetInstance())
						{
							Assert.IsFalse(instances[index].IsDisposed);
							Assert.IsFalse(instance.IsDisposed);
							Assert.IsTrue(object.ReferenceEquals(instance, instances[index]));
						}

						Assert.IsFalse(instances[index].IsDisposed);
						Assert.IsFalse(instance.IsDisposed);
						Assert.IsTrue(object.ReferenceEquals(instance, instances[index]));
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

				Assert.IsFalse(instance.IsDisposed);
			}

			Assert.IsTrue(instance.IsDisposed);
			for (int i = 0; i < numberOfThreads; i++)
			{
				Assert.IsTrue(instances[i].IsDisposed);
				Assert.IsTrue(object.ReferenceEquals(instance, instances[i]));
			}
		}

		[TestMethod]
		public void ReferenceTestMultipleTypes()
		{
			int numberOfThreads = 50;
			DisposableSingletonTestType instance;
			DisposableSingletonTestType2 instance2;
			DisposableSingletonTestType[] instances = new DisposableSingletonTestType[numberOfThreads];
			DisposableSingletonTestType2[] instances2 = new DisposableSingletonTestType2[numberOfThreads];
			Thread[] threads = new Thread[numberOfThreads];

			using (instance = DisposableSingletonTestType.GetInstance())
			{
				using (instance2 = DisposableSingletonTestType2.GetInstance())
				{

					for (int i = 0; i < numberOfThreads; i++)
					{
						threads[i] = new Thread((obj) =>
						{
							int index = (int)obj;

							using (instances[index] = DisposableSingletonTestType.GetInstance())
							{
								Assert.IsTrue(instances.All(inst => inst == null || !inst.IsDisposed));
								Assert.IsTrue(instances[0] == null || object.ReferenceEquals(instances[0], instances[index]));

								using (instances2[index] = DisposableSingletonTestType2.GetInstance())
								{
									Assert.IsTrue(instances2.All(inst => inst == null || !inst.HasDisposed));
									Assert.IsTrue(instances2[0] == null || object.ReferenceEquals(instances2[0], instances2[index]));
								}

								Assert.IsTrue(instances.All(inst => inst == null || !inst.IsDisposed));
								Assert.IsTrue(instances[0] == null || object.ReferenceEquals(instances[0], instances[index]));
							}
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

					Assert.IsTrue(instances.All(inst => inst != null && !inst.IsDisposed));
					Assert.IsTrue(instances.All(inst => object.ReferenceEquals(instances[0], inst)));
					Assert.IsTrue(instances2.All(inst => inst != null && !inst.HasDisposed));
					Assert.IsTrue(instances2.All(inst => object.ReferenceEquals(instances2[0], inst)));
				}

				Assert.IsTrue(instances.All(inst => inst != null && !inst.IsDisposed));
				Assert.IsTrue(instances.All(inst => object.ReferenceEquals(instances[0], inst)));
				Assert.IsTrue(instances2.All(inst => inst != null && inst.HasDisposed));
				Assert.IsTrue(instances2.All(inst => object.ReferenceEquals(instances2[0], inst)));
			}

			Assert.IsTrue(instances.All(inst => inst != null && inst.IsDisposed));
			Assert.IsTrue(instances.All(inst => object.ReferenceEquals(instances[0], inst)));
			Assert.IsTrue(instances2.All(inst => inst != null && inst.HasDisposed));
			Assert.IsTrue(instances2.All(inst => object.ReferenceEquals(instances2[0], inst)));
		}

		[TestMethod]
		public void ParameterlessConstructorTest()
		{
			DisposableSingletonTestTypeNoCtor instance;

			using (instance = null)
			{
				try
				{
					instance = DisposableSingletonTestTypeNoCtor.GetInstance();
					Assert.Fail("ArgumentException not thrown.");
				}
				catch (Exception exc)
				{
					Assert.IsInstanceOfType(exc, typeof(ArgumentException));
				}
			}
		}
	}
}
