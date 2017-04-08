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

		public async Task<bool> GetBearerToken(CancellationToken ct)
		{
			var token = await _magentoService().GetBearerToken(ct);

			_settingsService().SetValue("MagentoToken", token);

			return true;
		}

		public async Task<MagentoCategoryData> GetCategories(CancellationToken ct)
		{
			var token = _settingsService().GetValue<string>("MagentoToken");
			return await _magentoService().GetCategories(ct, token);
		}
	}
}
