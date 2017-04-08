namespace ApplicationFramework
{
	public static class Constants
	{
		public static class Settings
		{
			public const string CertID = nameof(CertID);
			public const string AppID = nameof(AppID);
			public const string RuName = nameof(RuName);
			public const string UserToken = nameof(UserToken);
			public const string ApplicationToken = nameof(ApplicationToken);
		}

		public static class Ebay
		{
#if SANDBOX
			public const string Url = "https://api.sandbox.ebay.com/identity/v1/oauth2/token";
#else
			public const string Url = "https://api.ebay.com/identity/v1/oauth2/token";
#endif

			public const string SignInUrl = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame=Denis_Railan-DenisRai-dc7a-4-tyvxxgew&oauthparams=%26state%3Dnull%26client_id%3DDenisRai-dc7a-4b50-92d8-77617cf67e96%26redirect_uri%3DDenis_Railan-DenisRai-dc7a-4-tyvxxgew%26response_type%3Dcode%26device_id%3Dnull%26display%3Dnull%26scope%3Dhttps%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.marketing.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.marketing+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.inventory.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.inventory+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.account.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.account+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.fulfillment.readonly+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.fulfillment+https%3A%2F%2Fapi.ebay.com%2Foauth%2Fapi_scope%2Fsell.analytics.readonly%26tt%3D1";

			public const string Inventory = "https://api.ebay.com/sell/inventory/v1/inventory_item?limit={0}&offset={1}";
		}

		public static class Magento
		{
			public const string Url = "http://yzinstruments4.life/index.php/rest/";
		}

		public static class Local
		{
			public const string TestUrl = "https://localhost:5001/api/callback/status";
			public const string RetrieveUrl = "https://yzinstruments4.life/ebay/callback.php?retrieve=1";
		}
	}
}
