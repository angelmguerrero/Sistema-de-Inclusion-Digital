using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Educafin.Startup))]
namespace Educafin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
