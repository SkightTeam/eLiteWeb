using System;
using System.IO;
using System.Text;
using FluentBuild.UtilitySupport;

namespace FluentBuild.BuildExe
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "/?" || args[0] == "/h")
            {
                Console.WriteLine("Usage: fb.exe BuildFileOrSource [-c:BuildClass] [-m:Method] [-p:property=value] [-p:property] [-v:Verbosity]");
                Console.WriteLine();
                Console.WriteLine("BuildFileOrSource: the dll that contains the precompiled build file OR the path to the source folder than contains build files (fb.exe will compile the build file for you)");
                Console.WriteLine("c: The class to run. If none is specified then \"Default\" is assumed");
                Console.WriteLine("p: properties to pass to the build script. These can be accessed via Properties.CommandLine in your build script. ");
                Console.WriteLine("v: verbosity of output. Can be None, TaskNamesOnly, TaskDetails, Full");
                Console.WriteLine("m: Method to run. Allows a user to execute specific methods in the build. If specified only the method will run. Multiple specifications are allowed.");
                Environment.Exit(1);
            }


            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            
            Defaults.Logger.Verbosity = VerbosityLevel.TaskDetails;

            //creates a new parser and parses args
            var parser = new CommandLineParser(args);

            var argString = new StringBuilder();
            foreach (string s in args)
            {
                argString.Append(" /" + s);
            }

            Defaults.Logger.Write("INIT", "running fb.exe " + argString);

            string pathToAssembly;
            if (parser.SourceBuild)
            {
                Defaults.Logger.Write("INIT", "building task from sources");
                if (!Directory.Exists(parser.PathToBuildSources))
                {
                    Defaults.Logger.WriteError("ERROR", "Could not find sources at: " + parser.PathToBuildSources);
                    Environment.Exit(1);
                }
                pathToAssembly = CompilerService.BuildAssemblyFromSources(parser.PathToBuildSources, Environment.CurrentDirectory);
            }
            else
            {
                pathToAssembly = parser.PathToBuildDll;
            }

            if (!File.Exists(pathToAssembly))
            {
                Console.WriteLine("Could not find compiled build script at: " + parser.PathToBuildSources);
                Environment.Exit(1);
            }


            string output = CompilerService.ExecuteBuildTask(pathToAssembly, parser.ClassToRun, parser.MethodsToRun);
            if (output != string.Empty)
            {
                Console.WriteLine(output);
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.ExitCode = 1;
            var exceptionObject = e.ExceptionObject as Exception;
            Defaults.Logger.WriteError("ERROR", "An unexpected error has occurred. Details:" + exceptionObject);
            Environment.Exit(1);
        }
    }
}