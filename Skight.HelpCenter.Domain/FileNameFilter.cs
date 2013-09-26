using System.Collections.Generic;
using System.IO;

namespace Skight.HelpCenter.Domain
{
    public class FileNameFilter
    {
        public IEnumerable<FileInfo> filter(IEnumerable<FileInfo> list )
        {
            var result = new List<FileInfo>();
            foreach (var file_info in list)
            {
                if (file_info.Extension == ".pdf") 
                    result.Add(file_info);
            }
            return result;
        }
    }
}