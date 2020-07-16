using Microsoft.Win32;
using RDotNet;
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

namespace TSR
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        private REngine engine;

        public char delimiter { get; private set; }

        public MainWindow ()
            {
            InitializeComponent ();
            // There are several options to initialize thengine, but by default the following suffice:
            engine = REngine.GetInstance ();
            engine.Initialize ();
            }

        private void TextBox_Browse_PreviewMouseDown (object sender, MouseButtonEventArgs e)
            {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName (System.Diagnostics.Process.GetCurrentProcess ().MainModule.FileName);

            if (openFileDialog.ShowDialog () == true)
                {
                TextBox_Browse.Text = openFileDialog.FileName;
                TextBox_Browse.Height = 40;
                TextBox_Browse.FontSize = 12;
                TextBox_Browse.ToolTip = "Click again to select another file";
                importCSVFile.IsEnabled = true;
                }
            }

        private void importCSVFile_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("\n\n********** Getting the header fields of the loaded time series. *************\n");
            TSAnalyze tsa = new TSAnalyze();
            tsa.ShowDialog ();


            }

        private char GetDelimiter ()
            {
            if (semicolonDelimiter.IsChecked == true)
                {
                return ';';
                }
            if (tabDelimiter.IsChecked == true)
                {
                return '\t';
                }
            if (spaceDelimiter.IsChecked == true)
                {
                return ' ';
                }
            if (OtherDelimiter.Text.Length > 0)
                {
                return OtherDelimiter.Text[0];
                }
            return ','; // default delimiter
            }


        private void OtherDelimiter_LostFocus (object sender, RoutedEventArgs e)
            {
            if (!OtherDelimiter.Text.Equals (""))
                {
                commaDelimiter.IsChecked = false;
                semicolonDelimiter.IsChecked = false;
                tabDelimiter.IsChecked = false;
                spaceDelimiter.IsChecked = false;
                }
            delimiter = GetDelimiter ();
            }

        private void PrepareNeededTS_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("\n\n********** Preparing the time series needed for the next step *************\n");

            }

        }
    }
