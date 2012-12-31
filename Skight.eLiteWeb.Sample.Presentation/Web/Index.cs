using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Sample.Presentation.Web
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Index:DiscreteCommand
    {
        public void process(WebRequest request)
        {
            
            request.Output.Display(new View("Index"),
                @" <h3>卓越之行</h3>
<p>宏卓科技公司专注于最新软件开发技术、开发流程和业务服务。让所有这些技术为了一个目标---您的业务服务. </p>
<ul>
    <li> 使用行为/测试驱动方式追溯需求，驱动开发,不丢需求 </li>
    <li> 利用敏捷流程提高用户体验，降低风险 </li>
    <li> 使用良好的架构提高系统的扩展性和维护性，同时降低开发的可变成本 </li>
    <li> 利用对业务流程的深入了解，开发适用软件，提供业务服务，使服务与软件无缝结合、同步发展。</li>
<ul>
<p>
终极目标：动成长软件，让我们的系统与你公司的业务一起成长。
</p>
"
                );
        }
    }
}