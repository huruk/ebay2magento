using Ebay2Magento.ApplicationFramework.Contracts;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.ApplicationFramework.Services
{
	public class QueryService : IQueryService
	{
		private readonly HttpClient _client;

		public QueryService()
		{
			_client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public IQueryService Header(string name, string value)
		{
			if (_client.DefaultRequestHeaders.Contains(name))
			{
				_client.DefaultRequestHeaders.Remove(name);
			}

			_client.DefaultRequestHeaders.Add(name, value);

			return this;
		}

		public async Task<HttpResponseMessage> Get(CancellationToken ct, string requestUri)
		{
			return await _client.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, ct);
		}

		//public async Task<Windows.Web.Http.HttpResponseMessage> UnsecureGet(CancellationToken ct, string requestUri)
		//{
		//	var filter = new HttpBaseProtocolFilter();

		//	filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
		//	filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.IncompleteChain);
		//	filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);

		//	using (var webClient = new Windows.Web.Http.HttpClient(filter))
		//	{
		//		webClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));

		//		return await webClient.GetAsync(new Uri(requestUri));
		//	}
		//}

		public Task<HttpResponseMessage> Post(CancellationToken ct, string requestUri, HttpContent content)
		{
			return _client.PostAsync(requestUri, content, ct);
		}
	}
}
