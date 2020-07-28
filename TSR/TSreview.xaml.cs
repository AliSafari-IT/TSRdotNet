using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for TSreview.xaml
    /// </summary>
    public partial class TSreview : Window
        {
        private TSAnalyze tsAnalyze;
        private string selctedComboItem;
        private string stsFileSelected;
        private List<Double> ts_dataValues;
        public int arima_d_order;
        public int arima_p_order;
        public int arima_q_order;
        public double alpha;
        private int predictionNumber;
        private MainWindow mainWindow;
        private string[] headers;

        public TSreview ()
            {
            InitializeComponent ();
            }

        public TSreview (TSAnalyze tsAnalyze)
            {
            this.tsAnalyze = tsAnalyze;
            InitializeComponent ();
            Console.WriteLine ("\nTSreview.1. Getting SelectedItem for ==>  " + tsAnalyze.selectedTSFile);
            FillUp_TS_Combo (tsAnalyze.selectedTSFilePath);
            }

        public TSreview (MainWindow mainWindow)
            {
            this.mainWindow = mainWindow;
            InitializeComponent ();
            FillUp_TS_Combo (mainWindow.fileName);
            }

        private void FillUp_TS_Combo (string @selectedFile)
            {

            selctedComboItem = Path.GetFileName (@selectedFile);

            string directoryOfSelectedFile = Path.GetDirectoryName(@selectedFile);

            object[] tsFiles = new DirectoryInfo (directoryOfSelectedFile).GetFiles ("*.sts").ToArray ();

            foreach (object o in tsFiles)
                {
                Combo_TS_FileList.Items.Add (o.ToString ());
                if (selctedComboItem == o.ToString ())
                    {
                    Combo_TS_FileList.SelectedItem = o.ToString ();
                    }
                }
            Combo_TS_FileList.SelectedItem = selctedComboItem;
            Console.WriteLine ("3.2. Setting Combo_TS_FileList with SelectedItem: " + selctedComboItem);
            }

        private void Window_Loaded (object sender, RoutedEventArgs e)
            {

            if (Double.TryParse (numPredic.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double predictionNumber))
                {
                if (predictionNumber < 1)
                    numPredicExplain.Text = "If number of prediction np&lt;1, then number of predictions is calculated as a ratio of the total data number.";
                }
            else
                {
                numPredicExplain.Text = "Number of prediction np&gt;=1.";
                }

            // Get time series values from the selected PTS file
            ts_dataValues = get_TS_data (tsAnalyze.selectedTSFile, ',');  //Load data list

            //  Calculate mean value
            meanValue.Text = GetAverage ().ToString ();

            // median of elements
            medianValue.Text = GetMedian ().ToString ();

            //calculate the standard deviation
            sdValue.Text = getStandardDeviation ().ToString ();

            // Get max and min
            maxVal.Text = GetMax ().ToString ();
            minVal.Text = getMin ().ToString ();

            // Count the list elements 
            tsLength.Text = getLength ().ToString ();

            updateArimaOrders ();
            }

        private void FillInArimaModelButton ()
            {
            modelARIMA.Content = "ARIMA(" + pOrder.Text + "," + diffOrder.Text + "," + qOrder.Text + ")";
            }

        private int getLength ()
            {

            return ts_dataValues.Count;
            }

        private object getMin ()
            {
            return ts_dataValues.Min ();
            }

        private double GetMax ()
            {
            return ts_dataValues.Max ();
            }

        private double GetAverage ()
            {
            double ave = 0;
            try
                {
                ave = ts_dataValues.Average ();    //  Calculate mean value
                }
            catch (InvalidOperationException ex)
                {
                MessageBox.Show ("The sequence on which it is called is empty: (" + ex.Message +
                    "). Check if your source input STS file is a comma-separated values (CSV), in which each line of the file is a " +
                    "data record. Each STS record consists of two or three fields, separated by commas. If it is not a CSV, You can " +
                    "convert the file to a CSV (comma-separated values) file by using a spreadsheet application such as Microsoft Excel" +
                    " or LibreOffice Calc.", "Err# Null Reference Exception! ", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit (-1);
                }
            return ave;
            }

        private double getStandardDeviation ()
            {
            double sumOfSquaresOfDifferences = ts_dataValues.Select(val => (val - GetAverage()) * (val - GetAverage())).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / ts_dataValues.Count);
            return sd;
            }

        private double GetMedian ()
            {
            var ary = new double[ts_dataValues.Count];
            for (var ii = 0; ii < ts_dataValues.Count; ii++)
                {
                ary[ii] = Convert.ToDouble (ts_dataValues[ii]);
                }

            Array.Sort (ary);
            if (ary.Length % 2 != 0)
                {
                return ary[ary.Length / 2];
                }
            else
                {
                return (ary[ary.Length / 2 - 1] + ary[ary.Length / 2]) / 2;
                }
            }

        private double ts_sum (List<double> ts_dataValues)
            {
            double sum=0;
            foreach (double v in ts_dataValues)
                {
                sum += v;
                }
            return sum;
            }

        private List<Double> get_TS_data (string stsFile, char delimiter)
            {
            List<Double> ts = new List<Double>();
            StreamReader fileReader = null;
            string[] headers=GetHeader (stsFile, '\t');
            int fieldsNr = headers.Count();

            try
                {
                fileReader = new StreamReader (stsFile);
                }
            catch (FileNotFoundException)
                {
                string msg = "Error# File Not Found! "+stsFile;
                MessageBox.Show (msg);
                }

            string line = fileReader.ReadLine(); // skip header
            int lineNr=1;
            while ((line = fileReader.ReadLine ()) != null)
                {
                lineNr++;

                line = line.Replace ('"', ' ');
                string[] values = line.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();

                try
                    {
                    if (double.TryParse (values[values.Count () - 1], NumberStyles.Any, CultureInfo.CurrentCulture, out double number))
                        {
                        Console.WriteLine (string.Format ("Local culture results for the parsing of {0} yielded: {1}", values[values.Count () - 1], number));
                        ts.Add (number);
                        }
                    }
                catch (NullReferenceException ne)
                    {
                    Console.WriteLine ("Unable to parse '{0}'.", values[values.Count () - 1].ToString ());
                    MessageBox.Show ("An error just found in the input data: (" + ne.Message + "). Check line number: " + lineNr, "Err# Null Reference Exception! ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Environment.Exit (-1);
                    }
                catch (IndexOutOfRangeException ior)
                    {
                    Console.WriteLine ("Unable to parse '{0}'.", values[values.Count () - 1].ToString ());
                    MessageBox.Show ("An error just found in the input data: (" + ior.Message + "). Check line number: " + lineNr, "Err# Index Out Of Range Exception! ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Environment.Exit (-1);
                    }
                }
            return ts;
            }

        // returns an array of strings representing the different fields of the SELECTED time series file
        private string[] GetHeader (string stsFile, char delimiter)
            {
            StreamReader fileReader = null;
            try
                {
                fileReader = new StreamReader (@stsFile);
                }
            catch (IOException)
                {
                MessageBox.Show ("The process cannot access the file '" + @stsFile + "' because it is being used by another process.'", "Error#IOException");
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

        private void Combo_TS_FileList_SelectionChanged (object sender, SelectionChangedEventArgs e)
            {

            stsFileSelected = e.AddedItems[0].ToString ();

            // Get time series values from the selected PTS file
            ts_dataValues = get_TS_data (stsFileSelected, ',');  //Load data list


            //  Calculate mean value
            meanValue.Text = GetAverage ().ToString ();

            // median of elements
            medianValue.Text = GetMedian ().ToString ();

            //calculate the standard deviation
            sdValue.Text = getStandardDeviation ().ToString ();

            // Get max and min
            maxVal.Text = GetMax ().ToString ();
            minVal.Text = getMin ().ToString ();

            // Count the list elements 
            tsLength.Text = getLength ().ToString ();


            }

        private void Combo_Modeling_List_SelectionChanged (object sender, SelectionChangedEventArgs e)
            {

            }

        private void boxCox_Click (object sender, RoutedEventArgs e)
            {
            lambdaPanel.Visibility = Visibility.Visible;
            }

        private void logTrans_Click (object sender, RoutedEventArgs e)
            {
            lambdaPanel.Visibility = Visibility.Collapsed;
            }

        private void sigLevel_TextChanged (object sender, TextChangedEventArgs e)
            {

            }

        private void sigLevel_LostFocus (object sender, RoutedEventArgs e)
            {
            if (!double.TryParse (sigLevel.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out alpha))
                {
                string str= "Try again! The level of statistical significance (α) for 95% confidence intervals is 5% and for 99% confidence is 1%.";
                MessageBox.Show (str, "Invalid α level", MessageBoxButton.OK);
                }
            else if (alpha > 100 || alpha < 0)
                {
                MessageBox.Show ("α level beyound the range : " + alpha);
                }
            }

        private void updateArimaOrders ()
            {

            arima_d_order = getParsed (diffOrder.Text, "Err# ARIMA differencing is an integer value. ");
            arima_p_order = getParsed (pOrder.Text, "Err# p is an integer value. ");
            arima_q_order = getParsed (qOrder.Text, "Err# q is an integer value. ");

            }

        private int getParsed (string textNr, string v)
            {
            if (!int.TryParse (textNr, NumberStyles.Any, CultureInfo.CurrentCulture, out int number))
                {
                v += "Try again!";
                MessageBox.Show (v, "Wrong input!", MessageBoxButton.OK);
                }
            else
                {
                FillInArimaModelButton ();
                }

            return number;
            }

        private void Order_PreviewKeyDown (object sender, KeyEventArgs e)
            {
            if (e.Key == Key.Return)
                {
                updateArimaOrders ();
                }
            }

        //*****************************************************************************************************************************************************************************************//

        private void modelARIMA_Click (object sender, RoutedEventArgs e)
            {

            updateArimaOrders ();

            ARiMAProcess process = new ARiMAProcess();
            List<String[]> dataTS = new List<String[]>();


            }
        }
    }
