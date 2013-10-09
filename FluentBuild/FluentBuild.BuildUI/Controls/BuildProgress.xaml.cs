using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

using FluentBuild.MessageLoggers;

namespace FluentBuild.BuildUI
{
    /// <summary>
    /// Interaction logic for BuildProgress.xaml
    /// </summary>
    public partial class BuildProgress : UserControl, IMessageLogger, INotifyPropertyChanged
    {
        public VerbosityLevel Verbosity
        {
            get { throw new NotImplementedException("implemented by proxy"); }
            set { throw new NotImplementedException("implemented by proxy"); }
        }


        //public AutoResetEvent Lock;
        //public object Lock;

        public BuildProgress()
        {
            InitializeComponent();
            BuildNotices = new ObservableCollection<BuildData>();
            DataContext = this;
            //Lock = new AutoResetEvent(false);
            MessageActions=new Queue<Action>();
         //   Lock = new object();
        }

        public ObservableCollection<BuildData> BuildNotices { get; set; }

        #region IMessageLogger Members

        private Queue<Action> MessageActions { get; set; }
 
        public void WriteHeader(string header)
        {
            Dispatcher.BeginInvoke(new Action(delegate
                                           {
                                               if (BuildNotices.Count > 0)
                                               {
                                                   BuildNotices.Last().Completed = true;
                                               }
                                               BuildNotices.Add(new BuildData(header));
                                           }));
                
        }

        private void ProcessMessageQueue()
        {
            Dispatcher.BeginInvoke(new Action(delegate
                                                  {
                                                      while (MessageActions.Count == 0)
                                                      {
                                                          var action = MessageActions.Dequeue();
                                                          action.Invoke();
                                                      }
                                                  }));
            
        }

        private void RunOnDispatcherThread(Action x)
        {

     if (BuildNotices.Count == 0)
                {
                    return;
                }

                //MessageActions.Enqueue(x);
                //create a lock so that events happen in the proper order
                //Lock.Reset();
                var dispatcherOperation = Dispatcher.BeginInvoke(x);
                //dispatcherOperation.Completed += delegate { Lock.Set(); };
           // }
           // ProcessMessageQueue();
        }

        public void WriteDebugMessage(string message)
        {
            RunOnDispatcherThread(new Action(() => BuildNotices.Last().AddItem(message, TaskState.Normal)));
        }


        public void Write(string type, string message, params string[] items)
        {
            var data = string.Format(message, items);
            RunOnDispatcherThread(new Action(() => BuildNotices.Last().AddItem(data, TaskState.Normal)));
        }

        public void Write(string type, string message, string statusDescription)
        {
            RunOnDispatcherThread(new Action(() => BuildNotices.Last().AddItem(message, TaskState.Normal)));
        }

        public void WriteError(string type, string message)
        {
            RunOnDispatcherThread(new Action(delegate { BuildNotices.Last().AddItem(message, TaskState.Error);
                                                           BuildNotices.Last().State = TaskState.Error;
            }));
        }

        public void WriteWarning(string type, string message)
        {
            RunOnDispatcherThread(new Action(delegate { BuildNotices.Last().AddItem(message, TaskState.Warning);
                                                           BuildNotices.Last().State = TaskState.Warning;
            }));
        }

        public IDisposable ShowDebugMessages
        {
            get { throw new NotImplementedException("This should only be handled from a proxy wrapping this logger"); }
        }

        public ITestSuiteMessageLogger WriteTestSuiteStarted(string name)
        {
            RunOnDispatcherThread(new Action(() => BuildNotices.Last().AddItem(name, TaskState.Normal)));
            return new UnitTestSuiteHandler(Dispatcher, BuildNotices.Last());
        }

        #endregion

        public void Reset()
        {
            this.BuildNotices.Clear();
            InvokePropertyChanged("BuildNotices");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }
}