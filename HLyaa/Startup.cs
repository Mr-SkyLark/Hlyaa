using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HLyaa.Startup))]
namespace HLyaa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
