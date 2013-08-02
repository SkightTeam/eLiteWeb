using System;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.CommandFilters
{
    public class NameConventionFilter : CommandFilter
    {
        private string command_name;
        private string command_full_name;
        private readonly string suffix = ".do";
        public NameConventionFilter(DiscreteCommand internalCommand)
        {
            var type = internalCommand.GetType();
            command_full_name= type.FullName;
            command_name = type.Name;

        }

        public bool can_process(WebRequest request)
        {
            var request_list = request.Input.RequestPath.Split('/');
            var request_last = request_list[request_list.Length-1];
            return command_name + suffix==request_last;
        }
    }
}