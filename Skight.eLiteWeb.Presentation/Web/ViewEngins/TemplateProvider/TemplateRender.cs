using System.Collections;
using System.IO;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class TemplateRender
    {
        private TemplateGenerator generator;

        public TemplateRender(TemplateGenerator generator)
        {
            this.generator = generator;
        }

        public string Render(IDictionary context, string templatePath)
        {
            var instance = generator.generate(context, templatePath);
            string result;

            instance.Execute();

            if (instance.Layout != null) {
                result = RenderMasterView(context, templatePath, instance);
            } else {
                result = instance.Result;
            }

            return result;
        }

        public string Render<T>(T model, IDictionary context, string templatePath) {
            var instance = generator.generate(model, context, templatePath);

            string result ;

            instance.Execute();

            if (instance.Layout != null) {
                result = RenderMasterView(context, templatePath, instance);
            } else {
                result = instance.Result;
            }

            return result;
        }

      
        private string RenderMasterView(IDictionary context, string templatePath, TemplateBase instance) {
            var masterPath = Helpers.ResolveTemplatePath(instance.Layout, new[] { Path.GetDirectoryName(templatePath) });

            var masterInstance = generator.generate(context, masterPath);
            //RenderBody is a func that we can overwrite
            masterInstance.RenderBody = () => instance.Result;

            masterInstance.Execute();

            return masterInstance.Result;
        }
    }
}