﻿using System;
using System.Reactive.Disposables;

namespace ApplicationFramework.Extensions
{
	public static class DisposableExtensions
	{
		public static void DisposeWith(this IDisposable disposable, CompositeDisposable composite)
		{
			if (composite == null || disposable == null)
				return;

			composite.Add(disposable);
		}
	}
}
