using System;
using System.Web;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public class WebOutputImpl : WebOutput
    {
        private HttpContext context;

        public WebOutputImpl(HttpContext context)
        {
            this.context = context;
        }


        public void Display<T>(View view, T model)
        {
            Container.Current.get_a<ViewEngin>().display(view, model, context.Items);
        }

        public void Display(View view)
        {
            Container.Current.get_a<ViewEngin>().display(view, new object(), context.Items);
        }
    }
}