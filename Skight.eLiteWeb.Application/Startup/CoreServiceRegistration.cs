using System.Web;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class CoreServiceRegistration:StartupCommand
    {
        private readonly Registration registration;

        public CoreServiceRegistration(Registration registration)
        {
            this.registration = registration;
        }

        public void run()
        {
            registration.register<WebServerRenderAction>(create_render_action);
            registration.register<WebClientRedirectAction>(create_redirect_action);
        }
        WebServerRenderAction create_render_action()
        {
            return x => HttpContext.Current.Response.Write(x);
        }

        private WebClientRedirectAction create_redirect_action() {
            return x => HttpContext.Current.Response.Redirect(x, false);
        }

    }
}