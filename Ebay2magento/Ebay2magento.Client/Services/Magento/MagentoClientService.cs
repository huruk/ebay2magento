﻿using ApplicationFramework;
using ApplicationFramework.Extensions;
using Ebay2magento.Client.Contracts.Magento;
using Ebay2magento.Client.Entities;
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

		public async Task<string> GetBearerToken(CancellationToken ct)
		{
			var content = JsonConvert.SerializeObject(new
			{
				username = "",
				password = ""
			});

			var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

			return await _queryService()
				.Post(ct, Constants.Magento.Url + "V1/integration/admin/token", httpContent)
				.ToEntity<string>(ct);
		}

		public async Task<MagentoCategoryData> GetCategories(CancellationToken ct, string bearerToken)
		{
			return await _queryService()
				.Header("Authorization", "Bearer " + bearerToken)
				.Get(ct, Constants.Magento.Url + "V1/categories/")
				.ToEntity<MagentoCategoryData>(ct);
		}
	}
}
