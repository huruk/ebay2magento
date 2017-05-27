using eBay.Service.Core.Soap;
using Ebay2magento.ApplicationFramework.Entities;
using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Client.Contracts.Ebay
{
	public interface IEbayClientService
	{
		Task<string> GetSessionId(CancellationToken ct, EbayContext context);

		Task<UserTokenData> GetUserToken(CancellationToken ct, EbayContext context, string sessionID);

		Task<ItemTypeCollection> GetSellerListIDs(CancellationToken ct, EbayContext context);

		Task<StoreCustomCategoryTypeCollection> GetCategories(CancellationToken ct, EbayContext context);

		Task<string> GetItemDescription(CancellationToken ct, EbayContext context, string ItemID);
	}
}
