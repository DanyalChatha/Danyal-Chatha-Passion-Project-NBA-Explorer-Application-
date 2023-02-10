using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Danyal_Chatha_Passion_Project.Startup))]
namespace Danyal_Chatha_Passion_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
