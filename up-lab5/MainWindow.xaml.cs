using Dynamsoft.Core;
using Dynamsoft.TWAIN;
using Dynamsoft.TWAIN.Enums;
using Dynamsoft.TWAIN.Interface;
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

namespace up_lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAcquireCallback
    {
        private TwainManager twain=null;
        private string productKey = "t0085oQAAAArwO/cwe/1bBxHcLnR11M2KM6y7mvPEuHQcjEst8sjzwRe60Xu8qHEu+R4L6wxY/sf8rlZ9LyCwZU4JLYzeW2v8Np37wYfJYXDrxUsdGeUf0w==";
        private string licenseKey = "t0068MgAAAHHmKDNbUkX6w4t3T6rJyl8C1Z/KWY93as4nzMua4yRe8lU7xcuLoG/Y1N8Lda0pckJnk988kROODdF3M4GJMEw=;";
        private ImageCore imageCore = null;
        private List<int> resolutions = new List<int>(){50,100,150,200,300,400,500,600};
        private int fileNr = 0;
        public MainWindow()
        {
            InitializeComponent();

            //m_StrProductKey = m_StrProductKey + ";" + LicenseLoader.ReadLocalLicense();
            productKey = productKey + ";" + licenseKey;
            twain = new TwainManager(productKey);
            imageCore = new ImageCore();
            dsViewer.Bind(imageCore);

            //init res combo box
            ResolutionComboBox.ItemsSource = resolutions;
            ResolutionComboBox.SelectedIndex = 3;

            //init color combo box
            ColorComboBox.ItemsSource = Enum.GetValues(typeof(TWICapPixelType)).Cast<TWICapPixelType>(); ;
            ColorComboBox.SelectedIndex = 2;

            //nit sliders
            ContrastSlider.Minimum = -1000;
            ContrastSlider.Maximum = 1000;
            BrightnessSlider.Minimum = -1000;
            BrightnessSlider.Maximum = 1000;
            ContrastSlider.Value = 0;
            BrightnessSlider.Value = 0;
        }

        public void OnPostAllTransfers()
        {
            //throw new NotImplementedException();
        }

        public bool OnPostTransfer(System.Drawing.Bitmap bit, string info)
        {
            bit.Save("C:/Users/oskro/Desktop/fold/" + fileNr++ + ".png");
            imageCore.IO.LoadImage(bit);
            return true;
        }

        public void OnPreAllTransfers()
        {
            //throw new NotImplementedException();
        }

        public bool OnPreTransfer()
        {
            return true;
        }

        public void OnSourceUIClose()
        {
            //throw new NotImplementedException();
        }

        public void OnTransferCancelled()
        {
            //throw new NotImplementedException();
        }

        public void OnTransferError()
        {
           //throw new NotImplementedException();
        }

        public bool IfGetImageInfo
        {
            get
            {
                return true;
            }
        }

        public bool IfGetExtImageInfo
        {
            get
            {
                return true;
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            twain.SelectSource();
            twain.CloseSource();
            twain.OpenSource();
            try
            {
                twain.AcquireImage(this as IAcquireCallback);
            }
            catch (TwainException) { }
        }

        private void ScanUIButton_Click(object sender, RoutedEventArgs e)
        {
            twain.SelectSource();
            twain.CloseSource();
            twain.OpenSource();
            twain.IfShowUI = false; //Hide the User Interface of the scanner
            twain.IfFeederEnabled = true; //Use the document feeder for batch scan
            twain.IfDuplexEnabled = false; //Scan in Simplex mode
            twain.PixelType = (TWICapPixelType)ColorComboBox.SelectedItem; //Scan pages in Grey
            
            twain.Resolution = (int)ResolutionComboBox.SelectedItem; //Scan pages in 200 DPI
            twain.Contrast = (float)ContrastSlider.Value;
            twain.Brightness = (float)BrightnessSlider.Value;
            try
            {
                twain.AcquireImage(this as IAcquireCallback);
            }
            catch (TwainException) { }
        }

    }
}
