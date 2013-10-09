using System;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class CallParser : ITaskParser
    {
        public string Target { get; set; }

        #region ITaskParser Members

        public void Parse(XElement data, BuildProject buildProject)
        {
            Target = data.Attribute("target").Value;
        }

        public string GererateString()
        {
            return TargetParser.GetNameOfTarget(Target) + "();";
        }

        #endregion
    }
}