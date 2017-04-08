using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Client.Contracts.Ebay
{
	public interface IEbayClientService
	{
		Task<ApplicationTokenData> GetApplicationToken(CancellationToken ct, string ruName);

		Task<UserTokenData> GetUserToken(CancellationToken ct, string ruName);

		Task<UserTokenData> RefreshUserToken(CancellationToken ct, UserTokenData currentToken);

		Task GetInventory(CancellationToken ct, UserTokenData userToken, int limit, int offset);
	}
}
