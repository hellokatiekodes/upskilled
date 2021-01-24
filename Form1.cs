using System;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace Simple_Stock_IT_App
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source=KATIE-TEA-XPS" + "\\" + "SQLEXPRESS;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM StockITDB.dbo.StockListMain", sqlCon);
                DataTable csvdata = new DataTable();
                sqlDa.Fill(csvdata);

                dgItems.DataSource = csvdata;

            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(dgItems.Text);
                }
            }

            DialogResult res = MessageBox.Show("Are you sure you want to Save", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                //showing file has been saved successfullly
                MessageBox.Show("CSV file saved successfully");
            }
            if (res == DialogResult.Cancel)
            {
                //showing CSV file not saved properly if user clicks çancel 
                MessageBox.Show("CSV file not saved");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
 