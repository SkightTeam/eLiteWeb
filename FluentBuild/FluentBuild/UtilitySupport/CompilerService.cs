using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentFs.Core;
using Directory = FluentFs.Core.Directory;
using File = System.IO.File;

namespace FluentBuild.UtilitySupport
{
    public class CompilerService
    {
        public static string BuildAssemblyFromSources(string path, string baseDirectory)
        {
            if (!System.IO.Directory.Exists(path))
                throw new DirectoryNotFoundException("Could not find the directory to build of " + path);

            Defaults.Logger.WriteDebugMessage("Sources found in: " + path);
            var fileset = new FileSet();
            fileset = fileset.Include(new Directory(path)).RecurseAllSubDirectories.Filter("*.cs");

            string startPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string fluentBuilddll = Path.Combine(startPath, "FluentBuild.dll");
            
            Defaults.Logger.WriteDebugMessage("Adding in reference to the FluentBuild DLL from: " + fluentBuilddll);
            
            string tempPath = Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\" + DateTime.Now.Ticks;
            //System.IO.Directory.Delete(Environment.GetEnvironmentVariable("TEMP") + "\\FluentBuild\\", true);
            System.IO.Directory.CreateDirectory(tempPath);
            string outputAssembly = Path.Combine(tempPath, "build.dll");


            Defaults.Logger.WriteDebugMessage("Output Assembly: " + outputAssembly);

            var references = new List<String>() { fluentBuilddll };
          
            /*
            var fluentFs = new FluentFs.Core.File(Path.Combine(startPath, "FluentFs.dll"));
            if (File.Exists(fluentFs.ToString()))
            {
                fluentFs.Copy.To(tempPath);
                references.Add(fluentFs.ToString());
            }
             */

            //add in third party references
            var projectParser = new ProjectParser(path, baseDirectory);
            if (projectParser.HasProjectFile())
            {
                var referencesFromProjectFile = projectParser.GetReferences();
                foreach (var referenceFromProjectFile in referencesFromProjectFile)
                {
                    Defaults.Logger.WriteDebugMessage("Adding in third party reference " + referenceFromProjectFile);
                }
                references.AddRange(referencesFromProjectFile);
            }

            Task.Build.Csc.Target.Library(x => x.AddSources(fileset).AddRefences(references.ToArray()).IncludeDebugSymbols.OutputFileTo(outputAssembly));
            return outputAssembly;
        }

        /// <summary>
        /// Builds an assembly from a source folder. Currently this only works with .cs files
        /// </summary>
        /// <param name="path">The path to the source files</param>
        /// <returns>returns the path to the compiled assembly</returns>
        /// <summary>
        /// Executes a DLL.
        /// </summary>
        /// <param name="path">The path to the DLL that has a class that implements IBuild</param>
        /// <param name="classToRun"></param>
        /// <param name="methodsToRun"></param>
        public static string ExecuteBuildTask(string path, string classToRun, IList<string> methodsToRun)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework " + Defaults.FrameworkVersion);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            bool classfound = false;
            foreach (Type t in types)
            {
                if ((t.Name == classToRun) && t.IsSubclassOf(typeof(BuildFile)))
                {
                    classfound = true;
                    return StartRun(assemblyInstance, t, methodsToRun);
                }
            }

            if (!classfound)
            {
                return String.Format("Could not find class {0} that inherits from FluentBuild.BuildFile", classToRun);
            }

            return string.Empty;
        }

        public static IEnumerable<String> FindBuildClasses(string path)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework {0}", Defaults.FrameworkVersion.ToString());
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            return types.Where(t => t.IsSubclassOf(typeof(BuildFile))).Select(t=>t.Name);
        }

        private static string StartRun(Assembly assemblyInstance, Type t, IList<string> methodsToRun)
        {
            var build = (BuildFile)assemblyInstance.CreateInstance(t.FullName);
            Defaults.Logger.WriteHeader("Execute");
            Defaults.Logger.Write("EXECUTE", "Running Class: " + t.FullName);
            if (build.TaskCount == 0)
                return "No tasks were found. Make sure that you add a task in your build classes constructor via AddTask()";
            if (methodsToRun != null && methodsToRun.Count != 0)
            {
                if (!DoAllMethodsExistInType(t, methodsToRun))
                    return "Methods that were specified could not be found in the build file. Ensure the method is Public and spelled correctly";
                build.ClearTasks();

                foreach (string method in methodsToRun)
                {
                    string methodToInvoke = method;
                    var a = new Action(delegate { t.InvokeMember(methodToInvoke, BindingFlags.Default | BindingFlags.InvokeMethod, null, build, null); });
                    build.AddTask(method, a);
                }
            }

            build.InvokeNextTask();
            return string.Empty;
        }

        public static bool DoAllMethodsExistInType(Type type, IList<string> methodsToRun)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (string methodToRun in methodsToRun)
            {
                bool found = false;
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == methodToRun)
                        found = true;
                }
                if (found == false)
                    return false;
            }
            return true;
        }
/*
        public static IEnumerable<Type> FindBuildClasses(string path)
        {
            Defaults.Logger.WriteDebugMessage("Executing DLL build from " + path);

            Defaults.Logger.Write("INFO", "Using framework " + Defaults.FrameworkVersion);
            Assembly assemblyInstance = Assembly.LoadFile(path);
            Type[] types = assemblyInstance.GetTypes();
            return types.Where(t => t.IsSubclassOf(typeof(BuildFile)));
        }
 */
    }
}
