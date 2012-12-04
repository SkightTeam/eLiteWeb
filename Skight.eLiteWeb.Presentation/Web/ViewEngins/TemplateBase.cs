using System;
using System.Collections;
using System.IO;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins
{

    public abstract class TemplateBase:IDisposable
    {
        public string Layout { get; set; }
        public Func<string> RenderBody { get; set; }
        public string Path { get; internal set; }
        public string Result { get { return Writer.ToString(); } }
        public IDictionary Context { get; set; }

        protected TemplateBase()
        {
           
        }

        public TextWriter Writer
        {
            get
            {
                if(writer==null)
                {writer = new StringWriter();
                }
                return writer;
            }
            set { 
                writer = value;
            }
        }

        private TextWriter writer;

        public void Clear() {
           Writer.Flush();
        }

        public virtual void Execute() { }

        public void Write(object @object) {
            if (@object == null) {
                return;
            }

            Writer.Write(@object);
        }

        public void WriteLiteral(string @string) {
            if (@string == null) {
                return;
            }

            Writer.Write(@string);
        }

        public static void WriteLiteralTo(TextWriter writer, string literal) {
            if (literal == null) {
                return;
            }

            writer.Write(literal);
        }

        public static void WriteTo(TextWriter writer, object obj) {
            if (obj == null) {
                return;
            }

            writer.Write(obj);
        }

        public void Dispose()
        {
            if(writer!=null)
                writer.Dispose();
        }
    }
    public abstract class TemplateBase<T> :TemplateBase
    {
        public T Model { get; set; }
       
      
    }
}