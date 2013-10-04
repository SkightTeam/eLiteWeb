using System.Collections.Specialized;
using System.Web;
using Skight.eLiteWeb.Domain;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public class WebInputImpl:WebInput
    {
        private HttpContext context;
        private static ConventionPayloader payloader=new ConventionPayloader();

        public WebInputImpl(HttpContext context)
        {
            this.context = context;
        }

        public string RequestPath { get { return context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "/"); } }
        public T Read<T>()
        {
            var name_values =new NameValueCollection( context.Request.Form);
            name_values.Add(context.Request.QueryString);
            var result = payloader.read<T>(name_values);
            return result;
        }
    }
}