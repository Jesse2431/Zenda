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
using System.Drawing; // NOTE: Splash Screen requires it
using System.Reflection; // NOTE: Splash Screen requires it
using System.Resources; // NOTE: Splash Screen requires it
using DSCript; // loads Fireboyd78's module

namespace Zenda
{
	class SplashScreen : Form 
	{
		const int Interval = 3000; // 3 seconds
		const string TextA = "Loading...";
		const string TextB = "Made by Jesse and BuilderDemo7";

		Timer splashTimer;

        private void InitializeComponent()
        {
        	FormBorderStyle = FormBorderStyle.None;
        	ClientSize = new System.Drawing.Size(600,300);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.SuspendLayout();

        	// Splash screen timer
        	splashTimer = new Timer(); // creates the timer
        	splashTimer.Interval = Interval; // sets the interval (in miliseconds)
        	splashTimer.Tick += delegate { this.Close(); }; // make the window close when a second is elapsed
        	splashTimer.Start(); // starts the timer
        }
		public SplashScreen()
        {
			InitializeComponent(); // Initalize the main (timer, client size and border)

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Define labels
            System.Windows.Forms.Label labA = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labB = new System.Windows.Forms.Label();
            // Define labels text
            labA.Text = TextA;
            labB.Text = TextB;
            // Define labels position and size
            labA.Location = new System.Drawing.Point(11,280);
            labB.Location = new System.Drawing.Point(461,280);
            labA.Size = new System.Drawing.Size(120,20);
            labB.Size = new System.Drawing.Size(120,20);
            // Define labels font
            labA.Font = new Font("Calibri", 10);
            labB.Font = new Font("Calibri", 10);
            // Define labels colors
            labA.BackColor = System.Drawing.Color.FromArgb(0, System.Drawing.Color.White);
            labB.BackColor = System.Drawing.Color.FromArgb(0, System.Drawing.Color.White);

            /*
            labA.BackColor = new Color.FromRgb(0,0,0);
            labB.BackColor = new Color.FromRgb(0,0,0);
            labA.ForeColor = new Color.FromRgb(255,255,255);
            labB.ForeColor = new Color.FromRgb(255,255,255);
            */


            
			// Now the splash screen image
            PictureBox spsimg = new PictureBox();
            spsimg.Image = System.Drawing.Image.FromFile("Images/splashscreen.png");
            spsimg.Visible = true;
            spsimg.Location = new System.Drawing.Point(0,0);
            spsimg.SizeMode = PictureBoxSizeMode.StretchImage;

            // Set width and height
            spsimg.Width = ClientSize.Width;
            spsimg.Height = ClientSize.Height;
            this.CenterToScreen(); // center the splash screen

            // Set label parent
            labA.Parent = spsimg;
            labB.Parent = spsimg;

            // Add the labels and image
            this.Controls.Add(labA);
            this.Controls.Add(spsimg);
            this.Controls.Add(labB);
        }
	}

    public partial class MainWindow : Window
    {
    	public const string programTitle = "Zenda"; // used when opening/closing a file
    	public const string programTitleNF = "Zenda - No file opened"; // when a file is closed this will be used
    	public FileStream file;

        public MainWindow()
        {
        	SplashScreen spscreen = new SplashScreen();
        	System.Windows.Forms.Application.Run(spscreen);
            InitializeComponent();
        }

        private void MenuFileExitClicked(object sender, RoutedEventArgs e)
        {
            var Result = System.Windows.MessageBox.Show(
                "Are you sure you want to exit?\n" +     // Text A to display in the MessageBox
                "Any unsaved changes will be lost!",     // Text B to display in the MessageBox
                "Exit Zenda",                            // Title of the MessageBox
                MessageBoxButton.YesNo,                  // What type of choices the user has
                MessageBoxImage.Warning                  // Pictogram icon to show
            );                                               

            if (Result == MessageBoxResult.Yes)
            {
            	if (file!=null) { file.Close(); }
                Environment.Exit(0); // Exits the program
            }
            else if (Result == MessageBoxResult.No)
            {
                // Do nothing because the user regretted his grave mistake of exiting Zenda. I won't forgive that next time
            }
        }
        
        // NOTE: called if a file was open in the menu
        // NOTE: string 'game' is which game on what game we will open the file
        public void onOpenFile(string game="Driv3r")
        {
            this.Title = String.Format("{0} - {1}",programTitle,file.Name);
        	// TODO: Decide how it will open the file
        }
        
        // NOTE: called if a file was closed in the menu 
        public void onCloseFile()
        {
           // TODO: Decide how it will close the file
        }
        
        // little help
        private void MenuFileOpenClicked(object sender, RoutedEventArgs e)
        {
        	// TODO: Use between DSCript.Driv3r.GetPath() and DSCript.DriverPL.GetPath() as initial directory if needed
            // NOTE FROM JESSE: About above TODO, we want to do that? I think it's okay with user manually searching it
            //                  I mean we could implement it but I dunno man
            OpenFileDialog fileDialog = new OpenFileDialog(); // creates the open file dialog
        	fileDialog.InitialDirectory = Directory.GetCurrentDirectory().ToString(); // set directory to current directory

        	// TODO: set formats Zenda can open
        	fileDialog.Filter = "Category 1 (*.bin)|*.bin|Category 2 (*.sp)|*.sp|All files (*.*)|*.*";
        	fileDialog.Title = "Choose a file to open";
        	fileDialog.RestoreDirectory = true;
        	
        	// show open file dialog & check if user has choosed a file
        	if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
        		try
                {
        		    file = new FileStream(fileDialog.FileName,FileMode.Open); // TODO: Decide what FileMode will be used
        		    MenuFileClose.IsEnabled = true;
        		    onOpenFile(); // call onOpenFile to indicate that the file was open
        		}
        		catch (Exception ex)
                {
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
                "            Source: https://github.com/Jesse2431/Zenda", 
                "Credits", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information
            );
        }
        
		void MenuFileClose_Click(object sender, RoutedEventArgs e)
		{
            var Result = System.Windows.MessageBox.Show(
                "Are you sure you want to close this file?\n" +     // Text A to display in the MessageBox
                "Any unsaved changes will be lost!",                // Text B to display in the MessageBox
                "Close file",                                       // Title of the MessageBox
                MessageBoxButton.YesNo,                             // What type of choices the user has
                MessageBoxImage.Warning                             // Pictogram icon to show
            );

            if (Result == MessageBoxResult.Yes)
            {
                // If a file is open, close it
                if (file != null)
                {
                    Title = programTitle;
                    file.Close();
                    MenuFileClose.IsEnabled = false;
                    onCloseFile();
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                // Do nothing because user decided to not close current file
            }
		}
    }
}