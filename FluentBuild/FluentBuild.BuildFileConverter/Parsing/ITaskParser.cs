using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public interface ITaskParser
    {
        void Parse(XElement data, BuildProject buildProject);
        string GererateString();
    }
}