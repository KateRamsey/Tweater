using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tweater.Startup))]
namespace Tweater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
