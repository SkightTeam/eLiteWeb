using System;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.CommandFilters
{
    public class NameConventionFilter : CommandFilter
    {
        private string command_name;

        public NameConventionFilter(DiscreteCommand internalCommand)
        {
            var type = internalCommand.GetType();
            command_name = type.FullName;
        }

        public bool can_process(WebRequest request)
        {
            Console.WriteLine("Command Name {0}", command_name);
            Console.WriteLine("Request Path {0}", request.Input.RequestPath);
            return request.Input.RequestPath == command_name;
        }
    }
}