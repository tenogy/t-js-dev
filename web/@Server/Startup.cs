using Microsoft.AspNetCore.Builder;

namespace Tjs
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app)
		{
			app.UseDefaultFiles();
			app.UseStaticFiles();
		}
	}
}
