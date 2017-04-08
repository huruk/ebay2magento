using Newtonsoft.Json;
using System.ComponentModel;

namespace Ebay2Magento.Client.Entities
{
	[Bindable(BindableSupport.Yes)]
	public class TokenData
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("expires_in")]
		public string ExpiresIn { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }
	}
}
