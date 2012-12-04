using System;
using System.Collections.Concurrent;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    public class CachedFileCompiler:FileCompiler
    {
        //cache of already compiled types
        ConcurrentDictionary<Tuple<string, Type>, Type> cache = new ConcurrentDictionary<Tuple<string, Type>, Type>();
        ConcurrentDictionary<string, Type> cache_for_no_model = new ConcurrentDictionary<string, Type>();

        private FileCompilerImpl internal_compiler;

        public CachedFileCompiler(FileCompilerImpl internalCompiler)
        {
            internal_compiler = internalCompiler;
        }

        public Type compile_template<T>(string path)
        {
            var key = Tuple.Create(path, typeof(T));
            Type type;

            if (!cache.TryGetValue(key, out type)) 
            {
                type =internal_compiler.compile_template<T>(path);
                cache[key] = type;
            }
            return type;
        }

        public Type compile_template(string path) 
        {
            Type type;
            if (!cache_for_no_model.TryGetValue(path, out type)) {
                type =internal_compiler.compile_template(path);
                cache_for_no_model[path] = type;
            }
           return type;
        }  
    }
}