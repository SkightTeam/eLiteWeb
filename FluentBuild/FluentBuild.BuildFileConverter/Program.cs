using System;

namespace FluentBuild.BuildFileConverter
{
    internal class Program
    {
        /// <summary>
        /// Not at all meant for the real world. Just here to help get started by converting others scripts
        /// </summary>
        /// <param name="args"></param>
        

        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Attempts to convert nAnt build file to a preliminary fluent build file.");
                Console.WriteLine();
                Console.WriteLine("Usage: BuildFileConverter.exe pathToNantFile pathToOutputFolder");
            }
            else
            {
                var convertFile = new ConvertFile(args[0], args[1]);
                convertFile.Generate();
            }
        }
    }
}