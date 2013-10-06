using System.Collections.Generic;
using Skight.eLiteWeb.Domain.Containers;
using Skight.HelpCenter.Domain;
using Skight.eLiteWeb.Domain.Persistent;

namespace Skight.HelpCenter.Presentation.Services
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class TopicHosterService
    {
        private Repository repository;

        public TopicHosterService(Repository repository)
        {
            this.repository = repository;
        }

        public void answer(Sentence sentence, List<Keyword> tags)
        {
            var answer = new Answer();
            answer.Sentence = sentence;
            repository.save(answer);
            //answer.Tags.Add(tags);
        }
    }
}