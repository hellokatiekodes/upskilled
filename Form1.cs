using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.DAL;

namespace Simple_Stock_IT_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                int ImportedRecord = 0, inValidItem = 0;
                string SourceURl = "";

                if (dialog.FileName != "")
                {
                    if (dialog.FileName.EndsWith(".csv"))
                    {

                        


                        DataTable dtNew = new DataTable();
                        DataGrid dgItems = new DataGrid();

                        dtNew = GetDataTabletFromCSVFile(dialog.FileName);
                        if (Convert.ToString(dtNew.Columns[0]).ToLower() != "lookupcode")
                        {
                            MessageBox.Show("Invalid Items File");
                            btnSave.Enabled = false;
                            return;
                        }
                        txtFile.Text = dialog.SafeFileName;
                        SourceURl = dialog.FileName;
                        if (dtNew.Rows != null && dtNew.Rows.ToString() != String.Empty)
                        {
                            dgItems.DataSource = dtNew;
                        }
                        foreach (DataGridViewRow row in dgItems.Rows)
                        {
                            if (Convert.ToString(row.Cells["ItemCode"].Value) == "" || row.Cells["ItemCode"].Value == null
                                || Convert.ToString(row.Cells["ItemDescription"].Value) == "" || row.Cells["ItemDescription"].Value == null
                                || Convert.ToString(row.Cells["CurrentCount"].Value) == "" || row.Cells["CurrentCount"].Value == null
                                || Convert.ToString(row.Cells["OnOrder"].Value) == "" || row.Cells["OnOrder"].Value == null)
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                                inValidItem += 1;
                            }
                            else
                            {
                                ImportedRecord += 1;
                            }
                        }
                        if (dgItems.Rows.Count == 0)
                        {
                            btnSave.Enabled = false;
                            MessageBox.Show("There is no data in this file", "GAUTAM POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected File is Invalid, Please Select valid csv file.", "GAUTAM POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex);
            }
        }

        //to create GetDataTabletfromCSVFile function 


        public static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                if (csv_file_path.EndsWith(".csv"))
                {
                    using (Microsoft.VisualBasic.FileIO.TextFieldParser csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(csv_file_path))
                    {
                        csvReader.SetDelimiters(new string[] { "," });
                        csvReader.HasFieldsEnclosedInQuotes = true;
                        //we are reading the column
                        string[] colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        while (!csvReader.EndOfData)
                        {
                            string[] fieldData = csvReader.ReadFields();
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }
                            csvData.Rows.Add(fieldData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exce " + ex);
            }
            return csvData;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtItem = (DataTable)(dgItems.DataSource);
                string Lookup, description, dept, UnitPrice;
                string InsertItemQry = "";
                int count = 0;
                foreach (DataRow dr in dtItem.Rows)
                {
                    Lookup = Convert.ToString(dr["LookupCode"]);
                    description = Convert.ToString(dr["ItemName"]);
                    dept = Convert.ToString(dr["DeptId"]);
                    UnitPrice = Convert.ToString(dr["Price"]);
                    if (Lookup != "" && description != "" && dept != "" && UnitPrice != "")
                    {
                        InsertItemQry += "Insert into tbItem(LookupCode,ItemName,DeptId,CateId,Cost,Price, Quantity, UOM, Weight, TaxID, IsDiscountItem,EntryDate)Values('" + Lookup + "','" + description + "','" + dept + "','" + dr["CateId"] + "','" + dr["Cost"] + "','" + UnitPrice + "'," + dr["Quantity"] + ",'" + dr["UOM"] + "','" + dr["Weight"] + "','" + dr["TaxID"] + "','" + dr["IsDiscountItem"] + "',GETDATE()); ";
                        count++;
                    }
                }
                if (InsertItemQry.Length > 5)
                {
                    bool isSuccess = DBAccess.ExecuteQuery(InsertItemQry);
                    if (isSuccess)
                    {
                        MessageBox.Show("Item Imported Successfully, Total Imported Records : " + count + "", "GAUTAM POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgItems.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex);
            }
        }
    }
}

