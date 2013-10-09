using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;


namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class AssemblyInfoLanguageTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	    [Test]
        public void EnsureCSharpLanguageBuildsProperly()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            var subject = new AssemblyInfoLanguage(mock);
            Action<IAssemblyInfoDetails> action = x=>x.OutputPath("c:\test.cs");
            subject.CSharp(action);
            mock.AssertWasCalled(x => x.Execute(Arg<Action<AssemblyInfoDetails>>.Is.Equal(action), Arg<CSharpAssemblyInfoBuilder>.Is.Anything));
        }

        ///<summary>
        ///</summary>
        ///<summary />
	    [Test]
        public void EnsureVisualBasicLanguageBuildsProperly()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            var subject = new AssemblyInfoLanguage(mock);
            Action<IAssemblyInfoDetails> action = x => x.OutputPath("c:\test.cs");
            subject.VisualBasic(action);
            mock.AssertWasCalled(x => x.Execute(Arg<Action<AssemblyInfoDetails>>.Is.Equal(action), Arg<VisualBasicAssemblyInfoBuilder>.Is.Anything));
        }
    }
}