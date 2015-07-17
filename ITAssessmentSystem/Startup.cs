using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ITAssessmentSystem.Startup))]
namespace ITAssessmentSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
