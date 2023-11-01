using System;
using System.Windows;
using System.Windows.Media;

namespace FramePFX.Themes {
    public static class ThemesController {
        public static ThemeType CurrentTheme { get; set; }

        private static ResourceDictionary ThemeDictionary {
        	get { return Application.Current.Resources.MergedDictionaries[0]; } // fixed error CS1519 in older C# versions
        	set { Application.Current.Resources.MergedDictionaries[0] = value; } // fixed error CS1519 in older C# versions
        }

        private static ResourceDictionary ControlColours {
        	get { return Application.Current.Resources.MergedDictionaries[1]; } // fixed error CS1519 in older C# versions
        	set { Application.Current.Resources.MergedDictionaries[1] = value; } // fixed error CS1519 in older C# versions
        }

        private static ResourceDictionary Controls {
        	get { return Application.Current.Resources.MergedDictionaries[2]; } // fixed error CS1519 in older C# versions
        	set { Application.Current.Resources.MergedDictionaries[2] = value; } // fixed error CS1519 in older C# versions
        }

        public static void SetTheme(ThemeType theme) {
            string themeName = theme.GetName();
            if (string.IsNullOrEmpty(themeName)) {
                return;
            }

            CurrentTheme = theme;
            ThemeDictionary = new ResourceDictionary() { Source = new Uri(String.Format("Themes/ColourDictionaries/{0}.xaml",themeName), UriKind.Relative) };
            ControlColours = new ResourceDictionary() { Source = new Uri("Themes/ControlColours.xaml", UriKind.Relative) };
            Controls = new ResourceDictionary() { Source = new Uri("Themes/Controls.xaml", UriKind.Relative) };
        }

        public static object GetResource(object key) {
            return ThemeDictionary[key];
        }

        public static SolidColorBrush GetBrush(string name) {
        	// fixed error CS1525 in old C# versions
        	SolidColorBrush brush = (SolidColorBrush)GetResource(name);
        	if (brush!=null) {
               return brush;
        	}
        	else {
        	   return new SolidColorBrush(Colors.White);
        	}
        }
    }
}