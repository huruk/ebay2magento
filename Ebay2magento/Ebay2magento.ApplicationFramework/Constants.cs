using Ebay2magento.ApplicationFramework.Entities;
using System;

namespace ApplicationFramework
{
	public static class Constants
	{
		public static class Settings
		{
			public const string CertID = nameof(CertID);
			public const string AppID = nameof(AppID);
			public const string RuName = nameof(RuName);
			public const string DevID = nameof(DevID);
			public const string UserToken = nameof(UserToken);
			public const string ApplicationToken = nameof(ApplicationToken);
			public const string MagentoToken = nameof(MagentoToken);
			public const string MagentoUrl = nameof(MagentoUrl);

			public const string EbayContext = nameof(EbayContext);
			public const string MagentoContext = nameof(MagentoContext);
		}

		public static class Ebay
		{
			public const string ApiUrl = "https://api.ebay.com/ws/api.dll";
			public const string SignInUrl = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame=";
		}

		public static class Local
		{
			public const string TestUrl = "https://localhost:5001/api/callback/status";
			public const string RetrieveUrl = "https://yzinstruments4.life/ebay/callback.php?retrieve=1";
		}
	}
}
