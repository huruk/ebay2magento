using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.ApplicationFramework.Contracts
{
	public interface IQueryService
	{
		IQueryService Header(string name, string value);

		Task<HttpResponseMessage> Get(CancellationToken ct, string requestUri);

		Task<HttpResponseMessage> Post(CancellationToken ct, string requestUri, HttpContent content);
	}
}
