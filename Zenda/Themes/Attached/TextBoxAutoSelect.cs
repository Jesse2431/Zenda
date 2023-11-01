using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FramePFX.Themes.Attached {
    public static class TextBoxAutoSelect {
        private static readonly RoutedEventHandler Handler = ControlOnLoaded;

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TextBoxAutoSelect), new PropertyMetadata(false, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        	Control control = new Control();
            if (d.Equals(control)) {
                control.Loaded += Handler;
            }
        }

        private static void ControlOnLoaded(object sender, RoutedEventArgs e) {
        	Control control = new Control();
        	if (sender.Equals(control)) {
                control.Focus();
                // I don't get it
                /*
                TextBoxBase textbox = new TextBoxBase();
                if (control.Equals(textbox)) {
                    textbox.SelectAll();
                }
                */

                control.Loaded -= Handler;
            }
        }

        public static void SetIsEnabled(DependencyObject element, bool value) {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element) {
            return (bool) element.GetValue(IsEnabledProperty);
        }
    }
}