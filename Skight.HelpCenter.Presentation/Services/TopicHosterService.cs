using System.Collections.Generic;
using Skight.eLiteWeb.Domain.Containers;
using Skight.HelpCenter.Domain;

namespace Skight.HelpCenter.Presentation.Services
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class TopicHosterService
    {
        public void answer(Sentence sentence, List<Keyword> tags)
        {
            var answer = new Answer();
            answer.Sentence = sentence;
            //answer.Tags.Add(tags);
        }
    }
}