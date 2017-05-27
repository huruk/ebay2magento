using Ebay2Magento.ApplicationFramework.Contracts;
using System;
using System.Diagnostics;
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

		public async Task<HttpResponseMessage> Post(CancellationToken ct, string requestUri, HttpContent content)
		{
			return await _client.PostAsync(requestUri, content, ct);
		}

		public async Task<HttpResponseMessage> Delete(CancellationToken ct, string requestUri)
		{
			return await _client.DeleteAsync(requestUri, ct);
		}

		public async Task<HttpResponseMessage> Put(CancellationToken ct, string requestUri, HttpContent content)
		{
			return await _client.PutAsync(requestUri, content, ct);
		}
	}
}
