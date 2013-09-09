namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public interface WebOutput
    {
        void Display<T>(View view, T model);
        void Display(View view);
    }
}