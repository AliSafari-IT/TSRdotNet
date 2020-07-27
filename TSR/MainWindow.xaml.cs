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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public char delimiter;
        public string fileName;

        public MainWindow ()
            {
            InitializeComponent ();
            }

        private void TextBox_Browse_PreviewMouseDown (object sender, MouseButtonEventArgs e)
            {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName (System.Diagnostics.Process.GetCurrentProcess ().MainModule.FileName);

            if (openFileDialog.ShowDialog () == true)
                {
                TextBox_Browse.ToolTip = "Click again to select another file!";
                TextBox_Browse.FontSize = 11;
                TextBox_Browse.Text = openFileDialog.FileName;
                Console.WriteLine ("\nMainWindow.1. Browsed file name (TextBox_Browse.Text): " + TextBox_Browse.Text);

                TextBox_Browse_SelectedTS.Visibility = Visibility.Collapsed;
                next_open_selectedTS.Visibility = Visibility.Collapsed;
                browsePanel.SetValue (Grid.ColumnSpanProperty, 8);
                TextBox_Browse.SetValue (Grid.ColumnSpanProperty, 8);

                importCSVFile.IsEnabled = true;
                delimiterSPanel.IsEnabled = true;

                next_open_csv.Visibility = Visibility.Hidden;

                next_list_delimiter.Visibility = Visibility.Visible;
                }
            }

        private void importCSVFile_Click (object sender, RoutedEventArgs e)
            {
            delimiter = GetDelimiter ();
            Console.WriteLine ("\nMainWindow.2. GetDelimiter: \t" + delimiter);

            fileName = TextBox_Browse.Text;

            TSAnalyze tsa = new TSAnalyze(this);
            tsa.ShowDialog ();
            }

        public char GetDelimiter ()
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
            //Console.WriteLine ("\n\n********** Preparing the time series needed for the next step *************\n");

            }

        private void TextBox_Browse_TextChanged (object sender, TextChangedEventArgs e)
            {
            }

        private void TextBox_Browse_SelectedTS_PreviewMouseDown (object sender, MouseButtonEventArgs e)
            {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "STS files (*.sts)|*.STS|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName (System.Diagnostics.Process.GetCurrentProcess ().MainModule.FileName);
            importSelectedTS.Visibility = Visibility.Visible;
            if (openFileDialog.ShowDialog () == true)
                {
                TextBox_Browse_SelectedTS.Text = openFileDialog.FileName;
                TextBox_Browse_SelectedTS.FontSize = 11;
                TextBox_Browse_SelectedTS.ToolTip = "Click again to select another Time Series";
                browsePanel.Visibility = Visibility.Collapsed;
                delimiterSPanel.Visibility = Visibility.Collapsed;
                TextBox_Browse_SelectedTS.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                next_open_selectedTS.Visibility = Visibility.Collapsed;
                TextBox_Browse_SelectedTS.SetValue (Grid.ColumnProperty, 1);
                TextBox_Browse_SelectedTS.SetValue (Grid.ColumnSpanProperty, 8);
                importSelectedTS.SetValue (Grid.ColumnProperty, 1);
                importSelectedTS.SetValue (Grid.RowProperty, 5);
                importSelectedTS.IsEnabled = true;

                next_open_csv.Visibility = Visibility.Hidden;
                next_list_delimiter.Visibility = Visibility.Visible;
                }

            }

        private void importSelectedTS_Click (object sender, RoutedEventArgs e)
            {
            delimiter = '\t';
            fileName = TextBox_Browse_SelectedTS.Text;
            Console.WriteLine ("\nMainWindow.importSelectedTS_Click. delimiter is {0} and the filename is: {1}." , delimiter, fileName);
            TSreview tsr= new TSreview(this);
            tsr.ShowDialog ();
            }
        }
    }
