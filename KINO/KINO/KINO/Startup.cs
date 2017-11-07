using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KINO.Startup))]
namespace KINO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
