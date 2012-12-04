using System;
using System.IO;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    public interface Compiler
    {
        Type compile_template(Stream stream);
        Type compile_template<T>(Stream stream);
    }
}