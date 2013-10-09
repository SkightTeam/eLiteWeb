using System.IO;
using System.Text.RegularExpressions;

namespace FluentFs.Support.FileSet
{
    public interface ISearchPatternParser
    {
        void Parse(string pattern);
        string SearchPattern { get; set; }
        string Folder { get; set; }
        bool Recursive { get; set; }
    }

    internal class SearchPatternParser : ISearchPatternParser
    {
        public SearchPatternParser()
        {
            SearchPattern = "*.*";
        }

        public void Parse(string pattern)
        {
            pattern = pattern.Replace(@"\\", @"\"); //remove double slashes
            //no wildcards so just a folder i.e. c:\temp)
            if (pattern.IndexOf("*") == -1)
            {
                Folder = pattern;
                return;
            }

            // c:\temp\auto*.cs
            var regex = new Regex(@"[a-zA-Z0-9]\*.");
            if (regex.IsMatch(pattern))
            {
                SearchPattern = Path.GetFileName(pattern);
                Folder = pattern.Substring(0, pattern.LastIndexOf("\\")+1);
                if (Folder.IndexOf("\\**\\") >=0)
                {
                    Recursive = true;
                    Folder = Folder.Replace("\\**\\", "\\");
                }
            }
            else
            {
                SearchPattern = pattern.Substring(pattern.IndexOf("*"));
                Folder = pattern.Substring(0, pattern.IndexOf("*"));

                if (SearchPattern.IndexOf("**") >= 0)
                {
                    Recursive = true;
                    SearchPattern = SearchPattern.Substring(SearchPattern.IndexOf("**") + 3);
                }
            }
        }

        public string SearchPattern { get; set; }
        public string Folder { get; set; }
        public bool Recursive { get; set; }
    }
}