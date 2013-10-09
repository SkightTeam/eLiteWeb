using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FluentBuild.BuildUI
{
    public class StateToColorConverter : IValueConverter
    {
        private LinearGradientBrush CreateBrush(string baseColor, string centerColor)
        {
            var converter = new ColorConverter();
            var linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)converter.ConvertFrom(baseColor), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)converter.ConvertFrom(centerColor), 0.5));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)converter.ConvertFrom(baseColor), 1));
            return linearGradientBrush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            switch((TaskState)value)
            {
                case TaskState.Normal:
                    return CreateBrush("#13aa13", "#43d343");
                case TaskState.Warning:
                    return CreateBrush("#d9bf1b", "#f0e460");
                case TaskState.Error:
                    return CreateBrush("#d21313", "#ee5454");
            }
           throw new NotImplementedException("Could not convert state to color");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
