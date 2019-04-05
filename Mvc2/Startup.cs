using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mvc2.Startup))]
namespace Mvc2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
