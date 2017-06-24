using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DealCrash.Startup))]
namespace DealCrash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
