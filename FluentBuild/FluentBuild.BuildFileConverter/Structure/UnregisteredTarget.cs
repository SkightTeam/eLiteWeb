using System;
using System.Collections.Generic;
using FluentBuild.BuildFileConverter.Parsing;

namespace FluentBuild.BuildFileConverter.Structure
{
    public class UnregisteredTarget : ITarget
    {
        //This class is used internally to represent a known target
        //that has not yet been registered with the TargetRepository
        
       
        public string Name { get; set; }
        
        private ITarget GetTarget
        {
        get
        {
            var resolve = TargetRepository.ResolveBy(Name);
            if (resolve.GetType() == typeof(UnregisteredTarget))
                throw new ApplicationException("Could not find target " + Name);
            return resolve;
        }
        }

        public string Body
        {
            get { return GetTarget.Body; }
            set { throw new NotImplementedException(); }
        }

        public IList<ITaskParser> Tasks
        {
            get { return GetTarget.Tasks; }
            set { throw new NotImplementedException(); }
        }

        public IList<ITarget> DependsOn
        {
            get { return GetTarget.DependsOn; }
            set { throw new NotImplementedException(); }
        }
    }
}