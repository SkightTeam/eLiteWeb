using System;

namespace Skight.eLiteWeb.Domain.Containers
{
    public interface Registration
    {
        void register<Contract, Implementaion>() where Implementaion : Contract;
        void register(DiscreteItemResolver resolver, params Type[] contracts);
    }
}