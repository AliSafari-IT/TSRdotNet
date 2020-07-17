using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.Specialized;
using RDotNet;
using System.IO;
using System;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;

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
        private char delimiter;
        public bool tsHasTime;

        public string selectedTimeSeries;
        public int var_ts_idx;
        public int dateIndx;
        public int timeIndx;

        private List<string[]> tsData;



        public TSAnalyze (MainWindow mainWindow)
            {
            this.mainWindow = mainWindow;
            fileName = mainWindow.fileName;
            InitializeComponent ();
            Console.WriteLine ("File name is: " + fileName);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            delimiter = mainWindow.delimiter;
            tsHasTime = false;
            loadComboBoxItems ();
            selectedTimeSeries = (string) headersComboList.SelectedItem;
            Console.WriteLine ("\n1. Selected time seris is " + selectedTimeSeries + "\n");
            }

        private void headersComboList_SelectionChanged (object sender, SelectionChangedEventArgs e)
            {
            selectedTimeSeries = e.AddedItems[0].ToString ();
            Console.WriteLine ("\n2. Selected time seris is " + selectedTimeSeries + "\n");
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
            Console.WriteLine ("selectedTimeSeries delimiter is: " + delimiter);
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


        private void Make_TS_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("\n\n********** Extracting the chosen time series. *************\n");
            var_ts_idx = GetVarTSindx ();   //Get the index of the selected time series in the 
            dateIndx = GetDateIndx ();      //Get the index of date column in the source csv file
            if (tsHasTime) timeIndx = GetTimeIndx ();       //Get the index of time column in the source csv file
            tsData = GetTS (); // Get the selected time series
            }


        private int GetDateIndx ()
            {
            dateIndx = 0;
            for (int j = 0; j < headers.Length; j++)
                {
                if (String.Compare (headers[j], dateColTitle.Text) == 0)
                    dateIndx = j;
                }
            Console.WriteLine ("\ndateIndex  => " + dateIndx);
            return dateIndx;
            }

        private int GetTimeIndx ()
            {
            timeIndx = 0;
            for (int j = 0; j < headers.Length; j++)
                {
                if (String.Compare (headers[j], timeColTitle.Text) == 0)
                    timeIndx = j;
                }
            Console.WriteLine ("\ntimeIndex => " + timeIndx);
            return timeIndx;
            }


        private int GetVarTSindx ()
            {
            for (int j = 0; j < headers.Length; j++)
                {
                if (String.Compare (headers[j], selectedTimeSeries) == 0)
                    {
                    var_ts_idx = j;
                    }
                }
            Console.WriteLine ("\nselectedTimeSeriesIndex => " + var_ts_idx);
            return var_ts_idx;
            }



        private List<string[]> GetTS ()
            {
            List<string[]> data = new List<String[]>();
            CSVReader csvReader = new CSVReader();
            data = csvReader.getData (GetVarTSindx (), GetDateIndx (), this.fileName, this.delimiter, tsHasTime, GetTimeIndx ());
            return data;
            }


        private void confirmTS_Click (object sender, RoutedEventArgs e)
            {
            MakeTSpanel.IsEnabled = true;
            stackPanel_ImportBtn.Visibility = Visibility.Collapsed;
            stackPanel_hiddenTitle.Visibility = Visibility.Visible;
            next_rd_dateTime.Visibility = Visibility.Visible;
            setTimeColumnTitle ();
            setDateColumnTitle ();
            }


        private void setTimeColumnTitle ()
            {
            setDateColumnTitle ();
            string stringToCheck = "time";
            int stringToCheckIndex = -1;
            string elementInArray = "Not Defined or Not Found";
            timeColTitle.Text = elementInArray;
            if (Array.Exists<string> (headers, (Predicate<string>) delegate (string s)
                {
                    stringToCheckIndex = s.IndexOf (stringToCheck, StringComparison.OrdinalIgnoreCase);
                    elementInArray = s;
                    return stringToCheckIndex > -1;
                    }))
                {
                timeColTitle.Text = elementInArray;
                }
            }

        private void setDateColumnTitle ()
            {

            dateColTitle.Text = "Not Defined or Not Found";
            timeColTitle.Text = "The chosen time seies have only date but no time to display.";


            var match = headers.FirstOrDefault(c => c.IndexOf("date", StringComparison.OrdinalIgnoreCase) > -1);

            if (match != null)
                dateColTitle.Text = match;

            }


        private void rBtn_onlyDate_Checked (object sender, RoutedEventArgs e)
            {
            tsHasTime = false;
            }

        private void rBtn_DateTime_Click (object sender, RoutedEventArgs e)
            {
            tsHasTime = true;
            }

        private void rBtn_DateTime_Checked (object sender, RoutedEventArgs e)
            {

            }

        private void questionMarkDate_MouseEnter (object sender, System.Windows.Input.MouseEventArgs e)
            {

            }

        private void questionMarkDate_MouseLeave (object sender, System.Windows.Input.MouseEventArgs e)
            {

            }

        private void questionMarkTime_MouseEnter (object sender, System.Windows.Input.MouseEventArgs e)
            {

            }

        private void questionMarkTime_MouseLeave (object sender, System.Windows.Input.MouseEventArgs e)
            {

            }

        private void headersComboList_SelectionChanged_1 (object sender, SelectionChangedEventArgs e)
            {

            }
        }

    internal class CSVReader
        {
        // returns an array of strings representing the different fields of the csv file
        public string[] getHeaders (string filename, char delimiter)
            {
            StreamReader filereader = null;
            try
                {
                filereader = new StreamReader (@filename);
                }
            catch (FileNotFoundException)
                {
                return null;
                }
            string header = filereader.ReadLine();
            header = header.Replace ('"', ' ');
            string[] headers = header.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
            return headers;
            }

        // returns the data in a list of string arrays in the form of [date, time, xvalue, yvalue] if the dataset contains both a date and a time
        public List<string[]> getData (int xIndex, int dateIndex, string filename, char delimiter, bool hastime, int timeIndex)
            {
            StreamReader reader = null;
            try
                {
                reader = new StreamReader (@filename);
                }
            catch (FileNotFoundException)
                {
                return null;
                }
            string line = reader.ReadLine();
            List<String[]> data = new List<String[]>();
            double variableValue; int k = 0;
            while ((line = reader.ReadLine ()) != null)
                {
                line = line.Replace ('"', ' ');

                string[] values = line.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();

                Console.WriteLine ("CSV LN {0}: [{1}]", k++, string.Join (", ", values));

                if (hastime)
                    {
                    try
                        {
                        if (values.Length == 0)
                            {
                            throw new System.FormatException ();
                            }
                        if (Double.TryParse (values[xIndex], NumberStyles.Any, CultureInfo.CurrentCulture, out variableValue))
                            {

                            }
                        }
                    catch (System.FormatException)
                        {
                        continue;
                        }
                    string[] linedata = new string[] { values[dateIndex], values[timeIndex], variableValue.ToString() };
                    data.Add (linedata);
                    }
                else
                    {
                    try
                        {
                        if (values.Length == 0)
                            {
                            throw new System.FormatException ();
                            }
                        if (Double.TryParse (values[xIndex], NumberStyles.Any, CultureInfo.CurrentCulture, out variableValue))
                            {

                            }
                        }
                    catch (System.FormatException)
                        {
                        continue;
                        }
                    string[] linedata = new string[] { values[dateIndex], values[xIndex] };
                    data.Add (linedata);
                    }
                }
            return data;
            }
        }
    }
