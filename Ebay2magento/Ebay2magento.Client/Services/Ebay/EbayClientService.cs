using ApplicationFramework;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using Ebay2magento.ApplicationFramework.Entities;
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

		public async Task<string> GetSessionId(CancellationToken ct, EbayContext context)
		{
			var xmlDoc = new XmlDocument();

			string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <GetSessionIDRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <RuName>" + context.RuName + @"</RuName>
                        </GetSessionIDRequest>";

			var httpContent = new StringContent(strReq, Encoding.UTF8, "text/xml");

			var response = await _queryService()
				.Header("X-EBAY-API-DEV-NAME", context.DevID)
				.Header("X-EBAY-API-APP-NAME", context.AppID)
				.Header("X-EBAY-API-CERT-NAME", context.CertID)
				.Header("X-EBAY-API-COMPATIBILITY-LEVEL", "679")
				.Header("X-EBAY-API-SITEID", "0")
				.Header("X-EBAY-API-CALL-NAME", "GetSessionID")
				.Post(ct, Constants.Ebay.ApiUrl, httpContent);

			using (var stream = await response.Content.ReadAsStreamAsync())
			{
				using (var sr = new StreamReader(stream))
				{
					xmlDoc.LoadXml(sr.ReadToEnd());
				}
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

		public async Task<UserTokenData> GetUserToken(CancellationToken ct, EbayContext context, string sessionID)
		{
			var xmlDoc = new XmlDocument();

			string strReq = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <FetchTokenRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                          <SessionID>" + sessionID + @"</SessionID>
                        </FetchTokenRequest>";

			var httpContent = new StringContent(strReq, Encoding.UTF8, "text/xml");

			var response = await _queryService()
				.Header("X-EBAY-API-DEV-NAME", context.DevID)
				.Header("X-EBAY-API-APP-NAME", context.AppID)
				.Header("X-EBAY-API-CERT-NAME", context.CertID)
				.Header("X-EBAY-API-COMPATIBILITY-LEVEL", "679")
				.Header("X-EBAY-API-SITEID", "0")
				.Header("X-EBAY-API-CALL-NAME", "FetchToken")
				.Post(ct, Constants.Ebay.ApiUrl, httpContent);

			using (var stream = await response.Content.ReadAsStreamAsync())
			{
				using (var sr = new StreamReader(stream))
				{
					xmlDoc.LoadXml(sr.ReadToEnd());
				}
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

		public async Task<StoreCustomCategoryTypeCollection> GetCategories(CancellationToken ct, EbayContext context)
		{
			var apiContext = new ApiContext()
			{
				ApiCredential = new ApiCredential()
				{
					ApiAccount = new ApiAccount()
					{
						Application = context.AppID,
						Certificate = context.CertID,
						Developer = context.DevID
					},
					eBayToken = context.Token
				}
			};

			var call = new GetStoreCall(apiContext);
			call.CategoryStructureOnly = true;

			call.Execute();

			return call.Store.CustomCategories;
		}

		public async Task<ItemTypeCollection> GetSellerListIDs(CancellationToken ct, EbayContext context)
		{
			var items = new ItemTypeCollection();

			var apiContext = new ApiContext()
			{
				ApiCredential = new ApiCredential()
				{
					ApiAccount = new ApiAccount()
					{
						Application = context.AppID,
						Certificate = context.CertID,
						Developer = context.DevID
					},
					eBayToken = context.Token
				}
			};

			var call = new GetSellerListCall(apiContext);
			call.DetailLevelList = new DetailLevelCodeTypeCollection(new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll });
			call.EndTimeFrom = DateTime.Today;
			call.EndTimeTo = DateTime.Today.AddDays(120);
			call.Pagination = new PaginationType()
			{
				EntriesPerPage = 5,
				PageNumber = 1
			};

			await Task.Run(() =>
			{
				do
				{
					call.Execute();
					items.AddRange(call.ItemList);
					call.Pagination.PageNumber++;
				} while (call.HasMoreItems);
			}, ct);

			return items;
		}
	}
}
