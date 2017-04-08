using Microsoft.Practices.Unity;
using System;

namespace ApplicationFramework.Extensions
{
	public static class UnityContainerExtensions
	{
		public static T ResolveLazy<T>(this IUnityContainer container)
			where T : class
		{
			var instanceFunc = container.Resolve<Func<T>>();

			return instanceFunc();
		}

		public static IUnityContainer RegisterLazy<T>(this IUnityContainer container, Func<IUnityContainer, T> builder)
		{
			Func<T> func = () => builder(container);
			container.RegisterInstance(func.AsMemoized());
			return container;
		}
	}
}
