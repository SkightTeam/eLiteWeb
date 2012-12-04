using System;
using System.IO;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider.Razor
{
    public class RazorCompilerSpecs:Specification<RazorCompiler>
    {
        private Establish context =
            () =>
                {
                    template = new MemoryStream();
                    writer = new StreamWriter(template);
                };
        private Cleanup it =
            () =>
                {
                    writer.Dispose();
                    template.Dispose();
                };

        protected static MemoryStream template;
        protected static StreamWriter writer;
        protected static Type type;

    }

    public class When_compile_all_html_against_none_modal_template : RazorCompilerSpecs
    {
        private Establish context =
            () => writer.Write(
                @"<!doctype html> 
<meta charset='utf-8' />
<html>
<head>
   
</head>
<body>
    
    <header >
      <h1>Skight I-Tech Inc.</h1>
     </header>
</body>
</html>
");
        private Because of = () => type = subject.compile_template(template);
        private It should_generate_a_type_inherite_from_template_base =
         () => type.BaseType.ShouldEqual(typeof(TemplateBase));

        private It should_generate_a_type__not_inherite_from_generic_template_base =
        () => type.BaseType.ShouldNotEqual(typeof(TemplateBase<string>));
       
    }

    public class When_compile_a_modal_template:RazorCompilerSpecs
    {
        private Establish context =
            () => writer.Write(
@"
@inherits TemplateBase<string>

@{
  Layout = 'Shared/_Layout.cshtml';
}
<!DOCTYPE html>

Message：@(Model)。
"
                );
        private Because of = () => type = subject.compile_template<string>(template);
        private It should_generate_a_type_inherite_from_generic_template_base =
        () => type.BaseType.ShouldEqual(typeof (TemplateBase<string>));
    }
}