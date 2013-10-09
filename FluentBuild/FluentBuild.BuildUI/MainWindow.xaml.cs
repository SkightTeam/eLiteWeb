using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace FluentBuild.BuildUI
{
    public partial class MainWindow : Window
    {
        private string _buildAssembly;

        public MainWindow()
        {
            InitializeComponent();
            Defaults.SetLogger(BuildProgress);
                
        }

        private void ResetOutput(object sender, EventArgs e)
        {
            BuildProgress.Reset();
        }
    }
}