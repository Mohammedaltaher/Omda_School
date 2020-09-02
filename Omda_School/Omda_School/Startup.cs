using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Omda_School.Startup))]
namespace Omda_School
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
