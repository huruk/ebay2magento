using eBay.Service.Core.Soap;
using Ebay2magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IMagentoService
	{
		Task<string> GetBearerToken(CancellationToken ct, string url, string username, string password);

		Task<CategoryData> GetCategories(CancellationToken ct);

		Task<CategoryData> CreateCategory(CancellationToken ct, string name, CategoryData parent);

		Task SyncCategories(CancellationToken ct, StoreCustomCategoryType[] ebayCategories, CategoryData magentoCategories);
	}
}
