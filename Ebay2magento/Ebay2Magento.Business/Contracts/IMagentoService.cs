using Ebay2magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IMagentoService
	{
		Task<bool> GetBearerToken(CancellationToken ct);

		Task<MagentoCategoryData> GetCategories(CancellationToken ct);
	}
}
