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
using System.Windows.Shapes;
using TSR.Model;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for ARiMAProcess.xaml
    /// </summary>
    public partial class ARiMAProcess : Window
        {
        private TSreview tsReview;
        private REngine engine;

        public ARiMAProcess ()
            {
            InitializeComponent ();
            }

        public ARiMAProcess (TSreview tsReview) : base ()
            {
            this.tsReview = tsReview;
            InitializeComponent ();
            InitializeRDotNet ();
            }
        public void InitializeRDotNet ()
            {
            REngine.SetEnvironmentVariables ();
            engine = REngine.GetInstance ();
            Console.WriteLine ("REngine version: " + engine.DllVersion);
            try
                {
                if (engine == null)
                    {
                    string rhome = System.Environment.GetEnvironmentVariable("R_HOME");
                    Console.WriteLine ("R_HOME: " + rhome);
                    if (string.IsNullOrEmpty (rhome))
                        {
                        rhome = (string) Registry.GetValue (@"HKEY_LOCAL_MACHINE\Software\R-core\R", "InstallPath", "C:");
                        Environment.SetEnvironmentVariable ("R_HOME", rhome);
                        Environment.SetEnvironmentVariable ("PATH", System.Environment.GetEnvironmentVariable ("PATH") + ";" +
                                                                            rhome + @"\bin\i386;" +
                                                                            rhome + @"\bin\x64;");
                        }
                    engine.Initialize ();
                    Console.WriteLine ("REngine has been initialized successfully!");
                    }
                engine = REngine.GetInstance ("RDotNet");
                engine.Initialize ();
                }
            catch (Exception ex)
                {
                Console.WriteLine ("Error initializing RDotNet: " + ex.Message);
                MessageBox.Show ("Error initializing RDotNet: " + ex.Message, "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown ();
                }
            }
        public void Calculate (string calc)
            {
            REngine.SetEnvironmentVariables ();
            engine = REngine.GetInstance ();
            Console.WriteLine ("REngine version: " + engine.DllVersion);
            try
                {
                if (engine == null)
                    {
                    string rhome = System.Environment.GetEnvironmentVariable("R_HOME");
                    Console.WriteLine ("R_HOME: " + rhome);
                    if (string.IsNullOrEmpty (rhome))
                        {
                        rhome = (string) Registry.GetValue (@"HKEY_LOCAL_MACHINE\Software\R-core\R", "InstallPath", "C:");
                        Environment.SetEnvironmentVariable ("R_HOME", rhome);
                        Environment.SetEnvironmentVariable ("PATH", System.Environment.GetEnvironmentVariable ("PATH") + ";" +
                                                                            rhome + @"\bin\i386;" +
                                                                            rhome + @"\bin\x64;");
                        }
                    engine.Initialize ();
                    Console.WriteLine ("REngine has been initialized successfully!");
                    }
                engine = REngine.GetInstance ("RDotNet");
                engine.Initialize ();
                }
            catch (Exception ex)
                {
                Console.WriteLine ("Error initializing RDotNet: " + ex.Message);
                }

            Console.WriteLine ("Please enter the calculation");
            string input = calc;

            //calculate
            CharacterVector vector = engine.Evaluate(input).AsCharacter();
            string result = vector[0];

            //clean up
            engine.Dispose ();

            //output
            Console.WriteLine ("");
            Console.WriteLine ("Result of "+ input + " is '{0}'", result);
            //calculation.Content = calc;
            //calcResult.Text = "= "+result;
            }

        private void Button_Click (object sender, RoutedEventArgs e)
            {
            string calc = "2+3";
            Calculate (calc);
            string inputRead= new DescStat("Insert something", "Title", "Arial", 20).ShowDialog();
            }

        }
    }
