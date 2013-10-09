using System.Collections.Generic;
using System.Text;

namespace FluentBuild.Utilities
{
    public class StringUtility
    {
        public static string CreateCommaSeperatedList(IList<string> input)
        {
            var sb = new StringBuilder();
            foreach (var paths in input)
            {
                sb.Append(paths);
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
        
    }
}