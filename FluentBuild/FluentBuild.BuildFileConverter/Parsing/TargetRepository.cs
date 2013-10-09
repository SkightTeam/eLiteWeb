using System.Collections.Generic;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public interface ITargetRepository
    {
        ITarget Resolve(string name);
        void Register(Target target);
    }

    class TargetRepository : ITargetRepository
    {
        private static readonly IDictionary<string, Target> _knownTargets;

        static TargetRepository()
        {
            _knownTargets = new Dictionary<string, Target>();
        }

        public static void ClearKnownTargets()
        {
            _knownTargets.Clear();
        }

        public static ITarget ResolveBy(string name)
        {
            if (!_knownTargets.ContainsKey(name))
            {
                return new UnregisteredTarget() { Name = name };
            }

            return _knownTargets[name];
        }

        public ITarget Resolve(string name)
        {
            return TargetRepository.ResolveBy(name);
        }


        public void Register(Target target)
        {
            TargetRepository.RegisterTarget(target);
        }

        public static void RegisterTarget(Target target)
        {
            //check each previously registered targets dependancies
            //if it depends on the target we are registering then
            //ensure that the dependancy is updated (from unregisteredTarget to an actual Target)
            //foreach (var knownTarget in _knownTargets.Values)
            //{
            //    for (var index = 0; index < knownTarget.DependsOn.Count; index++)
            //    {
            //        if (knownTarget.DependsOn[index].Name == target.Name)
            //        {
            //            knownTarget.DependsOn[index] = target;
            //        }
            //    }
            //}
            _knownTargets.Add(target.Name, target);
        }
    }
}
