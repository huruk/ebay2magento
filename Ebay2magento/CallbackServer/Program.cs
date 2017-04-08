using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.DotNet.PlatformAbstractions;

namespace CallbackServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var certFile = ApplicationEnvironment.ApplicationBasePath + "\\ebay2magento.pfx";
			var signingCertificate = new X509Certificate2(certFile, "ebay2magento");

			var host = new WebHostBuilder()
				.UseKestrel(options =>
					options.UseHttps(signingCertificate)
				)
				.UseUrls("http://localhost:5000;https://localhost:5001")
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
