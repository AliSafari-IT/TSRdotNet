using Microsoft.Win32;
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
                TextBox_Browse.Text = openFileDialog.FileName;
                TextBox_Browse.Height = 40;
                TextBox_Browse.FontSize = 12;
                importCSVFile.Visibility = Visibility.Visible;
                TextBox_Browse.ToolTip = "Click again to select another file";
                importCSVFile.IsEnabled = true;
                }
            }

        private void importCSVFile_Click (object sender, RoutedEventArgs e)
            {
            Console.WriteLine ("Moving to the next window: ");

            //TS_LoadData loadData = new TS_LoadData(this);

            //loadData.ShowDialog ();

            }
        }

    }
