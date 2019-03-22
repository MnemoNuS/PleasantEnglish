using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PleasantEnglish.Startup))]
namespace PleasantEnglish
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			ConfigureAuth(app);
        }
    }
}
