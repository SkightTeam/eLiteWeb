using System;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;

namespace Skight.eLiteWeb.Presentation.Web.ViewEngins.TemplateProvider
{
    public class CachedFileCompilerSpecs
    {
         
    }
    public class When_call_compile_first_time:Specification<CachedFileCompiler>
    {
       
        Because of = () => {result= subject.compile_template("path"); };

      
        private It should_call_internal_compiler =
            () => DependencyOf<FileCompilerImpl>().AssertWasCalled(
               x => x.compile_template("path"), c => c.Repeat.Once());

        private static Type result;
    }

    public class When_call_compile_second_time : Specification<CachedFileCompiler>
    {
        private Establish context =
            () => subject.compile_template("path");
        Because of = () =>result= subject.compile_template("path");


        private It should_call_internal_compiler =
            () => DependencyOf<FileCompilerImpl>().AssertWasCalled(
                x => x.compile_template("path"),c=>c.Repeat.Once());

        private static Type result;
    }

    public class When_call_compile_generic_template_first_time : Specification<CachedFileCompiler> {

        Because of = () => { result = subject.compile_template<string>("path"); };


        private It should_call_internal_compiler =
            () => DependencyOf<FileCompilerImpl>().AssertWasCalled(
               x => x.compile_template<string>("path"), c => c.Repeat.Once());

        private static Type result;
    }

    public class When_call_compile_generic_template_second_time : Specification<CachedFileCompiler> {
        private Establish context =
            () => subject.compile_template<string>("path");
        Because of = () => result = subject.compile_template<string>("path");


        private It should_call_internal_compiler =
            () => DependencyOf<FileCompilerImpl>().AssertWasCalled(
                x => x.compile_template<string>("path"), c => c.Repeat.Once());

        private static Type result;
    }

}