using System;
using System.Threading;
using NUnit.Framework;

namespace FluentBuild
{
    ///<summary />
    [TestFixture]
    public class BuildFileTests
    {
        ///<summary />
        [Test]
        public void TestThatQueueGetsProcessed()
        {
            var subject = new BuildFile();
            bool methodCalled = false;

            subject.AddTask(delegate { methodCalled = true; });
            Assert.That(subject.Tasks.Count, Is.EqualTo(1));
            subject.InvokeNextTask();
            Assert.IsTrue(methodCalled);
            Assert.That(subject.Tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public void ClearTasksShouldRemoveAllTasks()
        {
            var subject = new BuildFile();
            subject.AddTask(delegate { });
            Assert.That(subject.TaskCount, Is.EqualTo(1));
            subject.ClearTasks();
            Assert.That(subject.TaskCount, Is.EqualTo(0));
        }

        [Test]
        public void ShouldHaveProperCount()
        {
            var subject = new BuildFile();
            bool methodCalled = false;

            subject.AddTask(delegate { methodCalled = true; });
            Assert.That(subject.TaskCount, Is.EqualTo(1));
        }

        [Test]
        public void AddNamedTask()
        {
            var subject = new BuildFile();
            bool methodCalled = false;

            subject.AddTask("Test", delegate { methodCalled = true; });
            var task = subject.Tasks.Dequeue();
            Assert.That(task.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void TaskThatThrowsExceptionShouldSignalErrorState()
        {
            Assert.That(!BuildFile.IsInErrorState);
            var subject = new BuildFile();
            subject.AddTask("Test", delegate { throw new ApplicationException("testing execption handling");});
            subject.InvokeNextTask();
            Assert.That(BuildFile.IsInErrorState);
        }

        [TearDown]
        public void TearDown()
        {
            //ensure errors are cleared
            BuildFile.IsInErrorState = false;
        }

        [Test]
        public void TaskThatThrowsExceptionShouldPreventOtherTasksFromRunning()
        {
            Assert.That(!BuildFile.IsInErrorState);
            var DidSecondTaskRun = false;
            var subject = new BuildFile();
            subject.AddTask("Test", delegate { throw new ApplicationException("testing execption handling"); });
            subject.AddTask("Test", delegate { DidSecondTaskRun = true; });
            subject.InvokeNextTask();
            Assert.That(!DidSecondTaskRun);
        }
    }
}