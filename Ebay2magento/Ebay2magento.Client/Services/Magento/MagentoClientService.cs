﻿using ApplicationFramework;
using ApplicationFramework.Extensions;
using Ebay2magento.Client.Contracts.Magento;
using Ebay2magento.Client.Entities;
using Ebay2magento.Client.Entities.Outbound;
using Ebay2Magento.ApplicationFramework.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2magento.Client.Services.Magento
{
	public class MagentoClientService : IMagentoClientService
	{
		private readonly Func<IQueryService> _queryService;

		public MagentoClientService(Func<IQueryService> queryService)
		{
			_queryService = queryService;
		}

		public async Task<string> GetBearerToken(CancellationToken ct, string url, string username, string password)
		{
			var content = JsonConvert.SerializeObject(new
			{
				username = username,
				password = password
			});

			var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

			return await _queryService()
				.Post(ct, url + "/index.php/rest/V1/integration/admin/token", httpContent)
				.ToEntity<string>(ct);
		}

		public async Task<CategoryData> GetCategories(CancellationToken ct, string url, string bearerToken)
		{
			return await _queryService()
				.Header("Authorization", "Bearer " + bearerToken)
				.Get(ct, url + "/index.php/rest/V1/categories/")
				.ToEntity<CategoryData>(ct);
		}

		public async Task<CategoryData> CreateCategory(CancellationToken ct, string url, string bearerToken, CategoryOutboundData category)
		{
			var obj = new { category = category };

			var httpContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

			await _queryService()
				.Header("Authorization", "Bearer " + bearerToken)
				.Post(ct, url + "/index.php/rest/V1/categories", httpContent);

			return null;
		}

		public async Task CreateProduct(CancellationToken ct, string url, string bearerToken, ProductOutboundData product)
		{
			var obj = new { product = product };

			var httpContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

			await _queryService()
				.Header("Authorization", "Bearer " + bearerToken)
				.Post(ct, url + "/index.php/rest/V1/products", httpContent);
		}

		public async Task DeleteCategory(CancellationToken ct, string url, string categoryId, string bearerToken)
		{
			await _queryService()
				.Delete(ct, url + categoryId);
		}
	}
}
