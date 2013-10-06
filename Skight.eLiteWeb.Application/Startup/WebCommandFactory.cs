using System;
using Skight.eLiteWeb.Application.CommandDecorators;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.CommandFilters;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class WebCommandFactory {
        public Command create_from(DiscreteCommand command)
        {
            
            var result= new CommandImpl(
                    new NameConventionFilter(command),
                        new TransactionDecorator(command));
            return result;
          
        }
         
    }
}