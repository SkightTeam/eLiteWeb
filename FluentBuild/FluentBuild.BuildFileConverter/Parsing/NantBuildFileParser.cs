using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class NantBuildFileParser
    {
        public BuildProject ParseDocument(XDocument document)
        {
            var buildProject = new BuildProject();
            var parser = new TargetParser();

            var projectNode = document.Root; //get the project node
            if (projectNode == null)
                throw new ApplicationException("Could not open root node");

            if (projectNode.Attribute("default") != null) 
                buildProject.DefaultTarget = projectNode.Attribute("default").Value;

            foreach (var childNode in projectNode.Elements("property"))
            {
                var property = new Property(childNode.Attribute("name").Value, childNode.Attribute("value").Value);
                buildProject.AddProperty(property);
            }

            foreach (var childNode in projectNode.Elements("target"))
            {
                buildProject.Targets.Add(parser.Parse(childNode, buildProject));
                //childNode.Attribute("name").Value.Replace(".", "_"), childNode.ToString());
            }

            IEnumerable<XElement> unkownElements = from XElement unkownElement in projectNode.Elements()
                                                   where
                                                       unkownElement.Name != "target" &&
                                                       unkownElement.Name != "property"
                                                   select unkownElement;

            foreach (XElement unkownElement in unkownElements)
            {
                buildProject.Unkown.Add(unkownElement.Name.ToString(), unkownElement.ToString());
            }
            return buildProject;
        }
    }
}