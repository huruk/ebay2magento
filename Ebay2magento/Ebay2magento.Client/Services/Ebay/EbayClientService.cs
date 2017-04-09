using ApplicationFramework;
using Ebay2Magento.ApplicationFramework.Contracts;
using Ebay2Magento.Client.Contracts.Ebay;
using Ebay2Magento.Client.Entities;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

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

		public async Task<string> GetSessionId(CancellationToken ct, string runame, string devId, string appid, string certId)
		{
			var xmlDoc = new XmlDocument();

			string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <GetSessionIDRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <RuName>" + runame + @"</RuName>
                        </GetSessionIDRequest>";

			var httpContent = new StringContent(strReq, Encoding.UTF8, "text/xml");

			var response = await _queryService()
				.Header("X-EBAY-API-DEV-NAME", devId)
				.Header("X-EBAY-API-APP-NAME", appid)
				.Header("X-EBAY-API-CERT-NAME", certId)
				.Header("X-EBAY-API-COMPATIBILITY-LEVEL", "679")
				.Header("X-EBAY-API-SITEID", "0")
				.Header("X-EBAY-API-CALL-NAME", "GetSessionID")
				.Post(ct, Constants.Ebay.ApiUrl, httpContent);

			var stream = await response.Content.ReadAsStreamAsync();

			using (var sr = new StreamReader(stream))
			{
				xmlDoc.LoadXml(sr.ReadToEnd());
			}

			var ns = new XmlNamespaceManager(xmlDoc.NameTable);
			ns.AddNamespace("ebay", "urn:ebay:apis:eBLBaseComponents");

			var root = xmlDoc["GetSessionIDResponse"];

			var sessionNode = root.SelectSingleNode("//ebay:SessionID", ns);

			var sessionID = string.Empty;
			if (sessionNode != null)
			{
				sessionID = root["SessionID"].InnerText;
			}

			return sessionID;
		}

		public async Task<UserTokenData> GetUserToken(CancellationToken ct, string runame, string devId, string appid, string certId, string sessionId)
		{
			var xmlDoc = new XmlDocument();

			string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <FetchTokenRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <SessionID>" + sessionId + @"</SessionID>
                        </FetchTokenRequest>";

			var httpContent = new StringContent(strReq, Encoding.UTF8, "text/xml");

			var response = await _queryService()
				.Header("X-EBAY-API-DEV-NAME", devId)
				.Header("X-EBAY-API-APP-NAME", appid)
				.Header("X-EBAY-API-CERT-NAME", certId)
				.Header("X-EBAY-API-COMPATIBILITY-LEVEL", "679")
				.Header("X-EBAY-API-SITEID", "0")
				.Header("X-EBAY-API-CALL-NAME", "FetchToken")
				.Post(ct, Constants.Ebay.ApiUrl, httpContent);

			var stream = await response.Content.ReadAsStreamAsync();

			using (var sr = new StreamReader(stream))
			{
				xmlDoc.LoadXml(sr.ReadToEnd());
			}

			var root = xmlDoc["FetchTokenResponse"];

			if (root["Errors"] != null)
			{
				throw new InvalidOperationException();
			}

			return new UserTokenData()
			{
				AccessToken = root["eBayAuthToken"].InnerText,
				ExpiresIn = root["HardExpirationTime"].InnerText
			};
		}
	}
}
