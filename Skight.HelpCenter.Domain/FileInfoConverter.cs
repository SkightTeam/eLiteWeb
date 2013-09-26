using System.Collections.Generic;
using System.IO;

namespace Skight.HelpCenter.Domain
{
    public class FileInfoConverter
    {
        public IEnumerable<FileInfo> convert(string[] files)
        {
            var list = new List<FileInfo>();
            foreach (var file in files)
            {
                list.Add(new FileInfo(file));
            }
            return list;
        }
    }
}