using System;
using System.IO;
using System.Runtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        // check if datamanagerui is shown or not
        private bool shareUIShown = false;

        public Form1()
        {
            InitializeComponent();
            //this.Icon = Properties.Resources.logoFilledBlue.ico;
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            // get screen size
            Screen currentScreen = Screen.FromControl(this);
            Rectangle screenSize = currentScreen.WorkingArea;
            /* This app closes whenever the main window is focused
             * set the form to fill the entire screen so that whenever we click outside of it
             * the entire app is closed */
            this.Size = new Size(screenSize.Width, screenSize.Height);
            this.CenterToScreen();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Opacity = 0;
            base.OnLoad(e);
        }

        protected override void OnActivated(EventArgs e)
        {
           // MessageBox.Show("Hello, World!");
            base.OnActivated(e);
            // if shareUI already shown, close app
            if (shareUIShown)
            {
                Application.Exit();
            }
        }

        async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            // create datapackage
            DataPackage dp = args.Request.Data;
            // begin loading files
            try
            {
                // create List to hold all files to share
                var filesToShare = new List<IStorageItem>();
                // get files to share from args
                string[] argv = Environment.GetCommandLineArgs();
                if (argv.Length > 1)
                {
                    // Set properties of shareUI
                    dp.Properties.Title = "Share from Simple Photo Viewer";
                    if (argv.Length == 2)
                    {
                        // only 1 photo is being shared
                        dp.Properties.Description = Path.GetFileName(argv[1]);
                    } else
                    {
                        dp.Properties.Description = (argv.Length - 1) + " photos";
                    }
                    
                    // loop through command line arguments and add each file
                    // the filenames will start from index 1
                    for (int i = 1; i < argv.Length; i++)
                    {
                        StorageFile imageFile = await StorageFile.GetFileFromPathAsync(argv[i]);
                        filesToShare.Add(imageFile);
                    }
                    dp.SetStorageItems(filesToShare);
                }
            }
            finally
            {
                shareUIShown = true;
                deferral.Complete();
            }
        }

            private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IntPtr hwnd = Process.GetCurrentProcess().MainWindowHandle;
            var dtm = DataTransferManagerHelper.GetForWindow(hwnd);

            // Set datapackage to dtm
            dtm.DataRequested += OnDataRequested;

            DataTransferManagerHelper.ShowShareUIForWindow(hwnd);

        }
    }
}
