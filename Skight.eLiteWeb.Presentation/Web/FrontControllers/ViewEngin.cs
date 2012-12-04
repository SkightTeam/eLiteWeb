using System.Collections;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public interface ViewEngin
    {
        void display<T>(View view, T model, IDictionary context);
        void display(View view, IDictionary context);
        void redirect(string url);
        
    }
}