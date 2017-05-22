using ApplicationFramework;
using eBay.Service.Core.Soap;
using Ebay2magento.ApplicationFramework.Entities;
using Ebay2magento.ApplicationFramework.Serialization;
using Ebay2magento.Client.Contracts.Ebay;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Services
{
	public class EbayService : IEbayService
	{
		private Func<IEbayClientService> _ebayService;
		private Func<ISettingsService> _settingsService;
		private Func<ITurboListerClientService> _turbolisterService;

		private static string ItemPath =
			System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "ebay2magento", "ebayItems.json");

		private static string CategoryPath =
			System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "ebay2magento", "ebayCategories.json");

		private EbayContext _context;
		private ItemType[] _localItems;
		private StoreCustomCategoryType[] _localCategories;

		public EbayService(
			Func<IEbayClientService> ebayService,
			Func<ISettingsService> settingsService,
			Func<ITurboListerClientService> turbolisterService)
		{
			_ebayService = ebayService;
			_settingsService = settingsService;
			_turbolisterService = turbolisterService;

			_context = new EbayContext()
			{
				AppID = _settingsService().GetValue<string>(Constants.Settings.AppID),
				CertID = _settingsService().GetValue<string>(Constants.Settings.CertID),
				DevID = _settingsService().GetValue<string>(Constants.Settings.DevID),
				RuName = _settingsService().GetValue<string>(Constants.Settings.RuName),
				Token = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken)?.AccessToken,
			};

			if (File.Exists(ItemPath))
			{
				var items = File.ReadAllText(ItemPath);
				_localItems = JsonConvert.DeserializeObject<ItemType[]>(items);
			}

			if (File.Exists(CategoryPath))
			{
				var converter = new StoreCustomCategoryTypeSerializer();
				var categories = File.ReadAllText(CategoryPath);
				_localCategories = JsonConvert.DeserializeObject<StoreCustomCategoryType[]>(categories);

				foreach (var category in _localCategories)
				{
					if (category.ChildCategory != null)
					{
						var childrenCategories = new List<StoreCustomCategoryType>();

						foreach (var child in category.ChildCategory)
						{
							var obj = child as JObject;
							if (obj != null)
							{
								childrenCategories.Add(obj.ToObject<StoreCustomCategoryType>());
							}
						}

						category.ChildCategory = new StoreCustomCategoryTypeCollection(childrenCategories.ToArray());
					}
				}
			}
		}

		~EbayService()
		{
			var items = JsonConvert.SerializeObject(_localItems);
			var file = new FileInfo(ItemPath);
			file.Directory.Create();

			File.WriteAllText(file.FullName, items);

			var categories = JsonConvert.SerializeObject(_localCategories);
			file = new FileInfo(CategoryPath);
			file.Directory.Create();

			File.WriteAllText(file.FullName, categories);
		}

		public EbayContext Context => _context;

		public async Task<string> GetSessionId(CancellationToken ct)
		{
			return await _ebayService().GetSessionId(ct, Context);
		}

		public async Task<UserTokenData> GetUserToken(CancellationToken ct, string sessionId)
		{
			var userToken = await _ebayService().GetUserToken(ct, Context, sessionId);

			_settingsService().SetValue(Constants.Settings.UserToken, userToken);

			_context.Token = userToken.AccessToken;

			return userToken;
		}

		public async Task<StoreCustomCategoryType[]> GetCategories(CancellationToken ct)
		{
			if (_localCategories == null)
			{
				var items = await _ebayService().GetCategories(ct, Context);
				return _localCategories = items.ToArray();
			}

			return _localCategories;
		}

		public async Task<ItemType[]> GetInventory(CancellationToken ct)
		{
			if (_localItems == null)
			{
				var items = await _ebayService().GetSellerListIDs(ct, Context);
				return _localItems = items.ToArray();
			}

			return _localItems;
		}
	}
}
