using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FluentBuild.BuildUI
{
    public class MessageStateToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new ColorConverter();
          

            switch ((TaskState)value)
            {
                case TaskState.Normal:
                    //#13aa13
                    return new SolidColorBrush((Color)converter.ConvertFrom("#00CC00"));
                case TaskState.Warning:
                    //#b8a400
                    return new SolidColorBrush((Color)converter.ConvertFrom("#FFFF00")); //"#d9bf1b" 
                case TaskState.Error:
                    //#d21313
                    return new SolidColorBrush((Color)converter.ConvertFrom("#FF0000"));
            }
            throw new NotImplementedException("Could not convert state to color");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}