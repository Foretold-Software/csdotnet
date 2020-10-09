// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public class SingletonTestType1 : Singleton<SingletonTestType1>
	{
		public SingletonTestType1()
		{
			InstanceCount++;
		}

		public static volatile int InstanceCount = 0;
	}

	public class SingletonTestType2 : Singleton<SingletonTestType2>
	{
		public SingletonTestType2()
		{
			InstanceCount++;
		}

		public static volatile int InstanceCount = 0;
	}

	public class SingletonTestTypeNoCtor : Singleton<SingletonTestTypeNoCtor>
	{
		private SingletonTestTypeNoCtor() { }
	}
}
