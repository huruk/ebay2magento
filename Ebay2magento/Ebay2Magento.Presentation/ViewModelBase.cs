using ApplicationFramework.Extensions;
using Ebay2Magento.Classic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Presentation
{
	public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
	{
		private readonly IDictionary<string, object> _repository = new Dictionary<string, object>();

		protected CompositeDisposable Disposable { get; set; }

		private CancellationTokenSource _cancellationTokenSource;

		protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

		public bool IsBusy
		{
			get { return GetValue(() => IsBusy); }
			set { SetValue(() => IsBusy, value); }
		}

		public ViewModelBase()
		{
			Disposable = new CompositeDisposable();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public void Loaded()
		{
			Task.Run(() => OnLoaded());
		}

		public virtual async Task OnLoaded()
		{
		}

		public static TService Resolve<TService>()
			where TService : class
		{
			return Module.Container.ResolveLazy<TService>();
		}

		public TType GetValue<TType>(Expression<Func<TType>> key, Func<TType> factory = null)
		{
			return GetValue(GetPropertyName(key), factory);
		}

		private TType GetValue<TType>(string key, Func<TType> factory = null)
		{
			if (!_repository.ContainsKey(key))
			{
				_repository[key] = factory != null ? factory() : default(TType);
				RaisePropertyChanged(key);
			}

			return (TType)_repository[key];
		}

		public void SetValue<TType>(Expression<Func<TType>> key, TType value)
		{
			SetValue(GetPropertyName(key), value);
		}

		private void SetValue<TType>(string key, TType value)
		{
			_repository[key] = value;
			RaisePropertyChanged(key);
		}

		public override void Cleanup()
		{
			Disposable.Dispose();
			_repository.Clear();
			_cancellationTokenSource.Cancel();

			base.Cleanup();
		}
	}
}
