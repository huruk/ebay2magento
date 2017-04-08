using System;
using System.Collections.Generic;

namespace ApplicationFramework.Extensions
{
	public static class FuncExtensions
	{
		public static Func<T> AsMemoized<T>(this Func<T> func)
		{
			bool isSet = false;
			var value = default(T);

			return () =>
			{
				if (!isSet)
				{
					value = func();
					isSet = true;
				}

				return value;
			};
		}
	}
}
