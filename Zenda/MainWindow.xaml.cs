// Main interaction logic for the WPF screen
// Program written by Jesse and BuilderDemo
// 
// Powered by DSCript by Fireboyd78 and WPFDarkTheme by AngryCarrot789

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zenda
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuFileExitClicked(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show(
                "Are you sure you want to exit?\n" +     // Text A to display in the MessageBox
                "Any unsaved changes will be lost!",     // Text B to display in the MessageBox
                "Exit Zenda",                            // Title of the MessageBox
                MessageBoxButton.YesNo,                  // What type of choices the user has
                MessageBoxImage.Warning                  // Pictogram icon to show
            );                                              

            if (Result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (Result == MessageBoxResult.No)
            {
                // Do nothing because the user regretted his grave mistake of exiting Zenda. I won't forgive that next time
            }
        }

        private void MenuHelpCreditsClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Zenda, the universal Driver editor \n" +
                "Made by Jesse and BuilderDemo7 \n \n" +
                "Special thanks to: \n" +
                "Fireboyd78, for DSCript: https://github.com/Fireboyd78/driver-tools/ (no set license) \n" +
                "AngryCarrot789, for WPFDarkTheme: https://github.com/AngryCarrot789/WPFDarkTheme (MIT License) \n" +
                "\n" +
                "            Source: https://github.com/Jesse2431/Zenda", 
                "Credits", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information
            );
        }
    }
}
