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

namespace TSR.Pages.CSV
    {
    /// <summary>
    /// Interaction logic for ParseCSV.xaml
    /// </summary>
    public partial class ParseCSV_Page : Page
        {
        private string fileName;
        private char delimiter;

        public ParseCSV_Page ()
            {
            InitializeComponent ();
            }

        public ParseCSV_Page (string fileName, char delimiter)
            {
            this.fileName = fileName;
            this.delimiter = delimiter;
            }
        }
    }
