using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FluentBuild.BuildUI
{
    public class Message
    {
        public string Data { get; set; }
        public TaskState State { get; set; }

        public Message(string data, TaskState state)
        {
            Data = data;
            State = state;
        }
    }

    public class BuildData : INotifyPropertyChanged
    {
        public BuildData(string header)
        {
            Header = header;
            Info = new ObservableCollection<Message>();
            _state = TaskState.Normal;
            ItemCount = new ObservableCollection<string>();
        }

        public void AddItem(string message, TaskState state)
        {
            Info.Add(new Message(message, state));
            //reset the counter so the ticks don't get too long
            if (ItemCount.Count > 20)
                ItemCount.Clear();
            ItemCount.Add(message);
            InvokePropertyChanged("ItemCount");
        }

        public string Header { get; set; }
        public ObservableCollection<Message> Info { get; set; }

        //used for displaying ticks
        public ObservableCollection<string> ItemCount {get; set; }


        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; 
            InvokePropertyChanged("Completed");
            }
        }

        private TaskState _state;
        public TaskState State
        {
            get { return _state; }
            set
            {
                if (value > _state)
                {
                    _state = value;
                    InvokePropertyChanged("State");
                }
            }
        }

        public MessageType MessageType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(string e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(e));
        }
    }

    public enum TaskState
    {
        Normal=1,
        Warning=2,
        Error=3
    }

    public enum MessageType
    {
        Normal,
        Test
    }
}