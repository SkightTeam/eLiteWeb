using System.Collections.Generic;
using System.Text;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.HelpCenter.Domain
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Decomposor
    {
        private int max_keywords_length;
        private int min_keywords_length;

        public Decomposor(int minKeywordsLength, int maxKeywordsLength)
        {
            min_keywords_length = minKeywordsLength;
            max_keywords_length = maxKeywordsLength;
        }
        
        [Inject]
        public Decomposor():this(2,4)
        {
        }

        public IEnumerable<Keyword> decompose(Sentence sentence)
        {
            var chars =new List<char> (decompose((string)sentence));
            for (int i = 0; i < chars.Count; i++)
            {
                var builder = new StringBuilder();
                for (int j = 1; j <= max_keywords_length; j++)
                {
                    var position = i + j - 1;
                    if (position >= chars.Count) break;
                    builder.Append(chars[position]);
                    if (j >= min_keywords_length)
                    {
                        yield return builder.ToString();
                    }
                }
            }

        }

        IEnumerable<char> decompose(string content)
        {
            foreach (char item in content) {
                yield return item;
            }
        }
    }
}