using System;
using System.IO;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class FileCompilerImpl :FileCompiler
    {
        private Compiler internal_compiler;

        public  FileCompilerImpl(Compiler internalCompiler)
        {
            internal_compiler = internalCompiler;
        }

        protected FileCompilerImpl()
        {
        }

        public virtual  Type compile_template<T>(string path)
        {
            return internal_compiler.compile_template<T>(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public virtual Type compile_template(string path) 
        {
           return internal_compiler.compile_template(new FileStream(path, FileMode.Open, FileAccess.Read));
        } 
    }
}