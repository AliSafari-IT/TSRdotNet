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

        public TSAnalyze ()
            {
            InitializeComponent ();
            // There are several options to initialize thengine, but by default the following suffice:
            REngine engine = REngine.GetInstance();

            //init the R engine            
            REngine.SetEnvironmentVariables ();
            engine = REngine.GetInstance ();
            engine.Initialize ();


            //prepare data
            List<int> size = new List<int>() { 29, 33, 51, 110, 357, 45, 338, 543, 132, 70, 103, 301, 146, 10, 56, 243, 238 };
            List<int> population = new List<int>() { 3162, 11142, 3834, 7305, 81890, 1339, 5414, 65697, 11280, 4589, 320, 60918, 480, 1806, 4267, 63228, 21327 };

            var docPath = Directory.GetCurrentDirectory();
            //var myDir = $@"{docPath}\output";
            var myDir = bingPathToAppDir ("output");

            Directory.CreateDirectory (myDir);

            Console.WriteLine (Directory.Exists (myDir));
            Console.WriteLine (myDir);
            Console.WriteLine (bingPathToAppDir ("output"));

            Console.WriteLine ("my image location {0}", myDir);

            fileName = myDir + "\\myplot.png";

            //calculate
            IntegerVector sizeVector = engine.CreateIntegerVector(size);
            engine.SetSymbol ("size", sizeVector);

            IntegerVector populationVector = engine.CreateIntegerVector(population);
            engine.SetSymbol ("population", populationVector);

            CharacterVector fileNameVector = engine.CreateCharacterVector(new[] { fileName });
            engine.SetSymbol ("fileName", fileNameVector);

            engine.Evaluate ("reg <- lm(population~size)");
            engine.Evaluate ("png(filename=fileName, width=6, height=6, units='in', res=100)");
            engine.Evaluate ("plot(population~size)");
            engine.Evaluate ("abline(reg)");
            engine.Evaluate ("dev.off()");

            //clean up
            engine.Dispose ();

            //output
            Console.WriteLine ("");
            Console.WriteLine ("Press any key to exit");
            Console.ReadKey ();



            }

        public static string bingPathToAppDir (string localPath)
            {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
        Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString ();
            }
        }

        }
    }