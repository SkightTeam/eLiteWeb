namespace FluentBuild.AssemblyInfoBuilding
{
    internal interface IAssemblyInfoBuilder
    {
        string Build(IAssemblyInfoDetails details);
    }
}