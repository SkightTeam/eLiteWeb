using System;
using System.Linq;
using System.Windows.Threading;
using FluentBuild.MessageLoggers;

namespace FluentBuild.BuildUI
{


    public class UnitTestSuiteHandler : ITestSuiteMessageLogger
    {
        private readonly Dispatcher _dispatcher;
        private readonly BuildData _buildData;


        public UnitTestSuiteHandler(Dispatcher dispatcher, BuildData buildData)
        {
            _dispatcher = dispatcher;
            _buildData = buildData;
        }

        public ITestSuiteMessageLogger WriteTestSuiteStared(string name)
        {
            return new UnitTestSuiteHandler(_dispatcher, _buildData);
        }

        public void WriteTestSuiteFinished()
        {
            //_dispatcher.BeginInvoke(new Action(() => _buildData.AddItem("Done", TaskState.Normal)));
        }

        public ITestLogger WriteTestStarted(string testName)
        {
            _dispatcher.BeginInvoke(new Action(() => _buildData.AddItem(testName, TaskState.Normal)));
            return new TestLogger(_dispatcher, _buildData);
        }
    }

    public class TestLogger : ITestLogger
    {
        private readonly Dispatcher _dispatcher;
        private readonly BuildData _buildData;

        public TestLogger(Dispatcher dispatcher, BuildData buildData)
        {
            _dispatcher = dispatcher;
            _buildData = buildData;
        }

        public void WriteTestPassed(TimeSpan duration)
        {
            _dispatcher.BeginInvoke(new Action(() => _buildData.Info.Last().Data += "......Passed"));
        }

        public void WriteTestIgnored(string message)
        {
            _dispatcher.BeginInvoke(new Action(delegate
                                                   {
                                                       var last = _buildData.Info.Last();
                                                       last.Data += ".....Ignored";
                                                       last.State = TaskState.Warning;
                                                   }));
        }

        public void WriteTestFailed(string message, string details)
        {
            _dispatcher.BeginInvoke(new Action(delegate
            {
                var last = _buildData.Info.Last();
                //TODO: have this data split out and use a different presenter for it
                last.Data += String.Format(".....Failed \n{0}\n{1}", message,details);
                last.State = TaskState.Error;
            }));
        }
    }
}