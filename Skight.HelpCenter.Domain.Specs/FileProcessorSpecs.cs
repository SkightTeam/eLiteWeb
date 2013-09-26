using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;

namespace Skight.HelpCenter.Domain.Specs
{
    public class FileProcessorSpecs:Specification<FileProcessor>
    {

        private It should_run =
            () => subject.process();

    }
}