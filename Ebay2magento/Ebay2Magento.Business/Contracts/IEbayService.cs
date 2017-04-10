using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IEbayService
	{
		Task<string> GetSessionId(CancellationToken ct);

		Task<UserTokenData> GetUserToken(CancellationToken ct, string sessionId);

		Task GetInventory(CancellationToken ct);
	}
}
