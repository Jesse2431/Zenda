using System.Windows;

namespace FramePFX.Themes {
    public partial class Controls {
        private void CloseWindow_Event(object sender, RoutedEventArgs e) {
            if (e.Source != null)
                CloseWind(Window.GetWindow((FrameworkElement) e.Source));
        }

        private void AutoMinimize_Event(object sender, RoutedEventArgs e) {
            if (e.Source != null)
                MaximizeRestore(Window.GetWindow((FrameworkElement) e.Source));
        }

        private void Minimize_Event(object sender, RoutedEventArgs e) {
            if (e.Source != null)
                MinimizeWind(Window.GetWindow((FrameworkElement) e.Source));
        }

		private static void CloseWind(Window window) { window.Close(); } // fixed error CS1519 & CS1520 in older C# versions

        private static void MaximizeRestore(Window window) {
            if (window == null)
                return;

            switch (window.WindowState) {
                case WindowState.Normal:
                    window.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Minimized: // hmm...
                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;
                default: return;
            }
        }

		private static void MinimizeWind(Window window) { window.WindowState = WindowState.Minimized; } // fixed error CS1519 in older C# versions
    }
}