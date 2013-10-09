using System.Collections.Generic;
using System.Linq;

namespace FluentBuild.BuildFileConverter.Structure
{
    public class PropertyResolver
    {
        public static IList<Property> Properties { get; set; }

        //analzies the given property and its dependancies
        //and anything that depends on the property to further discover its type
        public static void RecomputeTypeFor(Property property)
        {
            var thisPropertyDependsOnCount = (from Property p in Properties
                        where property.DependsOnProperty.Contains(p.Name)
                        select p).Count();

            var otherPropertiesDependOnThisPropertyCount = (from Property p in Properties
                                                            where p.DependsOnProperty.Contains(property.Name)
                                                            select p).Count();


        }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertyType Type { get; set; }
        public IList<string> DependsOnProperty { get; set; }

        public Property(string name, string value)
        {
            Name = name.Replace(".", "_");
            Value = value;
            DependsOnProperty = new List<string>();

            var startIndex = value.IndexOf("${");
            if (startIndex > -1)
            {
                var dependancy = value.Substring(startIndex+2, value.IndexOf("}")-2-startIndex).Replace(".", "_");
                DependsOnProperty.Add(dependancy);
            }

            var lastDot = value.LastIndexOf('.');
            //if the dot appears after the last \ then assume it is a file
            if (lastDot > value.LastIndexOf('\\'))
            {
                Type = PropertyType.File;
            }
            else 
            {
                Type = PropertyType.Directory;
            }

           //PropertyResolver.Properties.Add(this);
        }
    }
}