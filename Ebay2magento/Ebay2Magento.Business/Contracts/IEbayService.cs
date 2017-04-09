using Ebay2Magento.Client.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Business.Contracts
{
	public interface IEbayService
	{
		Task<string> GetSessionId(CancellationToken ct);

		//Task GetTurbolisterItems(CancellationToken ct, StorageFile file);
	}
}
