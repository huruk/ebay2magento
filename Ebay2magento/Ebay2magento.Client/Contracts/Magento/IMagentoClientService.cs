using Ebay2magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2magento.Client.Contracts.Magento
{
	public interface IMagentoClientService
	{
		Task<string> GetBearerToken(CancellationToken ct);

		Task<MagentoCategoryData> GetCategories(CancellationToken ct, string bearerToken);
	}
}
