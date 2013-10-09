using System;
using System.Reflection;

namespace FluentBuild.Utilities
{
    public interface IActionExcecutor
    {

        void Execute<T>(Action<T> args) where T : InternalExecutable, new();
        void Execute<T>(Func<T, object> args) where T : InternalExecutable, new();
        
        void ExecuteFailable<T>(Func<T, object> args) where T : FailableInternalExecutable<T>, new();

        void Execute<T, TParams>(Action<T> args, TParams constructorParms) where T : InternalExecutable;
        void Execute<T, TParam, TParam2>(Action<T> args, TParam constructorParam1, TParam2 constructorParam2) where T : InternalExecutable;
    }

    internal class ActionExcecutor : IActionExcecutor
    {
        public void Execute<T>(Action<T> args) where T : InternalExecutable, new()
        {
            var concrete = new T();
            args(concrete);
            concrete.InternalExecute();
        }
        
        //allows running a property getter e.g. ContinueOnError so that we don't need a ContinueOnError() method
        public void Execute<T>(Func<T, object> args) where T : InternalExecutable, new()
        {
            var concrete = new T();
            args(concrete);
            concrete.InternalExecute();
        }

        public void ExecuteFailable<T>(Func<T, object> args) where T : FailableInternalExecutable<T>, new()
        {
            var concrete = new T();
            args(concrete);
            concrete.InternalExecute();
        }

        public ConstructorInfo FindConstructor<T,TParams,TParam2>()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            //try for a concrete implementation
            var constructor = typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(TParams), typeof(TParam2) }, null);

            //If that did not work check the base type for a concrete match
            if (constructor == null)
                constructor = typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(TParams).BaseType, typeof(TParam2).BaseType }, null);

            //no matches found
            if (constructor == null)
                throw new ApplicationException("Could not find a matching constructor");
            return constructor;
        }

        public ConstructorInfo FindConstructor<T,TParams>()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            //try for a concrete implementation
            var constructor = typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(TParams) }, null);
            
            //If that did not work check the base type for a concrete match
            if (constructor == null)
                constructor = typeof (T).GetConstructor(bindingFlags, null, new[] {typeof (TParams).BaseType}, null);
            
            //if that did not work check all interfaces. Return the first matching
            if (constructor == null)
            {
                foreach (var i in typeof(TParams).GetInterfaces())
                {
                    
                    constructor = typeof(T).GetConstructor(bindingFlags , null,  new[] { i }, null);
                    if (constructor != null)
                        return constructor;
                }
            }

            //no matches found
            if (constructor == null)
                throw new ApplicationException("Could not find a matching constructor");
            return constructor;
        }

        public void Execute<T, TParams>(Action<T> args, TParams constructorParms) where T : InternalExecutable
        {
            var concrete = (T)FindConstructor<T,TParams>().Invoke(new object[] { constructorParms });
            args(concrete);
            concrete.InternalExecute();
        }

        public void Execute<T, TParam, TParam2>(Action<T> args, TParam constructorParam1, TParam2 constructorParam2) where T : InternalExecutable
        {
            var concrete = (T)FindConstructor<T, TParam, TParam2>().Invoke(new object[] { constructorParam1, constructorParam2 });
            args(concrete);
            concrete.InternalExecute();
        }
    }
}