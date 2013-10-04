using System.Collections.Generic;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;
using Skight.HelpCenter.Domain;
using Skight.HelpCenter.Presentation.Services;

namespace Skight.HelpCenter.Presentation
{
     [RegisterInContainer(LifeCycle.singleton)]
    public class Install : DiscreteCommand
     {
         private TopicHosterService service;

         public Install(TopicHosterService service)
         {
             this.service = service;
         }

         public void process(WebRequest request)
        {
            Sentence sentence = "这是一个回答";
            var tags = new List<Keyword> {"测试"};
            service.answer(sentence, tags);
            request.Output.Display(new View("Install.cshtml"));
        }
    }
}