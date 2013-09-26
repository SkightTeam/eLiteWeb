using System;
using System.Collections.Generic;
using System.IO;

namespace Skight.HelpCenter.Domain
{
    public class FileProcessor
    {
        private const string Folder = @"C:\Temp";
        public void process()
        {
            var files= Directory.GetFiles(Folder);
            var process_list =
                new FileReadableFilter().filter(
                    new FileExtensionFilter().filter(
                    new FileNameFilter().filter(
                        new FileInfoConverter().convert(files))));
            foreach (var file_info in process_list)
            {
                Console.WriteLine("Processing {0}", file_info.FullName);
            }
        }
    }
}