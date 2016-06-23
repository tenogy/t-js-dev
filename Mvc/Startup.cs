using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Mvc.Startup))]
namespace Mvc
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
		}
	}
}
