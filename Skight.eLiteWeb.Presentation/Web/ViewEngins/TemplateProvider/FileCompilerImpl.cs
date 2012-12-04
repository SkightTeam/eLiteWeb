using System;
using System.IO;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
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