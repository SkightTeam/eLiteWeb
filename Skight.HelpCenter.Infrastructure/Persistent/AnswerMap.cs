using FluentNHibernate.Mapping;
using Skight.HelpCenter.Domain;
using Skight.eLiteWeb.Infrastructure.Persistent;

namespace Skight.HelpCenter.Infrastructure.Persistent
{
    public class AnswerMap:ClassMap<Answer>
    {
        public AnswerMap()
        {
            Not.LazyLoad();
            Id(x => x.Id);
            Map(x => x.Sentence).CustomType<UserTypeAsString<Sentence>>();
        }
    }
}