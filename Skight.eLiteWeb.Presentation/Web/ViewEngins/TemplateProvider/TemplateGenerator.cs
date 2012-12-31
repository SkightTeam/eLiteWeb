using System;
using System.Collections;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class TemplateGenerator
    {
        private CachedFileCompiler internal_compiler;

        public TemplateGenerator(CachedFileCompiler internalCompiler)
        {
            internal_compiler = internalCompiler;
        }

        public TemplateBase<T> generate<T>(T model, IDictionary context, string path)
        {
            var type= internal_compiler.compile_template<T>(path);
            var instance = (TemplateBase<T>)Activator.CreateInstance(type);
            instance.Path = path;
            instance.Model = model;
            instance.Context = context;
            return instance;
        }

        public TemplateBase generate(IDictionary context, string path) {
            var type = internal_compiler.compile_template(path);
            var instance = (TemplateBase)Activator.CreateInstance(type);
            instance.Path = path;
            instance.Context = context;
            return instance;
        }
    }
}