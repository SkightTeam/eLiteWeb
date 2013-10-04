using System.Collections.Generic;
using Skight.eLiteWeb.Domain.Containers;
using Skight.HelpCenter.Domain;

namespace Skight.HelpCenter.Presentation.Services
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Service
    {
        private Decomposor decomposor;

        public Service(Decomposor decomposor)
        {
            this.decomposor = decomposor;
        }

        public IEnumerable<Keyword> decompose(Sentence sentence)
        {
            return decomposor.decompose(sentence);
        }
    }
}