using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Settlers.Toolbox.Views.Properties
{
    public class IsExternalLinkProperty
    {
        // Using a DependencyProperty as the backing store. Enables animation, styling, binding and so on.
        public static readonly DependencyProperty IsExternalLink = DependencyProperty.RegisterAttached(
            nameof(IsExternalLink),
            typeof(bool),
            typeof(IsExternalLinkProperty),
            new UIPropertyMetadata(false, IsExternalLinkChanged));

        public static void SetIsExternalLink(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalLink, value);
        }

        public static bool GetIsExternalLink(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsExternalLink);
        }

        private static void IsExternalLinkChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var hyperlink = sender as Hyperlink;

            if (sender is Hyperlink && args.NewValue is bool isExternalLink)
            {
                if (isExternalLink)
                {
                    hyperlink.RequestNavigate += HandleRequestNavigate;
                }
                else
                {
                    hyperlink.RequestNavigate -= HandleRequestNavigate;
                }
            }
            else
            {
                throw new InvalidOperationException("Property can only assigned on Hyperlink elements and value must be of type boolean!");
            }
        }

        private static void HandleRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            }
            catch (Exception)
            {
                // No problem if we can't open the browser...
            }
            finally
            {
                e.Handled = true;
            }
        }
    }
}