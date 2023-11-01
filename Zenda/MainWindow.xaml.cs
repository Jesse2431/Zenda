// Main interaction logic for the WPF screen
// Program written by Jesse and BuilderDemo
// 
// Powered by DSCript by Fireboyd78 and WPFDarkTheme by AngryCarrot789

using System;
using System.IO; // NOTE: File class requires it
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
using System.Windows.Forms; // NOTE: open file dialog requires it
using DSCript; // loads Fireboyd78's module

namespace Zenda
{
    public partial class MainWindow : Window
    {
    	public const string programTitle = "Zenda"; // used when opening/closing a file
    	public FileStream file;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuFileExitClicked(object sender, RoutedEventArgs e)
        {
            var Result = System.Windows.MessageBox.Show(
                "Are you sure you want to exit?\n Any unsaved changes will be lost!", // Text to display in the MessageBox
                "Exit Zenda",                                                          // Title of the MessageBox
                MessageBoxButton.YesNo,                                                // What type of choices the user has
                MessageBoxImage.Warning);                                              // Pictogram icon to show

            if (Result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (Result == MessageBoxResult.No)
            {
                // Do nothing because the user regretted his grave mistake of exiting Zenda. I won't forgive that next time
            }
        }
        
        // NOTE: called if a file was open in the menu
        // NOTE: string 'game' is which game on what game we will open the file
        public void onOpenFile(string game="Driv3r") {
            this.Title = String.Format("{0} - {1}",programTitle,file.Name);
        	// TODO: Decide how it will open the file       
        }
        
        // NOTE: called if a file was closed in the menu 
        public void onCloseFile() {
           // TODO: Decide how it will close the file
        }
        
        // little help
        private void MenuFileOpenClicked(object sender, RoutedEventArgs e) {
        	// TODO: Use between DSCript.Driv3r.GetPath() and DSCript.DriverPL.GetPath() as initial directory if needed
            OpenFileDialog fileDialog = new OpenFileDialog(); // creates the open file dialog
        	fileDialog.InitialDirectory = Directory.GetCurrentDirectory().ToString(); // set directory to current directory
        	// TODO: set formats Zenda can open
        	fileDialog.Filter = "Category 1 (*.bin)|*.bin|Category 2 (*.sp)|*.sp|All files (*.*)|*.*";
        	fileDialog.Title = "Choose a file to open";
        	fileDialog.RestoreDirectory = true;
        	
        	// show open file dialog & check if user has choosed a file
        	if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
        		try {
        		    file = new FileStream(fileDialog.FileName,FileMode.Open); // TODO: Decide what FileMode will be used
        		    MenuFileClose.IsEnabled = true;
        		    onOpenFile(); // call onOpenFile to indicate that the file was open
        		}
        		catch (Exception ex) {
        			System.Windows.MessageBox.Show("ERROR:\n\n"+ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        		}
        	}
        }

        private void MenuHelpCreditsClicked(object sender, RoutedEventArgs e)
        {
        	// NOTE: I added 'System.Windows.' reason: System.Windows.Forms
            System.Windows.MessageBox.Show(
                "Zenda, the universal Driver editor \n" +
                "Made by Jesse and BuilderDemo7 \n \n" +
                "Special thanks to: \n" +
                "Fireboyd78, for DSCript: https://github.com/Fireboyd78/driver-tools/ (no set license) \n" +
                "AngryCarrot789, for WPFDarkTheme: https://github.com/AngryCarrot789/WPFDarkTheme (MIT License) \n" +
                "\n" +
                "            Source: https://github.com/Jesse2431/Zenda", "Credits", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
		void MenuFileClose_Click(object sender, RoutedEventArgs e)
		{
			if (file!=null) {
    			this.Title = programTitle;
     			file.Close();
     			MenuFileClose.IsEnabled = false;
     			onCloseFile();
			}
		}

    }
}