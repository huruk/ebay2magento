using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ApplicationFramework
{
	public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
	{
		public RangeEnabledObservableCollection(IEnumerable<T> collection) : base(collection) { }

		public RangeEnabledObservableCollection() : base() { }

		public async Task InsertRange(IEnumerable<T> items)
		{
			CheckReentrancy();

			if (items == null)
			{
				return;
			}

			foreach (var item in items)
			{
				Items.Add(item);
			}

			DispatcherScheduler.Current.Schedule(() => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
		}
	}
}
