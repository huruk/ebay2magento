using ApplicationFramework;
using Ebay2magento.Client.Contracts.Magento;
using Ebay2magento.Client.Entities;
using Ebay2magento.Client.Entities.Outbound;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using eBay.Service.Core.Soap;
using System.Linq;
using Ebay2Magento.Presentation.Entities;
using Ebay2Magento.Business.Extensions;

namespace Ebay2Magento.Business.Services
{
	public class MagentoService : IMagentoService
	{
		private Func<IMagentoClientService> _magentoService;
		private Func<ISettingsService> _settingsService;

		private CategoryData _localCategories;

		public MagentoService(Func<IMagentoClientService> ebayService, Func<ISettingsService> settingsService)
		{
			_magentoService = ebayService;
			_settingsService = settingsService;
		}

		public async Task<string> GetBearerToken(CancellationToken ct, string url, string username, string password)
		{
			var token = await _magentoService().GetBearerToken(ct, url, username, password);

			_settingsService().SetValue(Constants.Settings.MagentoToken, token);
			_settingsService().SetValue(Constants.Settings.MagentoUrl, url);

			return token;
		}

		public async Task<CategoryData> GetCategories(CancellationToken ct)
		{
			var token = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			var url = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);

			return _localCategories = await _magentoService().GetCategories(ct, url, token);
		}

		public async Task<CategoryData> CreateCategory(CancellationToken ct, string name, CategoryData parent)
		{
			var token = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			var url = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);

			var outbound = new CategoryOutboundData()
			{
				Name = name,
				ParentId = parent.Id,
				IncludeInMenu = true,
				IsActive = true
			};

			return await _magentoService().CreateCategory(ct, url, token, outbound);
		}

		public async Task CreateProduct(CancellationToken ct, StoreItem product)
		{
			var token = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			var url = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);

			var categories = _localCategories ?? await GetCategories(ct);

			var targetCategory = product.FindMagentoCategory(categories);

			var outbound = new ProductOutboundData()
			{
				AttributeSetId = 4,
				Name = product.Title,
				Price = product.Price,
				SKU = product.SKU,
				CustomAttributes = new CustomAttributes[]
				{
					new CustomAttributes()
					{
						AttributeCode = "category_ids",
						Value = targetCategory.Id.ToString()
					},
					new CustomAttributes()
					{
						AttributeCode = "description",
						Value = product.Description
					}
				},
				ExtensionAttributes = new ExtensionAttributes()
				{
					StockItem = new StockItem()
					{
						IsInStock = true,
						Qty = product.Quantity
					}
				}
			};

			await _magentoService().CreateProduct(ct, url, token, outbound);
		}

		public async Task DeleteCategory(CancellationToken ct, string categoryId)
		{
			var token = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			var url = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);

			await _magentoService().DeleteCategory(ct, url, categoryId, token);
		}

		public async Task SyncCategories(CancellationToken ct, StoreCustomCategoryType[] ebayCategories, CategoryData magentoCategories)
		{
			var selectedCategory = magentoCategories;

			foreach (var ebayCategory in ebayCategories)
			{
				if (!magentoCategories.ChildrenData.Any(c => c.Name.Equals(ebayCategory.Name)))
				{
					await CreateCategory(ct, ebayCategory.Name, selectedCategory);
				}
			}

			var updatedCategories = await GetCategories(ct);

			foreach (var ebayCategory in ebayCategories.ToArray())
			{
				if (ebayCategory.ChildCategory != null)
				{
					var targetCategory = updatedCategories.ChildrenData.Single(c => c.Name.Equals(ebayCategory.Name));

					foreach (var ebayChild in ebayCategory.ChildCategory.ToArray())
					{
						if (!targetCategory.ChildrenData.Any(c => c.Name.Equals(ebayChild.Name)))
						{
							await CreateCategory(ct, ebayChild.Name, targetCategory);
						}
					}
				}
			}
		}
	}
}
