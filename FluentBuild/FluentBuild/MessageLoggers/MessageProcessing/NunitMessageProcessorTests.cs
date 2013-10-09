using System;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.MessageLoggers.MessageProcessing
{
    [TestFixture]
    public class NunitMessageProcessorTests
    {
        #region Setup/Teardown

        /* SAMPLE OUTPUT
         * ProcessModel: Default    DomainUsage: Single
Execution Runtime: Default
  [ERROR] System.ApplicationException: testing execption handling
          at FluentBuild.BuildFileTests.<TaskThatThrowsExceptionShouldPreven
          tOtherTasksFromRunning>b__d() in C:\Projects\fluent-build\src\FluentB
          uild\BuildFileTests.cs:line 79
          at FluentBuild.BuildFile.InvokeNextTask() in C:\Projects\fluent-bu
          ild\src\FluentBuild\BuildFile.cs:line 50
  [ERROR] System.ApplicationException: testing execption handling
          at FluentBuild.BuildFileTests.<TaskThatThrowsExceptionShouldSignal
          ErrorState>b__b() in C:\Projects\fluent-build\src\FluentBuild\BuildFi
          leTests.cs:line 61
          at FluentBuild.BuildFile.InvokeNextTask() in C:\Projects\fluent-bu
          ild\src\FluentBuild\BuildFile.cs:line 50
  [vbc] Compiling 0 files to ''

<?xml version="1.0" encoding="utf-8" standalone="no"?>
<!--This file represents the results of running a test suite-->
<test-results name="src\FluentBuild\bin\Debug\FluentBuild.dll" total="220" errors="1" failures="0" not-run="0" inconclusive="0" ignored="0" skipped="0" invalid="0" date="2012-06-18" time="20:56:59">
  <environment nunit-version="2.5.7.10213" clr-version="2.0.50727.5456" os-version="Microsoft Windows NT 6.1.7601 Service Pack 1" platform="Win32NT" cwd="C:\Projects\fluent-build" machine-name="KUDOS-LAPTOP" user="Kudos" user-domain="Kudos-Laptop" />
  <culture-info current-culture="en-US" current-uiculture="en-US" />
  <test-suite type="Assembly" name="src\FluentBuild\bin\Debug\FluentBuild.dll" executed="True" result="Failure" success="False" time="2.145" asserts="0">
    <results>
      <test-suite type="Namespace" name="FluentBuild" executed="True" result="Failure" success="False" time="2.134" asserts="0">
        <results>
          <test-suite type="TestFixture" name="ActionExecutorTests" executed="True" result="Success" success="True" time="0.060" asserts="0">
            <results>
              <test-case name="FluentBuild.ActionExecutorTests.ShouldApplyValuesAndExecute" executed="True" result="Success" success="True" time="0.029" asserts="1" />
              <test-case name="FluentBuild.ActionExecutorTests.ShouldApplyValuesToItemWithConstructorAndExecute" executed="True" result="Success" success="True" time="0.004" asserts="2" />
              <test-case name="FluentBuild.ActionExecutorTests.ShouldWorkOnProperty" executed="True" result="Success" success="True" time="0.002" asserts="0" />
            </results>
          </test-suite>
        */

        public string GenerateTest(string testStatus)
        {
            var xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" ?>");
            xml.Append("<test-results>");
            xml.Append("    <environment />");
            xml.Append("    <culture-info />");
            xml.Append("    <test-suite name=\"" + _rootSuiteName + "\" success=\"True\" time=\"0.016\" asserts=\"0\">");
            xml.Append("        <results>");
            xml.Append("            <test-case name=\"" + _passingTestName + "\" description=\"Mock Test #1\" executed=\"True\" result=\"" + testStatus + "\" time=\"5.160\" asserts=\"0\">");
            xml.Append("              <failure>");
            xml.Append("                  <message><![CDATA[Intentional failure]]></message>");
            xml.Append("                  <stack-trace><![CDATA[at NUnit.Tests.Assemblies.MockTestFixture.FailingTest () [0x00000] in /home/charlie/Dev/NUnit/nunit-2.5/work/src/tests/mock-assembly/MockAssembly.cs:121]]></stack-trace>");
            xml.Append("              </failure>");
            xml.Append("           <reason>");
            xml.Append("              <message><![CDATA[ignoring this test method for now]]></message>");
            xml.Append("           </reason>");
            xml.Append("            </test-case>");
            xml.Append("        </results>");
            xml.Append("    </test-suite>");
            xml.Append("</test-results>");
            return xml.ToString();
        }

        [SetUp]
        public void SetUp()
        {
            _rootSuiteName = "C:\\Program Files\\NUnit 2.2.9\\bin\\mock-assembly.dll";
            _passingTestName = "NUnit.Tests.Assemblies.MockTestFixture.MockTest1";
         

            _logger = MockRepository.GenerateMock<IMessageLogger>();
            _suiteLogger = MockRepository.GenerateMock<ITestSuiteMessageLogger>();
            _testLogger = MockRepository.GenerateMock<ITestLogger>();
            _logger.Stub(x => x.WriteTestSuiteStarted(Arg<string>.Is.Anything)).Return(_suiteLogger);
            _suiteLogger.Stub(x => x.WriteTestStarted(Arg<string>.Is.Anything)).Return(_testLogger);
            _subject = new NunitMessageProcessor(_logger);
        }

        #endregion

        //private StringBuilder _xml;
        private string _rootSuiteName;
        private IMessageLogger _logger;
        private ITestSuiteMessageLogger _suiteLogger;
        private NunitMessageProcessor _subject;
        private string _passingTestName;
        private ITestLogger _testLogger;

        [Test]
        public void DisplayShouldLogTestSuiteStartedAndFinished()
        {
            _subject.Display("", "I TOTALLY FAILED", null, 0);
            _logger.AssertWasCalled(x => x.WriteError(Arg<String>.Is.Anything, Arg<String>.Is.Anything));
            //_suiteLogger.AssertWasCalled(x => x.WriteTestSuiteFinished());
        }


        [Test]
        public void DisplayShouldHandleMissingXml()
        {
            _subject.Display("", GenerateTest("Success"), null, 0);
            _logger.AssertWasCalled(x => x.WriteTestSuiteStarted(_rootSuiteName));
            _suiteLogger.AssertWasCalled(x => x.WriteTestSuiteFinished());
        }

        [Test]
        public void ParseTime_ShouldParseTimeFormat()
        {
            TimeSpan timeSpan = _subject.ParseTime("1.345");
            Assert.That(timeSpan.Seconds, Is.EqualTo(1));
            Assert.That(timeSpan.Milliseconds, Is.EqualTo(345));
        }

        [Test]
        public void ProcessTestSuite()
        {
            XDocument xmlDoc = XDocument.Parse(GenerateTest("Success"));
            _subject.ProcessTestSuite(xmlDoc.Root.Element("test-suite"));
            _suiteLogger.AssertWasCalled(x => x.WriteTestStarted(_passingTestName));
            _testLogger.AssertWasCalled(x => x.WriteTestPassed(Arg<TimeSpan>.Is.Anything));
        }

        [Test]
        public void ShouldHandleFailedTest()
        {
            XDocument xmlDoc = XDocument.Parse(GenerateTest("Failure"));
            _subject.ProcessTestSuite(xmlDoc.Root.Element("test-suite"));
            _suiteLogger.AssertWasCalled(x => x.WriteTestStarted(_passingTestName));
            _testLogger.AssertWasCalled(x => x.WriteTestFailed(Arg<String>.Is.Anything, Arg<String>.Is.Anything));
        }

        [Test]
        public void ShouldHandleIgnoredTest()
        {
            XDocument xmlDoc = XDocument.Parse(GenerateTest("Ignored"));
            _subject.ProcessTestSuite(xmlDoc.Root.Element("test-suite"));
            _suiteLogger.AssertWasCalled(x => x.WriteTestStarted(_passingTestName));
            _testLogger.AssertWasCalled(x => x.WriteTestIgnored(Arg<String>.Is.Anything));
        }

    }
}