using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter
{
    public class TargetTreeBuilder
    {
        public static IList<ITarget> CreateTree(ITarget target)
        {
            var final = new List<ITarget>();
            final.AddRange(BuildDependancyTree(target.DependsOn));
            final.Add(target);
            return final;
        }

        private static List<ITarget> BuildDependancyTree(IList<ITarget> dependancies)
        {
            var final = new List<ITarget>();
            foreach (var dependancy in dependancies)
            {
                var buildDependancyTree = BuildDependancyTree(dependancy.DependsOn);
                foreach (var childDependancy in buildDependancyTree)
                {
                    if (!final.Contains(childDependancy))
                        final.Add(childDependancy);    
                }
                final.Add(dependancy);
            }
            return final;
        }
    }
}
