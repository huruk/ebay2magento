using Newtonsoft.Json;
using System.ComponentModel;

namespace Ebay2Magento.Client.Entities
{
	[Bindable(BindableSupport.Yes)]
	public class UserTokenData : TokenData
	{
		[JsonProperty("refresh_token_expires_in")]
		public string RefreshTokenExpiresIn { get; set; }
	}
}
