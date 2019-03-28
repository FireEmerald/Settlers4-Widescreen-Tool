using System;
using System.Windows;
using System.Windows.Controls;

namespace Settlers.Toolbox.Views.Properties
{
    public class AutoScrollProperty
    {
        // Using a DependencyProperty as the backing store. Enables animation, styling, binding and so on.
        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.RegisterAttached(
            nameof(AutoScrollToEnd),
            typeof(bool),
            typeof(AutoScrollProperty),
            new PropertyMetadata(false, AutoScrollToEndPropertyChanged));

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToEndProperty, value);
        }

        public static bool GetAutoScrollToEnd(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoScrollToEndProperty);
        }

        private static void AutoScrollToEndPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox textBox && e.NewValue is bool autoScroll)
            {
                textBox.TextChanged += (sender, ee) => AutoScrollToEnd(sender, ee, textBox);
            }
            else
            {
                throw new InvalidOperationException("Property can only assigned on TextBox elements and value must be of type boolean!");
            }
        }

        private static void AutoScrollToEnd(object sender, TextChangedEventArgs e, TextBox textBox)
        {
            textBox.ScrollToEnd();
        }
    }
}