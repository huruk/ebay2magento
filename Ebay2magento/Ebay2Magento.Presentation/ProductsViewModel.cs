using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Business.Extensions;
using Ebay2Magento.Presentation.Entities;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ebay2Magento.Presentation
{
	public class ProductsViewModel : ViewModelBase
	{
		private Func<IEbayService> _ebayService;
		private Func<IMagentoService> _magentoService;

		public StoreItem[] EbayItems
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
			var ebayCategories = await _ebayService().GetCategories(CancellationToken);

			var items = ebayItems
				.Where(item => !string.IsNullOrEmpty(item.SKU))
				.Select(item =>
				{
					var itemCategory = item.FindItemCategory(ebayCategories);

					return new StoreItem()
					{
						Id = item.ItemID,
						Title = item.Title,
						Quantity = item.Quantity,
						SKU = item.SKU,
						Category = itemCategory?.Name,
						Price = item.StartPrice.Value,
						Description = item.Description.Replace("\n", "")
					};
				});

			Dispatcher.CurrentDispatcher.Invoke(() =>
			{
				EbayItems = items.ToArray();
				IsBusy = false;
			});
		}

		public ICommand SyncProducts => new RelayCommand(async () =>
		{
			IsBusy = true;

			foreach (var item in EbayItems)
			{
				await _magentoService().CreateProduct(CancellationToken, item);
			}

			IsBusy = false;
		});
	}
}
