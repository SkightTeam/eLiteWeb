using System;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    public interface FileCompiler
    {
        Type compile_template<T>(string path);
        Type compile_template(string path);
    }
}