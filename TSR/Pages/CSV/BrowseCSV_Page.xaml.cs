﻿using Microsoft.Win32;
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
using TSR.UserControls;

namespace TSR.Pages.CSV
    {
    /// <summary>
    /// Interaction logic for BrowseCSV.xaml
    /// </summary>
    public partial class BrowseCSV_Page : Page
        {
        public char delimiter;
        public string fileName;

        public BrowseCSV_Page ()
            {
            InitializeComponent ();
            }

        private void PrepareNeededTS_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("\n\n********** Preparing the time series needed for the next step *************\n");


            }

        private void TextBox_Browse_TextChanged (object sender, TextChangedEventArgs e)
            {

            }
        private void TextBox_Browse_PreviewMouseDown (object sender, MouseButtonEventArgs e)
            {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName (System.Diagnostics.Process.GetCurrentProcess ().MainModule.FileName);

            if (openFileDialog.ShowDialog () == true)
                {
                TextBox_Browse.Text = openFileDialog.FileName;
                TextBox_Browse.FontSize = 11;
                TextBox_Browse.ToolTip = "Click again to select another file";

                importCSVFile.IsEnabled = true;
                delimiterSPanel.IsEnabled = true;

                next_open_csv.Visibility = Visibility.Hidden;

                next_list_delimiter.Visibility = Visibility.Visible;

                }

            }

        private void importCSVFile_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("\n\n********** Getting the header fields of the loaded time series. *************\n");
            delimiter = GetDelimiter ();
            fileName = TextBox_Browse.Text;

            //TSAnalyze tsa = new TSAnalyze(this);
            //tsa.ShowDialog ();
            

            Console.WriteLine ("main.loadCSVpage.Content = parseCSV;");
 //           this.loadCSVpage.Content = new ParseCSV_Page (fileName, delimiter);

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

        }
    }