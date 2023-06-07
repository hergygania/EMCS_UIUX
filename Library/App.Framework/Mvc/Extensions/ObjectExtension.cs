using System;

namespace System
{
	public static class ObjectExtension
	{
		public static T NullToDefault<T>(this T obj) where T : class
		{
			if(obj != null)
				return obj;
			return Activator.CreateInstance<T>();
		}

		public static T NullToDefault<T>(this T obj, Func<T> factory) where T : class
		{
			if(obj != null)
				return obj;
			return factory();
		}

		public static T Validate<T>(this T obj, string errMsg) where T : class
		{
			if(obj == null)
				throw new NullReferenceException(errMsg);
			return obj;
		}

		public static T ValidateNullArg<T>(this T obj, string errMsg) where T : class
		{
			if(obj == null)
				throw new ArgumentNullException(errMsg);
			return obj;
		}
	}
}
