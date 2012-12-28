using System.Text.RegularExpressions;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.CommandFilters
{
    public class RegularExpressFilter:CommandFilter
    {
        private readonly Regex regex;

        public RegularExpressFilter(string match)
        {
            regex = new Regex(match);
        }

        public bool can_process(WebRequest request)
        {
            return regex.IsMatch(request.Input.RequestPath);
        }
    }
}