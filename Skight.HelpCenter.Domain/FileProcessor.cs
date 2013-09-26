using System;
using System.IO;

namespace Skight.HelpCenter.Domain
{
    public class FileProcessor
    {
        private const string Folder = @"C:\Temp";
        public void process()
        {
            var files= Directory.GetFiles(Folder);
            foreach (var file in files)
            {
                FileInfo file_info=new FileInfo(file);
                if (!file_info.Name.StartsWith("Test")) 
                    continue;

                if (file_info.Extension != ".pdf") 
                    continue;

                if (!file_info.IsReadOnly)
                {
                    Console.WriteLine("Processing {0}",file);
                }
            }
        }
    }
}