namespace Skight.eLiteWeb.Domain
{
    public interface Registration
    {
        void register<Contract, Implementaion>() where Implementaion : Contract;
    }
}