using System;
using System.Collections;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
 
    public class TemplateViewEngin:ViewEngin
    {
        private readonly WebClientRedirectAction web_client;
        private readonly WebServerRenderAction web_render;
        private readonly TemplateRender render;

        public TemplateViewEngin(WebClientRedirectAction webClient, WebServerRenderAction webRender, TemplateRender render)
        {
            web_client = webClient;
            web_render = webRender;
            this.render = render;
        }

        public void display(View view,  IDictionary context) {
            var fullpath = Helpers.ResolveTemplatePath(view);
            if (string.IsNullOrEmpty(fullpath)) {
                throw new ApplicationException(string.Format("Connot resolve template path {0}", view));
            }
            web_render(render.Render(context, fullpath));
        }
        public void display<T>(View view, T model, IDictionary context)
        {
            var fullpath = Helpers.ResolveTemplatePath(view);
            if(string.IsNullOrEmpty( fullpath))
            {
                throw new ApplicationException(string.Format("Connot resolve template path {0}", view)); 
            }
            web_render(render.Render(model,context, fullpath));
        }

        public void redirect(string url)
        {
            web_client(url);
        }

       
    }
}