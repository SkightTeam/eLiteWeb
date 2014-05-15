using NUnit.Framework;
using Skight.eLiteWeb.Sample.Domain.Specs.My.src;

namespace Skight.eLiteWeb.Sample.Domain.Specs.My.specs
{
    [TestFixture]
    public class FizzBuzzWhizzSpecs {
        [Test]
        public void Run() {
            new FizzBuzzWhizz().run();
        }
    }
}