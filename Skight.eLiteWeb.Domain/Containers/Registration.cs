namespace Skight.eLiteWeb.Domain.Containers
{
    public interface Registration
    {
        void register<Contract, Implementaion>() where Implementaion : Contract;
    }
}