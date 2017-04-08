using ApplicationFramework;
using ApplicationFramework.Extensions;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ebay2Magento.Client.Services.Ebay
{
	public class EbayClientService : IEbayClientService
	{
		private readonly Func<IQueryService> _queryService;
		private readonly Func<ISettingsService> _settingsService;

		private static string _base64Auth;

		public EbayClientService(Func<IQueryService> queryService, Func<ISettingsService> settingsService)
		{
			_queryService = queryService;
			_settingsService = settingsService;

			var appId = _settingsService().GetValue<string>(Constants.Settings.AppID);
			var certId = _settingsService().GetValue<string>(Constants.Settings.CertID);

			var auth = Encoding.UTF8.GetBytes(appId + ":" + certId);
			_base64Auth = Convert.ToBase64String(auth);
		}

		public async Task<ApplicationTokenData> GetApplicationToken(CancellationToken ct, string ruName)
		{
			var parameters = new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("grant_type", "client_credentials"),
				new KeyValuePair<string, string>("redirect_uri", ruName),
				new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope")
			};

			return await _queryService()
				.Header("Authorization", "Basic " + _base64Auth)
				.Post(ct, Constants.Ebay.Url, new FormUrlEncodedContent(parameters))
				.ToEntity<ApplicationTokenData>(ct);
		}

		public async Task<UserTokenData> GetUserToken(CancellationToken ct, string ruName)
		{
			//await Launcher.LaunchUriAsync(new Uri(Constants.Ebay.SignInUrl));

			var authCode = await GetAuthorizationCode(ct);

			var parameters = new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("redirect_uri", ruName),
				new KeyValuePair<string, string>("code", authCode)
			};

			var token = await _queryService()
				.Header("Authorization", "Basic " + _base64Auth)
				.Post(ct, Constants.Ebay.Url, new FormUrlEncodedContent(parameters))
				.ToEntity<UserTokenData>(ct);

			return token;
		}

		public async Task<UserTokenData> RefreshUserToken(CancellationToken ct, UserTokenData currentToken)
		{
			var parameters = new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("refresh_token", currentToken.RefreshToken)
			};

			return await _queryService()
				.Header("Authorization", "Basic " + _base64Auth)
				.Post(ct, Constants.Ebay.Url, new FormUrlEncodedContent(parameters))
				.ToEntity<UserTokenData>(ct);
		}

		private async Task<string> GetAuthorizationCode(CancellationToken ct)
		{
			var response = await _queryService()
				.Get(ct, Constants.Local.RetrieveUrl);

			return await response.Content.ReadAsStringAsync();
		}

		public async Task GetInventory(CancellationToken ct, UserTokenData userToken, int limit, int offset)
		{
			var req = await _queryService()
				.Header("Authorization", "Bearer " + userToken.AccessToken)
				.Get(ct, string.Format(Constants.Ebay.Inventory, limit, offset));
		}
	}
}
