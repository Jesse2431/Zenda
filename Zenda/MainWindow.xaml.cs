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
using System.Diagnostics;
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
		System.Drawing.Color BackgroundColor = System.Drawing.Color.FromArgb(0x343353);

		Timer splashTimer;

        private void InitializeComponent()
        {
        	FormBorderStyle = FormBorderStyle.None;
        	ClientSize = new System.Drawing.Size(600,300);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.SuspendLayout();
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location); // Define icon to splash screen (because we don't want that ugly icon)

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
            labA.Location = new System.Drawing.Point(0,276);
            labB.Location = new System.Drawing.Point(400,276);
            //labB.ImageAlign = ContentAlignment.TopRight;
            //labB.TextAlign = ContentAlignment.TopRight;
            labA.Size = new System.Drawing.Size(600,40);
            labB.Size = new System.Drawing.Size(600,40);
            // Define labels font
            labA.Font = new Font("Calibri", 10);
            labB.Font = new Font("Calibri", 10);
            // Define labels colors
            // NOTE From BuilderDemo7: I think no one can really notice with this new code now.
            // Set background color to default Splash Screen background color
            labA.BackColor = System.Drawing.Color.DarkSlateBlue; //System.Drawing.Color.FromArgb(0, System.Drawing.Color.Black);
            labB.BackColor = System.Drawing.Color.DarkSlateBlue; //System.Drawing.Color.FromArgb(0, System.Drawing.Color.Black);
            // and set the text color to white of course
            labA.ForeColor = System.Drawing.Color.White;
            labB.ForeColor = System.Drawing.Color.White;

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
            this.Controls.Add(labB);
            this.Controls.Add(labA);
            this.Controls.Add(spsimg);
        }
	}

    public partial class MainWindow : Window
    {
    	public const string programTitle = "Zenda"; // used when opening/closing a file
    	public const string programTitleNF = "Zenda - No file opened"; // when a file is closed this will be used
    	public FileStream file;
		public enum gameType : int {
			Driv3r = 0,
		    DriverPL = 1,
		    DriverSF = 2,
		    DriverWii = 3
		};
		gameType currentGame;
        // File filters for each game
        // NOTE: FF = File Filter
		public readonly string Driv3rFF = "Driv3r|*.vvs;*.vvv;*.vgt;*.d3c;*.pcs;*.cpr;*.dam;*.map;*.gfx;*.pmu;*.d3s;*.mec;*.bnk;*.bin";
		public readonly string DriverPLFF = "Driver: Parallel Lines|*.sp;*.an4;*.d4c;*.gfx;*.pmu;*.mec;*.bnk;*.bin";
		public readonly string DriverSFFF = "Driver: San Francisco|*.*"; // TODO: Set file filter for Driver: San Francisco
		public readonly string DriverWiiFF = "Driver Wii|*.d4c;*.feu;*.tpl;*.sp;*.gfx;*.txt;*.d4l"; // TODO: Set file filter for DriverWii
    	
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
        // OLD: string 'game' is which game on what game we will open the file
        // NEW: enumeration gameType is which game on what game we will open the file
        public void onOpenFile(gameType game,string path)
        {
        	currentGame = game;
        	string extension = System.IO.Path.GetExtension(path); // the extension of the file so we know who we're messing with
            this.Title = String.Format("{0} - {1}",programTitle,file.Name);
        	// TODO: Decide more files that Zenda can open
            // If the game is Driv3r
            if (game == gameType.Driv3r)
            {
            	// By BuilderDemo7 : Open HUD file (W.i.P)
            		try {
            		  switch (extension)
            		  {
            		    // NOTE: To add more extension handlers for Driv3r add 'case: *.extensionName'...
            		    // ...and add 'break;' in the end of your code
            		    case ".bin": 
            		        Zenda.Driv3r.HUD binHUD = new Zenda.Driv3r.HUD(file);
            		        break;
            		    default:
            		        // Do nothing as this file is not recognized
            		        break;
            		  }
            		}
            	    // If Subject has thrown a exception, let's tell the user
            		catch (Exception ex)
                    {
            		    // Get stack trace for the exception with source file information
                        var st = new StackTrace(ex, true);
                        // Get the top stack frame
                        var frame = st.GetFrame(0);
                        // Get the line number from the stack frame
                        var sourcefile = frame.GetFileName();
                        var line = frame.GetFileLineNumber();            			
            			System.Windows.MessageBox.Show("OPEN ERROR:\n\n"+ex.Message+"\n"+sourcefile+":"+line, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            		}
            	}
            if (game == gameType.DriverPL)
            {
                try
                {
                    switch (extension)
                    {
                        case ".sp":
                            // TODO: Differentiate types of SP files from each other
                            Zenda.DriverPL.VehicleOverride vehicleOverride = new Zenda.DriverPL.VehicleOverride(file);
                            break;
                        default:
                            // Do nothing as this file is not recognized
                            break;
                    }
                }
                // If Subject has thrown a exception, let's tell the user
                catch (Exception ex)
                {
                    // Get stack trace for the exception with source file information
                    var st = new StackTrace(ex, true);
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    // Get the line number from the stack frame
                    var sourcefile = frame.GetFileName();
                    var line = frame.GetFileLineNumber();
                    System.Windows.MessageBox.Show("OPEN ERROR:\n\n" + ex.Message + "\n" + sourcefile + ":" + line, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        // NOTE: called if a file was closed in the menu 
        public void onCloseFile()
        {
           // TODO: Decide how it will close the file
        }
        
        void OpenDialog(string filter,string initialDirectory,string title,gameType game) {
            OpenFileDialog fileDialog = new OpenFileDialog(); // creates the open file dialog
            if (initialDirectory!="NO_INITDIR") {
        	   fileDialog.InitialDirectory = initialDirectory; // set directory to current directory
            }

        	// TODO: set formats Zenda can open
        	fileDialog.Filter = filter;
        	fileDialog.Title = title;
        	fileDialog.RestoreDirectory = true;
        	// show open file dialog & check if user has choosed a file
        	if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
        		try
                {
        		    file = new FileStream(fileDialog.FileName,FileMode.Open); // TODO: Decide what FileMode will be used
        		    MenuFileClose.IsEnabled = true;
        		    onOpenFile(game,fileDialog.FileName); // call onOpenFile to indicate that the file was open
        		}
        		catch (Exception ex)
                {
        			System.Windows.MessageBox.Show("ERROR:\n\n"+ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        		}
        	}        	
        }
        
        /*
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
        */
        
        // Open button on click event handlers for each game
        private void OpenButtonDriv3rClick(object sender, RoutedEventArgs e) {
       	    // NOTE From BuilderDemo7: Directory.GetCurrentDirectory().ToString() = placeholder as the registry keys to the game paths still doesn't exist.
        	OpenDialog(Driv3rFF,Directory.GetCurrentDirectory().ToString(),"Open a file from Driv3r",gameType.Driv3r);
        }

        private void OpenButtonDPLClick(object sender, RoutedEventArgs e) {
       	    // NOTE From BuilderDemo7: Directory.GetCurrentDirectory().ToString() = placeholder as the registry keys to the game paths still doesn't exist.
        	OpenDialog(DriverPLFF,Directory.GetCurrentDirectory().ToString(),"Open a file from Driver: Parallel Lines",gameType.DriverPL);
        }       

        private void OpenButtonDSFClick(object sender, RoutedEventArgs e) {
       	    // NOTE From BuilderDemo7: Directory.GetCurrentDirectory().ToString() = placeholder as the registry keys to the game paths still doesn't exist.
        	OpenDialog(DriverSFFF,Directory.GetCurrentDirectory().ToString(),"Open a file from Driver: San Francisco",gameType.DriverSF);
        }     

        private void OpenButtonDriverWiiClick(object sender, RoutedEventArgs e) {
       	    // NOTE From BuilderDemo7: Directory.GetCurrentDirectory().ToString() = placeholder as the registry keys to the game paths still doesn't exist.
        	OpenDialog(DriverWiiFF,"NO_INITDIR","Open a file from Driver Wii",gameType.DriverWii);
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