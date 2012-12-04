using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using Microsoft.CSharp;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider.Razor
{
    public class RazorCompiler : Compiler
    {

        public Type compile_template(Stream stream)
        {
           return compile(stream, typeof (TemplateBase));
        }
        public Type compile_template<T>(Stream stream)
        {
            return compile(stream, typeof (TemplateBase<T>));
        }

        private Type compile(Stream stream,Type base_type)
        {
            var key = "c" + Guid.NewGuid().ToString("N");

            var parser = new HtmlMarkupParser();

            var host = new RazorEngineHost(new CSharpRazorCodeLanguage(), () => parser)
            {
                DefaultBaseClass = base_type.FullName,
                DefaultClassName = key,
                DefaultNamespace = "Skight.Arch.Presentation.Web.Core.ViewEngins.Razor.dynamic",
                GeneratedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral", "WriteTo", "WriteLiteralTo", "tinyweb.viewengine.razor.RazorCompiler.TemplateBase")
            };

            //always include this one

            host.NamespaceImports.Add("Skight.Arch.Presentation.Web.Core.ViewEngins");
            host.NamespaceImports.Add("System");

            //read web.config pages/namespaces
            if (File.Exists("\\web.config")) {
                var config = WebConfigurationManager.OpenWebConfiguration("\\web.config");
                var pages = config.GetSection("system.web/pages");
                if (pages != null) {
                    PagesSection pageSection = (PagesSection)pages;
                    for (int i = 0; i < pageSection.Namespaces.Count; i++) {
                        //this automatically ignores namespaces already added
                        host.NamespaceImports.Add(pageSection.Namespaces[i].Namespace);
                    }
                }
            }

            CodeCompileUnit code;
            using (var reader = new StreamReader(stream)) {
                var generatedCode = new RazorTemplateEngine(host).GenerateCode(reader);
                code = generatedCode.GeneratedCode;
            }

            var @params = new CompilerParameters
            {
                IncludeDebugInformation = false,
                TempFiles = new TempFileCollection(AppDomain.CurrentDomain.DynamicDirectory),
                CompilerOptions = "/target:library /optimize",
                GenerateInMemory = false
            };

            var assemblies = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(a => !a.IsDynamic)
               .Select(a => a.Location)
               .ToArray();

            @params.ReferencedAssemblies.AddRange(assemblies);

            var provider = new CSharpCodeProvider();
            var compiled = provider.CompileAssemblyFromDom(@params, code);

            if (compiled.Errors.Count > 0) {
                var compileErrors = string.Join("\r\n", compiled.Errors.Cast<object>().Select(o => o.ToString()));
                throw new ApplicationException("Failed to compile Razor:" + compileErrors);
            }

            return compiled.CompiledAssembly.GetType("Skight.Arch.Presentation.Web.Core.ViewEngins.Razor.dynamic." + key);
        }
    }
}