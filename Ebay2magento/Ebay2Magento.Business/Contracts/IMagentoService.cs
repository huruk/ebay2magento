using Ebay2magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IMagentoService
	{
		Task<string> GetBearerToken(CancellationToken ct, string url, string username, string password);

		Task<MagentoCategoryData> GetCategories(CancellationToken ct);
	}
}
