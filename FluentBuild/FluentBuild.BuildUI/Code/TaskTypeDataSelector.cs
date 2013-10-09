using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FluentBuild.BuildUI
{
    public class TaskTypeDataSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is Message)
            {
                var taskitem = item as Message;

                return element.FindResource("messageTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
