namespace FluentBuild.Publishing.NuGet
{
    public class DescriptionMandatory : OptionBase
    {
        public DescriptionMandatory(NuGetPublisher parent) : base(parent) { }

        public AuthorsMandatory Description(string description)
        {
            _parent._description = description;
            return new AuthorsMandatory(_parent);
        }
    }
}