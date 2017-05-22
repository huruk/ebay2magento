using eBay.Service.Core.Soap;
using Ebay2magento.ApplicationFramework.Entities;
using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IEbayService
	{
		EbayContext Context { get; }

		Task<string> GetSessionId(CancellationToken ct);

		Task<UserTokenData> GetUserToken(CancellationToken ct, string sessionId);

		Task<ItemType[]> GetInventory(CancellationToken ct);

		Task<StoreCustomCategoryType[]> GetCategories(CancellationToken ct);
	}
}
