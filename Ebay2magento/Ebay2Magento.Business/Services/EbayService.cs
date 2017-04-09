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

		public async Task<string> GetSessionId(CancellationToken ct)
		{
			var ruName = _settingsService().GetValue<string>(Constants.Settings.RuName);
			var devId = _settingsService().GetValue<string>(Constants.Settings.DevID);
			var certId = _settingsService().GetValue<string>(Constants.Settings.CertID);
			var appId = _settingsService().GetValue<string>(Constants.Settings.AppID);

			return await _ebayService().GetSessionId(ct, ruName, devId, appId, certId);
		}

		//public async Task GetTurbolisterItems(CancellationToken ct, StorageFile file)
		//{
		//	await _turbolisterService().ReadFile(ct, file);
		//}
	}
}
