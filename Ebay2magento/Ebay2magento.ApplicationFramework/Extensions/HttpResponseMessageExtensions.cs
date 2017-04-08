using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationFramework.Extensions
{
	public static class HttpResponseMessageExtensions
	{
		public static Task<T> ToEntity<T>(this Task<HttpResponseMessage> task, CancellationToken ct)
		{
			return Task.Run(async () =>
			{
				var result = await task;
				var json = await result.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<T>(json);
			}, ct);
		}
	}
}
