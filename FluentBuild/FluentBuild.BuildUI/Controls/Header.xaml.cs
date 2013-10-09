using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using FluentBuild.UtilitySupport;
using UserControl = System.Windows.Controls.UserControl;

namespace FluentBuild.BuildUI.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl, INotifyPropertyChanged
    {
        private string _buildAssembly;
        private string _classToRun;

        public Header()
        {
            InitializeComponent();
            Path.Text = SettingHelper.LastPath;
            WorkingDirectory.Text = SettingHelper.LastWorkingDirectory;
            Compile(Path.Text);
            Defaults.Logger.Verbosity = VerbosityLevel.Full;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public event EventHandler Reset;

        public void InvokeReset()
        {
            EventHandler handler = Reset;
            if (handler != null) handler(this, new EventArgs());
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                
                dialog.SelectedPath = Path.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Path.Text = dialog.SelectedPath;
                    WorkingDirectory.Text = dialog.SelectedPath;
                    InvokePropertyChanged("Path");
                    InvokePropertyChanged("WorkingDirectory");

                    Compile(dialog.SelectedPath);
                }
            }
        }


        private void Compile(string selectedPath)
        {
            if (string.IsNullOrEmpty(selectedPath))
                return;

            SettingHelper.LastPath = Path.Text;
            SettingHelper.LastWorkingDirectory = WorkingDirectory.Text;

            InvokeReset();

            Defaults.Logger.WriteHeader("Compile Build File");

            string workingDir = WorkingDirectory.Text;
            //run on another thread to not block the message thread
            var d = new Action(delegate
                                   {
                                       if (!Directory.Exists(selectedPath))
                                       {
                                           Defaults.Logger.WriteError("Folder Not Found", "Could not find the build folder at " + selectedPath);
                                           return;
                                       }

                                       try
                                       {
                                           _buildAssembly = CompilerService.BuildAssemblyFromSources(selectedPath, workingDir);
                                           Dispatcher.BeginInvoke(new Action(delegate
                                                                                 {
                                                                                     Targets.ItemsSource = CompilerService.FindBuildClasses(_buildAssembly);
                                                                                     Targets.SelectedIndex = 0;
                                                                                     InvokePropertyChanged("Targets");
                                                                                 }));
                                       }
                                       catch (Exception e)
                                       {
                                           Defaults.Logger.WriteError("Compile Build File", e.ToString());
                                       }

                                       Defaults.Logger.WriteHeader("Done");
                                   });

            d.BeginInvoke(null, null);
        }

        public void InvokePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        private void RunCurrentBuild(object sender, RoutedEventArgs e)
        {
            InvokeReset();

            Defaults.Logger.WriteHeader("Start");


            Directory.SetCurrentDirectory(WorkingDirectory.Text);
            Environment.CurrentDirectory = WorkingDirectory.Text;
            _classToRun = Targets.SelectedItem.ToString();
            var th = new Thread(StartCompile);
            th.Start();
        }

        private void StartCompile()
        {
            string output = CompilerService.ExecuteBuildTask(_buildAssembly, _classToRun, null);
            if (output != string.Empty)
                Defaults.Logger.WriteError("", output);
            //            var runner = new Runner(Targets.SelectedItem.ToString(), _buildAssembly);
            //          runner.Run();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            Compile(Path.Text);
        }

        private void WorkingDirectoryBrowseClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = WorkingDirectory.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WorkingDirectory.Text = dialog.SelectedPath;
                    InvokePropertyChanged("WorkingDirectory");
                }
            }
        }
    }
}