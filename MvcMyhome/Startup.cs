using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcMyhome.Startup))]
namespace MvcMyhome
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
