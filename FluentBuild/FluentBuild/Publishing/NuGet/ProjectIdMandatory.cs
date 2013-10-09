namespace FluentBuild.Publishing.NuGet
{
    public class ProjectIdMandatory : OptionBase
    {
        public ProjectIdMandatory(NuGetPublisher parent) : base(parent) { }

        public VersionMandatory ProjectId(string id)
        {
            _parent._projectId = id;
            return new VersionMandatory(this._parent);
        }
    }
}