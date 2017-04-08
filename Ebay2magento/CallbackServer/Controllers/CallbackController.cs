using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CallbackServer.Controllers
{
	[Route("api/Callback")]
	public class CallbackController : Controller
	{
		private IMemoryCache _cache;

		public CallbackController(IMemoryCache memoryCache)
		{
			_cache = memoryCache;
		}

		[HttpGet("status")]
		public string GetStatus()
		{
			return "On!";
		}

		[HttpGet("set")]
		public string Get(string state, string code)
		{
			_cache.Set("state", state);
			_cache.Set("code", code);

			return "Code was set successfully!";
		}

		[HttpGet("retrieve")]
		public string Get()
		{
			while (true)
			{
				if (_cache.TryGetValue("code", out string code) && _cache.TryGetValue("state", out string state))
				{
					_cache.Remove("code");
					_cache.Remove("state");

					return code;
				}
			}
		}
	}
}
