using ApplicationFramework;
using Ebay2magento.Client.Contracts.Magento;
using Ebay2magento.Client.Entities;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Business.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Services
{
	public class MagentoService : IMagentoService
	{
		private Func<IMagentoClientService> _magentoService;
		private Func<ISettingsService> _settingsService;

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

		public async Task<MagentoCategoryData> GetCategories(CancellationToken ct)
		{
			var token = _settingsService().GetValue<string>(Constants.Settings.MagentoToken);
			var url = _settingsService().GetValue<string>(Constants.Settings.MagentoUrl);

			return await _magentoService().GetCategories(ct, url, token);
		}
	}
}
