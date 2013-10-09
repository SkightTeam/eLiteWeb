using FluentBuild.BuildUI.Properties;

namespace FluentBuild.BuildUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /// 
    public class SettingHelper
    {
        public static string LastPath
        {
            get { return Settings.Default.LastPath; }
            set
            {
                Settings.Default.LastPath = value;
                Settings.Default.Save();
            }
        }

        public static string LastWorkingDirectory
        {
            get { return Settings.Default.LastWorkingDirectory; }
            set
            {
                Settings.Default.LastWorkingDirectory = value;
                Settings.Default.Save();
            }
        }
    }
}