using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.Specialized;
using RDotNet;
using System.IO;
using System;
using System.Windows;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for TSAnalyze.xaml
    /// </summary>
    public partial class TSAnalyze : Window
        {
        private string fileName;
        private MainWindow mainWindow;
        private string[] headers;
        private string selectedTimeSeries;
        private char delimiter;

        public TSAnalyze (MainWindow mainWindow)
            {
            this.mainWindow = mainWindow;
            fileName = mainWindow.fileName;
            InitializeComponent ();
            Console.WriteLine ("File name is: " + fileName);

            delimiter = mainWindow.delimiter;

            loadComboBoxItems ();

            }


        private void headersComboList_SelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            {
            selectedTimeSeries = e.AddedItems[0].ToString ();
            Console.WriteLine ("\nSelected time seris is " + selectedTimeSeries + "\n");
            }

        private void loadComboBoxItems ()
            {
            string[] headers=getCSV_Header (fileName, delimiter);

            Console.WriteLine ("Number of columns: {0}.", headers.Length);
            //MessageBox.Show ("number of columns " + headers.Length, "Verifying the choice of list separator ", (MessageBoxButton) MessageBoxButtons.OKCancel);

            //Fill in the Combo Box for choosing the time series to extract from the source file
            foreach (string colName in headers)
                {
                headersComboList.Items.Add (colName);
                }
            headersComboList.SelectedItem = headers[headers.Length - 1];
            }

        // returns an array of strings representing the different fields of the csv file
        public string[] getCSV_Header (string filename, char delimiter)
            {
            StreamReader fileReader = null;
            try
                {
                fileReader = new StreamReader (@filename);
                }
            catch (FileNotFoundException)
                {
                return null;
                }
            string header = fileReader.ReadLine();
            header = header.Replace ('"', ' ');
            Console.WriteLine ("selectedTimeSeries delimiter is: "+delimiter);
            headers = header.Split (delimiter).Select (s => s.Trim ()).Where (s => s != String.Empty).ToArray ();

            headers.ToList ().ForEach (i => Console.WriteLine (i.ToString ()));
            Console.WriteLine ("[{0}]", string.Join (", ", headers));

            fileReader.Close ();
            return headers;
            }

        private void previousButton_Click (object sender, RoutedEventArgs e)
            {
            this.Close ();
            mainWindow.Show ();
            }
        }

    }
