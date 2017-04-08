using ApplicationFramework.Extensions;
using Ebay2magento.Client.Contracts.Ebay;
using Ebay2magento.Client.Contracts.Magento;
using Ebay2magento.Client.Services.Ebay;
using Ebay2magento.Client.Services.Magento;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.ApplicationFramework.Services;
using Ebay2Magento.Business.Contracts;
using Ebay2Magento.Business.Services;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Services.Ebay;
using Microsoft.Practices.Unity;

namespace Ebay2Magento.Classic
{
	public static class Module
	{
		public static UnityContainer Container { get; private set; }

		public static void Bootstrap(UnityContainer container)
		{
			Container = container;

			container
				.RegisterLazy<IQueryService>(c => new QueryService())
				.RegisterLazy<ISettingsService>(c => new SettingsService())
				.RegisterLazy<IEbayClientService>(c =>
					new EbayClientService(
						c.ResolveLazy<IQueryService>,
						c.ResolveLazy<ISettingsService>
					)
				)
				.RegisterLazy<IMagentoClientService>(c =>
					 new MagentoClientService(
						 c.ResolveLazy<IQueryService>
					 )
				)
				.RegisterLazy<ITurboListerClientService>(c =>
					new TurboListerClientService()
				);

			container.RegisterLazy<IEbayService>(c =>
				new EbayService(
					c.ResolveLazy<IEbayClientService>,
					c.ResolveLazy<ISettingsService>,
					c.ResolveLazy<ITurboListerClientService>
				)
			);

			container.RegisterLazy<IMagentoService>(c =>
				new MagentoService(
					c.ResolveLazy<IMagentoClientService>,
					c.ResolveLazy<ISettingsService>
				)
			);
		}
	}
}
