using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jfrost_bugtracker.Startup))]
namespace jfrost_bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
