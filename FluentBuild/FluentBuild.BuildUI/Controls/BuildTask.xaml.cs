using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FluentBuild.BuildUI
{
    /// <summary>
    /// Interaction logic for BuildTask.xaml
    /// </summary>
    public partial class BuildTask : UserControl
    {
        public BuildTask()
        {
            InitializeComponent();
        }

        private void ShowHideDetails(object sender, MouseButtonEventArgs e)
        {
//            var data = new TaskMessages();
//            data.DataContext = this.DataContext;
//            data.ShowDialog();

            if (this.Details.Visibility == System.Windows.Visibility.Collapsed)
                this.Details.Visibility = System.Windows.Visibility.Visible;
            else
                this.Details.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
