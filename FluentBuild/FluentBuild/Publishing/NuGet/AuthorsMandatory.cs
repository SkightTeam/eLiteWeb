using System;

namespace FluentBuild.Publishing.NuGet
{
    public class AuthorsMandatory : OptionBase
    {
        public AuthorsMandatory(NuGetPublisher parent) : base(parent) { }

        public ApiKeyMandatory Author(string author)
        {
            _parent._authors = author;
            return new ApiKeyMandatory(_parent);
        }

        public ApiKeyMandatory Authors(params string[] authors)
        {
            if (authors.Length==0)
                throw new ArgumentException("At least one author must be specified");
            if (authors.Length == 1)
                return Author(authors[0]);

            var tmpAuthors = "";
            foreach (var author in authors)
            {
                tmpAuthors += author + ", ";
            }
            tmpAuthors = tmpAuthors.Remove(tmpAuthors.Length - 2);

            _parent._authors = tmpAuthors;
            return new ApiKeyMandatory(_parent);
        }
    }
}