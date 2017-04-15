﻿using eBay.Service.Core.Soap;
using Ebay2magento.Client.Entities;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using System;
using System.Threading.Tasks;
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

		public MagentoCategoryData MagentoCategories
		{
			get { return GetValue(() => MagentoCategories); }
			set { SetValue(() => MagentoCategories, value); }
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

				IsBusy = false;
			});
		}
	}
}