using Ebay2magento.Client.Entities;
using Ebay2magento.Client.Entities.Outbound;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2magento.Client.Contracts.Magento
{
	public interface IMagentoClientService
	{
		Task<string> GetBearerToken(CancellationToken ct, string url, string username, string password);

		Task<CategoryData> GetCategories(CancellationToken ct, string url, string bearerToken);

		Task<CategoryData> CreateCategory(CancellationToken ct, string url, string bearerToken, CategoryOutboundData category);

		Task DeleteCategory(CancellationToken ct, string url, string categoryId, string bearerToken);
	}
}
