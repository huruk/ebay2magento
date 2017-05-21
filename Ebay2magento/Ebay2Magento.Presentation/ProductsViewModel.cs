using eBay.Service.Core.Soap;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Presentation.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ebay2Magento.Presentation
{
	public class ProductsViewModel : ViewModelBase
	{
		private Func<IEbayService> _ebayService;
		private Func<IMagentoService> _magentoService;

		public EbayItem[] EbayItems
		{
			get { return GetValue(() => EbayItems); }
			set { SetValue(() => EbayItems, value); }
		}

		public ProductsViewModel()
		{
			_ebayService = Resolve<IEbayService>;
			_magentoService = Resolve<IMagentoService>;
		}

		public override async Task OnLoaded()
		{
			IsBusy = true;
			var ebayItems = await _ebayService().GetInventory(CancellationToken);

			Dispatcher.CurrentDispatcher.Invoke(() =>
			{
				var items = ebayItems.Select(item => new EbayItem()
				{
					Id = item.ItemID,
					Title = item.Title,
					Quantity = item.Quantity,
					SKU = item.SKU
				});

				EbayItems = items.ToArray();
				IsBusy = false;
			});
		}
	}
}
