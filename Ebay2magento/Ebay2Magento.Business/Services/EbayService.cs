using ApplicationFramework;
using Ebay2magento.Client.Contracts.Ebay;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Services
{
	public class EbayService : IEbayService
	{
		private Func<IEbayClientService> _ebayService;
		private Func<ISettingsService> _settingsService;
		private Func<ITurboListerClientService> _turbolisterService;

		public EbayService(
			Func<IEbayClientService> ebayService,
			Func<ISettingsService> settingsService,
			Func<ITurboListerClientService> turbolisterService)
		{
			_ebayService = ebayService;
			_settingsService = settingsService;
			_turbolisterService = turbolisterService;
		}

		public async Task<ApplicationTokenData> GetApplicationToken(CancellationToken ct)
		{
			var currentToken = _settingsService().GetValue<ApplicationTokenData>(Constants.Settings.ApplicationToken);

			if (currentToken == null || currentToken?.AccessToken == null)
			{
				var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);

				var token = await _ebayService().GetApplicationToken(ct, ruName);
				_settingsService().SetValue(Constants.Settings.ApplicationToken, token);

				return token;
			}

			return currentToken;
		}

		public async Task<UserTokenData> GetUserToken(CancellationToken ct)
		{
			var currentToken = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken);

			if (currentToken == null)
			{
				var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);

				var token = await _ebayService().GetUserToken(ct, ruName);
				_settingsService().SetValue(Constants.Settings.UserToken, token);

				return token;
			}

			return currentToken;
		}

		public async Task GetInventory(CancellationToken ct)
		{
			var currentToken = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken);

			await _ebayService().GetInventory(ct, currentToken, 50, 0);
		}

		//public async Task GetTurbolisterItems(CancellationToken ct, StorageFile file)
		//{
		//	await _turbolisterService().ReadFile(ct, file);
		//}
	}
}
