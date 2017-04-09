using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Client.Contracts.Ebay
{
	public interface IEbayClientService
	{
		Task<string> GetSessionId(CancellationToken ct, string runame, string devId, string appid, string certId);
	}
}
