using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace FluentBuild.BuildFileConverter.Structure
{    
    public class BuildProject
    {
        public readonly Dictionary<string, Property> Properties;
        public readonly List<Target> Targets;
        public readonly NameValueCollection Unkown;

        public BuildProject()
        {
            Properties = new Dictionary<string, Property>();
            Targets = new List<Target>();
            Unkown = new NameValueCollection();
        }

        public string DefaultTarget { get; set; }

        public void AddProperty(Property property)
        {
            if (Properties.ContainsKey(property.Name))
                Properties[property.Name] = property;
            else
                Properties.Add(property.Name, property);
        }
    }
}