using eBay.Service.Core.Soap;
using Ebay2magento.Client.Entities;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ebay2Magento.Presentation
{
	public class CategoryViewModel : ViewModelBase
	{
		private Func<ISettingsService> _settingsService;
		private Func<IEbayService> _ebayService;
		private Func<IMagentoService> _magentoService;

		public StoreCustomCategoryTypeCollection EbayCategories
		{
			get { return GetValue(() => EbayCategories); }
			set { SetValue(() => EbayCategories, value); }
		}

		public CategoryData MagentoCategories
		{
			get { return GetValue(() => MagentoCategories); }
			set { SetValue(() => MagentoCategories, value); }
		}

		public CategoryData[] FlattenedCategories
		{
			get { return GetValue(() => FlattenedCategories); }
			set { SetValue(() => FlattenedCategories, value); }
		}

		public CategoryData SelectedCategory
		{
			get { return GetValue(() => SelectedCategory); }
			set { SetValue(() => SelectedCategory, value); }
		}

		public string NewCategoryName
		{
			get { return GetValue(() => NewCategoryName); }
			set { SetValue(() => NewCategoryName, value); }
		}

		public CategoryViewModel()
		{
			_settingsService = Resolve<ISettingsService>;
			_ebayService = Resolve<IEbayService>;
			_magentoService = Resolve<IMagentoService>;
		}

		public override async Task OnLoaded()
		{
			IsBusy = true;
			var ebayCategories = await _ebayService().GetCategories(CancellationToken);
			var magentoCategories = await _magentoService().GetCategories(CancellationToken);

			Dispatcher.CurrentDispatcher.Invoke(() =>
			{
				EbayCategories = ebayCategories;
				MagentoCategories = magentoCategories;

				FlattenedCategories = FlattenCategories(MagentoCategories.ChildrenData);

				IsBusy = false;
			});
		}

		public ICommand CreateCategory => new RelayCommand(async () =>
		{
			await _magentoService().CreateCategory(CancellationToken, NewCategoryName, SelectedCategory);
			var magentoCategories = await _magentoService().GetCategories(CancellationToken);

			Dispatcher.CurrentDispatcher.Invoke(() =>
			{
				MagentoCategories = magentoCategories;
				FlattenedCategories = FlattenCategories(MagentoCategories.ChildrenData);
			});
		});

		private CategoryData[] FlattenCategories(CategoryData[] categoryData)
		{
			if (categoryData == null)
			{
				return new CategoryData[0];
			}

			var data = new List<CategoryData>();

			foreach (var category in categoryData)
			{
				data.Add(category);
				if (category.ChildrenData != null)
				{
					data.AddRange(FlattenCategories(category.ChildrenData));
				}
			}

			return data.ToArray();
		}
	}
}
