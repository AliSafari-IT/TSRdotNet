using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.Specialized;
using RDotNet;

namespace TSR
    {
    /// <summary>
    /// Interaction logic for TSAnalyze.xaml
    /// </summary>
    public partial class TSAnalyze : Window
        {
        public static TSAnalyze inst;
        private DataSet dsExperiments;
        private DataTable dtExperiments;
        private daoData _daoData = new daoData();
        private string dt = "WEIGHT_LENGTH";
        private int intID= 0;
        private bool bAdd = false;
        private bool bEdit = false;
        RStatistics _rStats = new RStatistics();
        private REngine engine;
        private bool bLoad = true;
        private Font fTitle = new Font("Times New Roman", 20, FontStyle.Bold);
        private Font fAxis = new Font("Times New Roman", 14, FontStyle.Bold);
        private Font fTick = new Font("Times New Roman", 10, FontStyle.Bold);
        private Font fLegend = new Font("Times New Roman", 10, FontStyle.Bold);
        private Color cTitle = new Color();
        private Color cAxis = new Color();
        private Color cTick = new Color();
        private Color cLegend = new Color();
        private ListDictionary seriesFont = new ListDictionary();
        List<string> xAxis = new List<string>();
        List<double> yAxis = new List<double>();
        List<string> titles = new List<string>();
        List<Font> fonts = new List<Font>();
        List<string> fontColors = new List<string>();
        List<string> textRotation = new List<string>();
        string sImagePath = "";
        private DataTable aovTable = new DataTable();

        public TSAnalyze ()
            {
            InitializeComponent ();
            // There are several options to initialize thengine, but by default the following suffice:
            engine = REngine.GetInstance ();
            engine.Initialize ();
            }

        public frmData (int id)
            {
            InitializeComponent ();
            // There are several options to initialize thengine, but by default the following suffice:
            engine = REngine.GetInstance ();
            engine.Initialize ();
            intID = id;
            setFilters (id);
            setGrid (id);
            setButtons (bAdd, bEdit);
            cTitle = Color.Black;
            cAxis = Color.Black;
            cLegend = Color.Black;
            cTick = Color.Black;
            }

        public static frmData CreateInstance ()
            {
            if (inst == null || inst.IsDisposed)
                {
                inst = new frmData ();
                }
            else
                {
                MessageBox.Show ("An Instance of the Experiments Form is already open.", "Form Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            return inst;
            }

        public static frmData CreateInstance (int id)
            {
            if (inst == null || inst.IsDisposed)
                {
                inst = new frmData (id);
                }
            else
                {
                MessageBox.Show ("An Instance of the Experiments Form is already open.", "Form Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            return inst;
            }

        private void setGrid (int id)
            {
            if (cbStart.Items.Count == 0)
                {
                MessageBox.Show ("Data is not available for this experiment.", "DATA NOT AVAILABLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bAdd = true;
                return;
                }

            bAdd = true;
            bEdit = true;

            string sWhere = " and TANK_NUM between " + cbStart.SelectedValue.ToString() + " and " + cbEnd.SelectedValue.ToString();
            int color = cbColor.SelectedIndex;
            int week = cbWeek.SelectedIndex;
            int sex = cbSex.SelectedIndex;

            if (!cbColor.Items[color].Equals ("ALL"))
                {
                sWhere += " and COLOR = '" + cbColor.Items[color] + "' ";
                }

            if (!cbWeek.Items[week].Equals ("ALL"))
                {
                sWhere += " and WEEK = '" + cbWeek.Items[week] + "' ";
                }

            if (!cbSex.Items[sex].Equals ("ALL"))
                {
                sWhere += " and SEX = '" + cbSex.Items[sex] + "' ";
                }


            dsExperiments = _daoData.getExData (id, sWhere);
            dtExperiments = dsExperiments.Tables[dt];
            dgExData.DataSource = dtExperiments;
            dgExData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

        private void setFilters (int id)
            {
            setStart (id);
            setEnd (id);
            setColor (id);
            setSex (id);
            setWeek (id);
            }

        private void setStart (int id)
            {
            DataSet dsStart = new DataSet();
            DataTable dtStart = new DataTable();

            dsStart = _daoData.getTankNum (id);
            dtStart = dsStart.Tables[dt];
            cbStart.DataSource = dtStart;
            cbStart.DisplayMember = "TANK_NUM";
            cbStart.ValueMember = "tank";

            }

        private void setEnd (int id)
            {
            DataSet dsStart = new DataSet();
            DataTable dtStart = new DataTable();

            dsStart = _daoData.getTankNum (id);
            dtStart = dsStart.Tables[dt];
            cbEnd.DataSource = dtStart;
            cbEnd.DisplayMember = "TANK_NUM";
            cbEnd.ValueMember = "tank";
            cbEnd.SelectedIndex = cbStart.Items.Count - 1;
            }

        private void setColor (int id)
            {
            DataSet dsColor = new DataSet();
            DataTable dtColor = new DataTable();

            cbColor.Items.Add ("ALL");
            dsColor = _daoData.getTankColor (id);
            dtColor = dsColor.Tables[dt];
            foreach (DataRow row in dtColor.Rows)
                {
                foreach (DataColumn col in dtColor.Columns)
                    {
                    cbColor.Items.Add (row[col.ColumnName].ToString ());
                    }
                }

            cbColor.SelectedIndex = 0;

            }

        private void setSex (int id)
            {
            cbSex.Items.Add ("M");
            cbSex.Items.Add ("F");
            cbSex.Items.Add ("ALL");
            cbSex.SelectedIndex = 2;
            }

        private void setWeek (int id)
            {
            DataSet dsWeek = new DataSet();
            DataTable dtWeek = new DataTable();

            cbWeek.Items.Add ("ALL");
            dsWeek = _daoData.getWeek (id);
            dtWeek = dsWeek.Tables[dt];

            foreach (DataRow row in dtWeek.Rows)
                {
                cbWeek.Items.Add (row["WEEK"].ToString ());
                }

            cbWeek.DisplayMember = "WEEK";
            cbWeek.ValueMember = "WEEK";
            cbWeek.SelectedIndex = 0;
            }

        private void btnFilter_Click (object sender, EventArgs e)
            {
            if (Convert.ToInt32 (cbStart.SelectedValue.ToString ()) > Convert.ToInt32 (cbEnd.SelectedValue.ToString ()))
                {
                MessageBox.Show ("Starting value must be lower or equal to ending value.", "Out of range violation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                }
            setGrid (intID);
            }

        private void btnClear_Click (object sender, EventArgs e)
            {
            cbColor.Items.Clear ();
            cbSex.Items.Clear ();
            cbWeek.Items.Clear ();

            setFilters (intID);
            setGrid (intID);
            }

        private void btnAdd_Click (object sender, EventArgs e)
            {
            frmFishData _frmFishData = frmFishData.CreateInstance(intID);
            _frmFishData.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _frmFishData.Show ();
            _frmFishData.BringToFront ();
            }

        private void setButtons (bool bAdd, bool bEdit)
            {
            btnAdd.Enabled = bAdd;
            btnEdit.Enabled = bEdit;
            }

        private void dgExData_CellDoubleClick (object sender, DataGridViewCellEventArgs e)
            {
            int row = Convert.ToInt32(dgExData.Rows[e.RowIndex].Cells["FI_ID"].Value);
            frmFishData _frmFishData = frmFishData.CreateInstance(intID, row);
            _frmFishData.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _frmFishData.Show ();
            _frmFishData.BringToFront ();
            }

        private void btnEdit_Click (object sender, EventArgs e)
            {
            int selectedrowindex = dgExData.SelectedCells[0].RowIndex;
            int row = Convert.ToInt32(dgExData.Rows[selectedrowindex].Cells["FI_ID"].Value);
            frmFishData _frmFishData = frmFishData.CreateInstance(intID, row);
            _frmFishData.StartPosition = FormStartPosition.WindowsDefaultLocation;
            _frmFishData.Show ();
            _frmFishData.BringToFront ();
            }

        private void btnExport_Click (object sender, EventArgs e)
            {
            var excelApp = new Excel.Application();
            DataSet ExcelData = new DataSet();
            DataTable dtExcelData = new DataTable();
            // Make the object visible.
            excelApp.Visible = true;

            // Create a new, empty workbook and add it to the collection returned  
            // by property Workbooks. The new workbook becomes the active workbook. 
            // Add has an optional parameter for specifying a praticular template.  
            // Because no argument is sent in this example, Add creates a new workbook. 
            excelApp.Workbooks.Add ();

            // This example uses a single workSheet. 
            Excel._Worksheet workSheet = excelApp.ActiveSheet;

            workSheet.Cells[1, "A"] = "COLOR";
            workSheet.Cells[1, "B"] = "TANK NUMBER";
            workSheet.Cells[1, "C"] = "FISH NUMBER";
            workSheet.Cells[1, "D"] = "WET WEIGHT";
            workSheet.Cells[1, "E"] = "LENGTH";
            workSheet.Cells[1, "F"] = "WEEK";
            workSheet.Cells[1, "G"] = "NOTES";
            workSheet.Cells[1, "H"] = "SEX";
            workSheet.Cells[1, "I"] = "INFO DATE";

            if (cbStart.Items.Count == 0)
                {
                MessageBox.Show ("Data is not available for this experiment.", "DATA NOT AVAILABLE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bAdd = true;
                return;
                }

            bAdd = true;
            bEdit = true;

            string sWhere = " and TANK_NUM between " + cbStart.SelectedValue.ToString() + " and " + cbEnd.SelectedValue.ToString();
            int color = cbColor.SelectedIndex;
            int week = cbWeek.SelectedIndex;
            int sex = cbSex.SelectedIndex;

            if (!cbColor.Items[color].Equals ("ALL"))
                {
                sWhere += " and COLOR = '" + cbColor.Items[color] + "' ";
                }

            if (!cbWeek.Items[week].Equals ("ALL"))
                {
                sWhere += " and WEEK = '" + cbWeek.Items[week] + "' ";
                }

            if (!cbSex.Items[sex].Equals ("ALL"))
                {
                sWhere += " and SEX = '" + cbSex.Items[sex] + "' ";
                }


            ExcelData = _daoData.getExData (intID, sWhere);
            dtExcelData = ExcelData.Tables[dt];


            for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                workSheet.Cells[i + 2, "A"] = dtExcelData.Rows[i]["COLOR"].ToString ();
                workSheet.Cells[i + 2, "B"] = dtExcelData.Rows[i]["TANK_NUM"].ToString ();
                workSheet.Cells[i + 2, "C"] = dtExcelData.Rows[i]["FISH_NUM"].ToString ();
                workSheet.Cells[i + 2, "D"] = dtExcelData.Rows[i]["WT_WEIGHT"].ToString ();
                workSheet.Cells[i + 2, "E"] = dtExcelData.Rows[i]["FISH_LENGTH"].ToString ();
                workSheet.Cells[i + 2, "F"] = dtExcelData.Rows[i]["WEEK"].ToString ();
                workSheet.Cells[i + 2, "I"] = dtExcelData.Rows[i]["INFO_DATE"].ToString ();
                workSheet.Cells[i + 2, "G"] = dtExcelData.Rows[i]["NOTES"].ToString ();
                workSheet.Cells[i + 2, "H"] = dtExcelData.Rows[i]["SEX"].ToString ();

                }

            for (int j = 1; j <= 9; j++)
                workSheet.Columns[j].AutoFit ();
            }

        private void frmData_Load (object sender, EventArgs e)
            {
            bLoad = true;
            SetChartComboBox ();
            SetYAxisChoice ();
            SetXAxisChoice ();
            bLoad = false;
            }

        private void SetYAxisChoice ()
            {
            cmbYAxis.Items.Clear ();
            cmbYAxis.Items.Add ("Fish Weight");
            cmbYAxis.Items.Add ("Fish Length");
            cmbYAxis.SelectedIndex = 0;
            }

        private void SetXAxisChoice ()
            {
            cmbXAxis.Items.Clear ();
            cmbXAxis.Items.Add ("Diet");
            cmbXAxis.Items.Add ("Color");
            cmbXAxis.Items.Add ("Tank");
            cmbXAxis.Items.Add ("Sex");
            cmbXAxis.Items.Add ("Week");
            cmbXAxis.SelectedIndex = 0;
            }

        private void SetChart ()
            {
            List<string> ReturnValues = new List<string>();
            if (!(String.IsNullOrEmpty (sImagePath)))
                File.Delete (sImagePath);
            string colorChange = "";

            //File.Delete(@"C:\RStat\whereamI.jpeg");

            xAxis.Clear ();
            yAxis.Clear ();
            fonts.Clear ();
            titles.Clear ();
            textRotation.Clear ();
            fontColors.Clear ();

            this.dtExperiments = this.dsExperiments.Tables[0];
            if (dtExperiments.Rows.Count == 0)
                return;

            titles.Add (txtChartTitle.Text);
            titles.Add (txtXAxisTitle.Text);
            titles.Add (txtYAxisTitle.Text);
            fonts.Add (fTitle);
            fonts.Add (fAxis);
            fonts.Add (fTick);
            fonts.Add (fLegend);
            textRotation.Add (txtXRotation.Text.ToString ());
            textRotation.Add (txtYRotation.Text.ToString ());

            colorChange = string.Format ("#{0:X2}{1:X2}{2:X2}{3:X2}", cTitle.A, cTitle.R, cTitle.G, cTitle.B);
            fontColors.Add ("#" + colorChange.Remove (0, 3));
            colorChange = string.Format ("#{0:X2}{1:X2}{2:X2}{3:X2}", cAxis.A, cAxis.R, cAxis.G, cAxis.B);
            fontColors.Add ("#" + colorChange.Remove (0, 3));
            colorChange = string.Format ("#{0:X2}{1:X2}{2:X2}{3:X2}", cTick.A, cTick.R, cTick.G, cTick.B);
            fontColors.Add ("#" + colorChange.Remove (0, 3));
            colorChange = string.Format ("#{0:X2}{1:X2}{2:X2}{3:X2}", cLegend.A, cLegend.R, cLegend.G, cLegend.B);
            fontColors.Add ("#" + colorChange.Remove (0, 3));

            // this.dtExperiments = this.query.CopyToDataTable<DataRow>();

            foreach (DataRow row in this.dtExperiments.Rows)
                {
                if (String.IsNullOrEmpty (row["WT_WEIGHT"].ToString ()) ||
                    String.IsNullOrEmpty (row["FISH_LENGTH"].ToString ()))
                    continue;

                if (cmbYAxis.SelectedItem.ToString ().Equals ("Fish Weight"))
                    yAxis.Add (Convert.ToDouble (row["WT_WEIGHT"]));
                else
                    yAxis.Add (Convert.ToDouble (row["FISH_LENGTH"]));

                xAxis.Add (row["COLOR"].ToString ());
                }

            aovTable = _rStats.doAnova (xAxis, yAxis);

            dgAnova.DataSource = aovTable;



            switch (cmbChartType.SelectedItem.ToString ())
                {
                case "2D Bar Chart":
                    ReturnValues = _rStats.BarPlot (xAxis, yAxis, titles, fonts, textRotation, fontColors);
                    break;

                case "3D Bar Chart":
                    break;

                case "Anova":
                    break;

                case "Box Plot":
                    ReturnValues = _rStats.BoxPlot (xAxis, yAxis, titles, fonts, textRotation, fontColors);
                    break;
                }


            int j = 0;
            rtbSummary.Clear ();

            for (int i = 0; i < ReturnValues.Count - 1; i++)
                {
                string setValues = "";

                switch (j)
                    {
                    case 0:
                        setValues = "\n";
                        break;
                    case 1:
                        setValues = "0%:  ";
                        break;
                    case 2:
                        setValues = "25%:  ";
                        break;
                    case 3:
                        setValues = "50%:  ";
                        break;
                    case 4:
                        setValues = "75%:  ";
                        break;
                    case 5:
                        setValues = "100%:  ";
                        break;
                    case 6:
                        setValues = "Standard Deviation:  ";
                        break;
                    case 7:
                        setValues = "Standard Error of the Means:  ";
                        break;

                    }
                rtbSummary.Text += setValues + ReturnValues[i].ToString () + "\n";
                j++;
                if (j == 8)
                    j = 0;

                }

            sImagePath = ReturnValues[ReturnValues.Count - 1].ToString ();

            if (String.IsNullOrEmpty (sImagePath))
                return;

            //Open file in read only mode
            using (FileStream stream = new FileStream (sImagePath, FileMode.Open, FileAccess.Read))
            //Get a binary reader for the file stream
            using (BinaryReader reader = new BinaryReader (stream))
                {
                //copy the content of the file into a memory stream
                var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                //make a new Bitmap object the owner of the MemoryStream
                Bitmap imgChart = new Bitmap(memoryStream);


                pbChart.Image = imgChart;
                pbChart.SizeMode = PictureBoxSizeMode.StretchImage;
                }

            txtIncline.Enabled = false;
            txtPerspective.Enabled = false;
            txtRotation.Enabled = false;
            txtPointWidth.Enabled = false;
            }

        private void SetChartComboBox ()
            {
            cmbChartType.Items.Clear ();
            cmbChartType.Items.Add ("Scatter Plot");
            cmbChartType.Items.Add ("2D Bar Chart");
            cmbChartType.Items.Add ("3D Bar Chart");
            cmbChartType.Items.Add ("Box Plot");
            cmbChartType.Items.Add ("Histogram");
            cmbChartType.Items.Add ("Anova");
            cmbChartType.SelectedIndex = 1;
            }

        private void btnSetChart_Click (object sender, EventArgs e)
            {
            SetChart ();
            }

        private void frmData_FormClosing (object sender, FormClosingEventArgs e)
            {

            }

        private void btnTitleFont_Click (object sender, EventArgs e)
            {
            if (bLoad)
                return;
            fontDialog1.ShowColor = true;

            DialogResult result = fontDialog1.ShowDialog();

            if (result == DialogResult.OK)
                {
                // Get Font.
                fTitle = fontDialog1.Font;
                cTitle = fontDialog1.Color;
                }
            }

        private void btnLegendFont_Click (object sender, EventArgs e)
            {
            if (bLoad)
                return;
            fontDialog1.ShowColor = true;

            DialogResult result = fontDialog1.ShowDialog();

            if (result == DialogResult.OK)
                {
                // Get Font.
                fLegend = fontDialog1.Font;
                cLegend = fontDialog1.Color;
                }
            }

        private void btnAxisFont_Click (object sender, EventArgs e)
            {
            if (bLoad)
                return;
            fontDialog1.ShowColor = true;

            DialogResult result = fontDialog1.ShowDialog();

            if (result == DialogResult.OK)
                {
                // Get Font.
                fAxis = fontDialog1.Font;
                cAxis = fontDialog1.Color;
                }
            }

        private void btnTickFont_Click (object sender, EventArgs e)
            {
            if (bLoad)
                return;
            fontDialog1.ShowColor = true;

            DialogResult result = fontDialog1.ShowDialog();

            if (result == DialogResult.OK)
                {
                // Get Font.
                fTick = fontDialog1.Font;
                cTick = fontDialog1.Color;
                }
            }

        private void btnSaveChart_Click (object sender, EventArgs e)
            {
            saveFileDialog1 = new SaveFileDialog ();
            if (tabChart.SelectedIndex == 1)
                {
                saveFileDialog1.Filter = @"Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image 
                                        (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
                saveFileDialog1.Title = "Save Chart As Image File";
                DialogResult result = saveFileDialog1.ShowDialog();
                saveFileDialog1.RestoreDirectory = true;

                if (result == DialogResult.OK && saveFileDialog1.FileName != "")
                    {
                    pbChart.Image.Save (saveFileDialog1.FileName);
                    }
                }
            else
                {
                saveFileDialog1.Filter = @"Word Documents|*.doc";
                saveFileDialog1.Title = "Save Summary";
                DialogResult result = saveFileDialog1.ShowDialog();
                saveFileDialog1.RestoreDirectory = true;

                if (result == DialogResult.OK && saveFileDialog1.FileName != "")
                    {
                    rtbSummary.SaveFile (saveFileDialog1.FileName);
                    }
                }


            }

        private void dgAnova_DataSourceChanged (object sender, EventArgs e)
            {
            foreach (DataGridViewRow row in dgAnova.Rows)
                {
                if (row.Index == dgAnova.Rows.Count - 1)
                    break;
                else
                    {
                    if (Convert.ToDouble (dgAnova["P Adjustment", row.Index].Value.ToString ()) > -0.051
                        && Convert.ToDouble (dgAnova["P Adjustment", row.Index].Value.ToString ()) < 0.051)
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }


        }
    }