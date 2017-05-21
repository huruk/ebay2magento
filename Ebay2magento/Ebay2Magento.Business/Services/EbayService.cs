using ApplicationFramework;
using eBay.Service.Core.Soap;
using Ebay2magento.ApplicationFramework.Entities;
using Ebay2magento.Client.Contracts.Ebay;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Entities;
using Newtonsoft.Json;
using System;
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

		private static string Path =
			System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "ebay2magento", "ebayItems.json");

		private EbayContext _context;
		private ItemType[] _localItems;

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

			if (File.Exists(Path))
			{
				var settings = File.ReadAllText(Path);
				_localItems = JsonConvert.DeserializeObject<ItemType[]>(settings);
			}
		}

		~EbayService()
		{
			var items = JsonConvert.SerializeObject(_localItems);
			var file = new FileInfo(Path);
			file.Directory.Create();

			File.WriteAllText(file.FullName, items);
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

		public async Task<StoreCustomCategoryTypeCollection> GetCategories(CancellationToken ct)
		{
			return await _ebayService().GetCategories(ct, Context);
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
