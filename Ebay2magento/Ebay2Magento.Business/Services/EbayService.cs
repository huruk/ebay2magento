using ApplicationFramework;
using Ebay2magento.ApplicationFramework.Entities;
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

		public async Task<string> GetSessionId(CancellationToken ct)
		{
			var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			var devId = _settingsService().GetValue<string>(Constants.Settings.DevID);
			var certId = _settingsService().GetValue<string>(Constants.Settings.CertID);
			var appId = _settingsService().GetValue<string>(Constants.Settings.AppID);

			return await _ebayService().GetSessionId(ct, ruName, devId, appId, certId);
		}

		public async Task<UserTokenData> GetUserToken(CancellationToken ct, string sessionId)
		{
			var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			var devId = _settingsService().GetValue<string>(Constants.Settings.DevID);
			var certId = _settingsService().GetValue<string>(Constants.Settings.CertID);
			var appId = _settingsService().GetValue<string>(Constants.Settings.AppID);

			var userToken = await _ebayService().GetUserToken(ct, ruName, devId, appId, certId, sessionId);

			_settingsService().SetValue(Constants.Settings.UserToken, userToken);

			return userToken;
		}

		public async Task GetInventory(CancellationToken ct)
		{
			var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			var devId = _settingsService().GetValue<string>(Constants.Settings.DevID);
			var certId = _settingsService().GetValue<string>(Constants.Settings.CertID);
			var appId = _settingsService().GetValue<string>(Constants.Settings.AppID);
			var userToken = _settingsService().GetValue<UserTokenData>(Constants.Settings.UserToken);

			var context = new EbayContext()
			{
				AppId = _settingsService().GetValue<string>(Constants.Settings.AppID),
				CertId = _settingsService().GetValue<string>(Constants.Settings.CertID),
				DevID = _settingsService().GetValue<string>(Constants.Settings.DevID),
				RuName = _settingsService().GetValue<string>(Constants.Settings.RuName),
				Token = userToken.AccessToken
			};

			await _ebayService().GetInventory(ct, context);
		}
	}
}
