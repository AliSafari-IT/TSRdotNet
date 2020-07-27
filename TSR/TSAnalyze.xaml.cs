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
using System.Diagnostics;
using Microsoft.Win32;
using System.Text;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for TSAnalyze.xaml
    /// </summary>
    public partial class TSAnalyze : Window
        {
        private readonly string fileName;
        private readonly MainWindow mainWindow;
        private string[] headers;
        private readonly char delimiter;
        private bool tsHasTime;

        public string selectedTimeSeries;
        public int var_ts_idx;
        public int dateIndx;
        public int timeIndx;
        private List<string[]> tsData;
        public string selectedTSFile;
        public string selectedTSFilePath;

        public bool TsHasTime { get => tsHasTime; set => tsHasTime = value; }

        public TSAnalyze (MainWindow mainWindow)
            {
            this.mainWindow = mainWindow;
            fileName = mainWindow.fileName;
            InitializeComponent ();
            Console.WriteLine ("\nTSAnalyze.1. File name (fileName) is: " + fileName);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            delimiter = mainWindow.delimiter;
            TsHasTime = false;
            loadComboBoxItems ();
            selectedTimeSeries = (string) headersComboList.SelectedItem;
            Console.WriteLine ("\nTSAnalyze.4. Selected column (selectedTimeSeries) is " + selectedTimeSeries + "\n");
            }

        private void headersComboList_SelectionChanged (object sender, SelectionChangedEventArgs e)
            {
            selectedTimeSeries = e.AddedItems[0].ToString ();
            }

        private void loadComboBoxItems ()
            {
            headers = getCSV_Header (fileName, delimiter);

            Console.WriteLine ("\nTSAnalyze.3. Number of columns (headers.Length) is {0}", headers.Length);
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
            catch (IOException)
                {
                MessageBox.Show ("The process cannot access the file '" + filename + "' because it is being used by another process.'", "Error#IOException");
                return null;
                }
            string header = fileReader.ReadLine();
            header = header.Replace ('"', ' ');

            headers = header.Split (delimiter).Select (s => s.Trim ()).Where (s => s != String.Empty).ToArray ();

            // headers.ToList ().ForEach (i => Console.WriteLine (i.ToString ()));
            Console.Write ("\nTSAnalyze.2. Header fields:  ==>  ");
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
            //Get the index of the selected time series in the 
            var_ts_idx = GetVarTSindx ();

            //Get the index of date column in the source csv file
            dateIndx = GetDateIndx ();

            //Get the index of time column in the source csv file
            if (TsHasTime)
                {
                timeIndx = GetTimeIndx ();
                }

            // Get the list of the selected time series 
            tsData = GetTS ();

            // Save the selected time series to a file with the extension *.sts (sts: selected time series)
            SaveToFile (tsData);

            // Check if the time series contain time columns
            if (TsHasTime)
                {
                Console.WriteLine ("\nTSAnalyze.10. this time series has time column => " + TsHasTime);
                }
            else
                {
                Console.WriteLine ("\nTSAnalyze.10. this time series has no time column => TS has time? " + TsHasTime);
                }

            // Move to the next window to analyse the selected time series
            TSreview tsr= new TSreview(this);
            tsr.ShowDialog ();
            }


        public void SaveToFile (List<string[]> tsData)
            {
            @selectedTSFile = Path.GetFileNameWithoutExtension (@fileName);
            Console.WriteLine ("\nTSAnalyze.9.1. SaveToFile => FileNameWithoutExtension (@fileName) =  @selectedTSFile: " + @selectedTSFile);

            string directoryOfSelectedFile = Path.GetDirectoryName(@fileName);
            @selectedTSFile += "_" + selectedTimeSeries + ".sts";
            Console.WriteLine ("\nTSAnalyze.9.2. SaveToFile => file name of the @selectedTSFile ==> " + @selectedTSFile);

            @selectedTSFilePath = directoryOfSelectedFile+Path.DirectorySeparatorChar +@selectedTSFile;
            Console.WriteLine ("\nTSAnalyze.9.3. SaveToFile => Full path of @selectedTSFile ==> \n@selectedTSFilePath: " + @selectedTSFilePath);

            try
                {
                using (TextWriter tw = new StreamWriter (@selectedTSFilePath))
                    {
                    if (TsHasTime)
                        {
                        tw.WriteLine ("{0},\t{1},\t{2}", headers[dateIndx], headers[timeIndx], headers[var_ts_idx]);
                        }
                    else
                        {
                        tw.WriteLine ("{0},\t{1}", headers[dateIndx], headers[var_ts_idx]);
                        }
                    foreach (var item in tsData)
                        {
                        tw.WriteLine ("{0}", string.Join (", ", item));
                        }
                    tw.Close ();
                    }
                }
            catch (IOException)
                {
                string msg = "'The process cannot access the file '"+@selectedTSFile+"' because it is being used by another process.";
                MessageBox.Show (msg);
                }
            }

        private int GetDateIndx ()
            {
            dateIndx = 0;
            for (int j = 0; j < headers.Length; j++)
                {
                if (String.Compare (headers[j], dateColTitle.Text) == 0)
                    dateIndx = j;
                }
            Console.WriteLine ("\nTSAnalyze.6. dateIndex in the source csv file => " + dateIndx);
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
            Console.WriteLine ("\nTSAnalyze.7. timeIndx in the source csv file => " + timeIndx);
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
            Console.WriteLine ("\nTSAnalyze.5. selectedTimeSeriesIndex => " + var_ts_idx);
            return var_ts_idx;
            }

        private List<string[]> GetTS ()
            {
            List<string[]> data = new List<String[]>();
            CSVReader csvReader = new CSVReader();
            data = csvReader.getData (GetVarTSindx (), GetDateIndx (), fileName, delimiter, TsHasTime, GetTimeIndx ());
            Console.WriteLine ("\nTSAnalyze.8. GetTS () => data = csvReader.getData");
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
            string elementInArray = "Not Defined or Not Found!";
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
            TsHasTime = false;
            }
        private void rBtn_onlyDate_Click (object sender, RoutedEventArgs e)
            {
            TsHasTime = false;
            }

        private void rBtn_DateTime_Click (object sender, RoutedEventArgs e)
            {
            TsHasTime = true;
            }

        private void rBtn_DateTime_Checked (object sender, RoutedEventArgs e)
            {
            TsHasTime = true;
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

        private void rBtn_onlyDate_Click_1 (object sender, RoutedEventArgs e)
            {

            }
        }

    internal class CSVReader
        {
        private string[] headers;

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
            headers = header.Split (delimiter).Select (s => s.Trim ()).Where (s => s != String.Empty).ToArray ();
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

                // Console.WriteLine ("CSV LN {0}: [{1}]", k++, string.Join (", ", values));
                try
                    {
                    if (values.Length == 0)
                        {
                        throw new System.FormatException ();
                        }
                    try
                        {
                        if (!Double.TryParse (values[xIndex], NumberStyles.Any, CultureInfo.CurrentCulture, out variableValue))
                            {
                            throw new IndexOutOfRangeException ();
                            }
                        }
                    catch (IndexOutOfRangeException)
                        {
                        Console.WriteLine ("Missing data (null at line {0}).", k + 1);
                        MessageBox.Show ("There is a problem with parsing data. Check this line " + (k + 1) + " and fix the problem; then try again.");

                        OpenFileToCorrect (filename);
                        continue;
                        }
                    }
                catch (System.FormatException)
                    {
                    continue;
                    }

                if (hastime)
                    {
                    string[] linedata = new string[] { values[dateIndex], values[timeIndex], variableValue.ToString()};
                    data.Add (linedata);
                    }
                else
                    {
                    string[] linedata = new string[] { values[dateIndex], values[xIndex] };
                    data.Add (linedata);
                    }
                }
            return data;
            }

        private void OpenFileToCorrect (string filename)
            {
            Process.Start (@"notepad.exe", filename);
            }
        }
    }
