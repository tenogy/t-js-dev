using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Tjs
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder().
				UseKestrel().
				UseWebRoot(".").
				UseContentRoot(Directory.GetCurrentDirectory()).
				UseIISIntegration().
				UseStartup<Startup>().
				Build();
			host.Run();
		}
	}
}
